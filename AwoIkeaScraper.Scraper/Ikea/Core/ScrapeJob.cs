using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public abstract class ScrapeJob : IScrapeJob
	{
		public int RetryCount { get; set; }
		public Guid Id { get; init; }
		public Uri Uri { get; init; }

		public ScrapeJob(string uri)
		{
			RetryCount = 0;
			Id = Guid.NewGuid();
			Uri = new Uri(uri);
		}
	}
}
