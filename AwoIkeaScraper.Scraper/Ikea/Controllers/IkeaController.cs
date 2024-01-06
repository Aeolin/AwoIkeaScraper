using AwoIkeaScraper.Scraper.Ikea.Core;
using AwoIkeaScraper.Scraper.Ikea.Results;
using AwoIkeaScraper.Shared.Models;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Controllers
{
	public class IkeaController : ScrapeController
	{
		private static readonly Regex CategoryRegex = new Regex(@"-(\w{2}[0-9]{3})", RegexOptions.Compiled);
		private const int ITEM_BATCH_SIZE = 100;

		[Route(Tag = "main-page")]
		public IScrapeResult ScrapeMainPageAsync(ScrapeJob job)
		{
			var products = Content.DocumentNode.QuerySelector("div .hnf-loading");
			var links = products.QuerySelectorAll("a .hnf-link");
			var jobs = links.Select(x => new ScrapeJob(x.Attributes["href"].Value, "category", job, 0));
			return Follow(jobs);
		}

		[Route(Tag = "category")]
		public async Task<IScrapeResult> ScrapeCategoryAsync(ScrapeJob job)
		{
			const string SORT_BY = /*"NAME_ASCENDING"*/"RELEVANCE";
			var categorySegment = job.Uri.Segments.Last();
			var category = CategoryRegex.Match(categorySegment).Groups[1].Value;
			int offset = job.GetData<int?>() ?? 0;
			var jobs = new List<ScrapeJob>();

			var request = $$"""
						{
				    "searchParameters": {
				        "input": "{{category}}",
				        "type": "CATEGORY"
				    },
				    "isUserLoggedIn": false,
				    "partyUId": null,
				    "components": [
				        {
				            "component": "PRIMARY_AREA",
				            "columns": 4,
				            "types": null,
				            "filterConfig": {
				                "max-num-filters": 4
				            },
				            "sort": "{{SORT_BY}}",
				            "window": {
				                "size": {{ITEM_BATCH_SIZE}},
				                "offset": {{offset}}
				            }
				        }
				    ]
				}
				""";
			var date = DateTime.Now.ToString("yyyyMMdd");
			var response = await HttpClient.PostAsync($"https://sik.search.blue.cdtapps.com/ch/de/search?c=listaf&v=20231027", new StringContent(request));
			var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
			var result = document.RootElement.GetProperty("results")[0];
			var items = result.GetProperty("items").EnumerateArray();
			foreach (var item in items)
			{
				if (item.TryGetProperty("product", out var product) == false)
					product = item.GetProperty("featuredProduct");

				var url = product.GetProperty("pipUrl").GetString();
				var itemId = product.GetProperty("itemNoGlobal").GetString();
				var infoRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.ingka.ikea.com/salesitem/communications/ru/ch?itemNos={itemId}");
				infoRequest.Headers.Add("X-Client-Id", "c4faceb6-0598-44a2-bae4-2c02f4019d06");
				jobs.Add(new ScrapeJob(url, "product", job, itemId, infoRequest));
			}

			var max = result.GetProperty("metadata").GetProperty("max").GetInt32();
			if (offset < max)
				jobs.Add(new ScrapeJob(job.Uri, "category", job, offset + ITEM_BATCH_SIZE));

			return Follow(jobs);
		}

		[Route(Tag = "product")]
		public async Task<IScrapeResult> ScrapeProductAsync(ScrapeJob job)
		{
			var id = job.GetData<string>();
			var item = JsonContent.RootElement.GetProperty("data")[0];
			var name = item.GetProperty("productNameGlobal").GetString();
			var communications = item.GetProperty("localisedCommunications");
			var communication = communications.EnumerateArray().FirstOrDefault(x => x.GetProperty("languageCode").GetString() == "de");
			var shortDescription = communication.GetProperty("productType").GetProperty("name").GetString();
			var description = string.Join("\n\n", communication.GetProperty("benefits").EnumerateArray().Select(x => x.GetString()));
			var dimensions = communication.GetProperty("measurements").GetProperty("detailedMeasurements").EnumerateArray();
			var widthText = dimensions.Cast<JsonElement?>().FirstOrDefault(x => x?.GetProperty("type").GetString() == "00047")?.GetProperty("textMetric").GetString();
			var lengthText = dimensions.Cast<JsonElement?>().FirstOrDefault(x => { var type = x?.GetProperty("type").GetString(); return type == "00044" || type == "00001"; })?.GetProperty("textMetric").GetString();
			var heightText = dimensions.Cast<JsonElement?>().FirstOrDefault(x => x?.GetProperty("type").GetString() == "00041")?.GetProperty("textMetric").GetString();
			var category = item.GetProperty("businessStructure").GetProperty("productRangeAreaName").GetString();
			var mainImages = communication.GetProperty("media").EnumerateArray().Cast<JsonElement?>().FirstOrDefault(x => x?.GetProperty("typeName").GetString() == "MAIN_PRODUCT_IMAGE");
			var image = mainImages?.GetProperty("variants").EnumerateArray().Last().GetProperty("href").GetString();

			var priceResponse = await HttpClient.GetAsync($"https://ikea.com/ch/de/products/{id[5..]}/s{id}.json");
			if(priceResponse.StatusCode == HttpStatusCode.NotFound)
				priceResponse = await HttpClient.GetAsync($"https://ikea.com/ch/de/products/{id[5..]}/{id}.json");

			var priceDocument = JsonDocument.Parse(await priceResponse.Content.ReadAsStringAsync());
			var price = priceDocument.RootElement.GetProperty("priceNumeral").GetDecimal();
			var currency = priceDocument.RootElement.GetProperty("currencyCode").GetString();
			var pipUrl = priceDocument.RootElement.GetProperty("pipUrl").GetString();

			var product = new Product {
				Price = price, 
				Description = description, 
				Name = name, 
				ShortDescription = shortDescription, 
				Width = widthText != null ? int.Parse(widthText.Split(' ').First()) : null, 
				Length = lengthText != null ? int.Parse(lengthText.Split(' ').First()) : null, 
				Height = heightText != null ? int.Parse(heightText.Split(' ').First()) : null, 
				Category = category, 
				ImageUrl = image,
				ProductNumber = id,
				Url = pipUrl
			};

			return Ok(product);
		}
	}
}
