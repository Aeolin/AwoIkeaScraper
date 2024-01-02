using AwoIkeaScraper.Scraper.Ikea.Jobs;
using AwoIkeaScraper.Shared.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea
{
	public class ScrapeEngine
	{
		private readonly HttpClient _client = new HttpClient();

		public async Task<ScrapeResult<Product>> ScrapeAsync(ScrapeJob job)
		{

		}

		private async Task<ScrapeResult<Product>> ScrapeMainPageAsync(ScrapeJobMainPage page)
		{
			var content = await _client.GetStringAsync(page.Uri);
			var doc = new HtmlDocument();
			doc.LoadHtml(content);

		}
	}
}
