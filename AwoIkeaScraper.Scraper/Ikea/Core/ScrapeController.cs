using AwoIkeaScraper.Scraper.Ikea.Results;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public abstract class ScrapeController
	{
		protected HtmlDocument Content { get; private set; }
		protected JsonDocument JsonContent { get; private set; }
		protected HttpClient HttpClient { get; private set; }
		
		public void Setup(HtmlDocument content, JsonDocument json, HttpClient client)
		{
			Content = content;
			JsonContent = json;
			HttpClient = client;
		}

		public IScrapeResult Follow(IEnumerable<ScrapeJob> jobs) => new FollowResult(jobs);

		public IScrapeResult OkFollow<T>(IEnumerable<ScrapeJob> jobs, T data) => new ScrapeResult<T>(jobs, data);

		public IScrapeResult Ok<T>(T data) => new ScrapeResult<T>(null, data);

		public IScrapeResult Fail(Exception exception) => new FailedResult(exception);
	}
}
