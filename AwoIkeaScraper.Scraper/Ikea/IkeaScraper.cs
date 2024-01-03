using AwoIkeaScraper.Scraper.Ikea.Core;
using AwoIkeaScraper.Scraper.Ikea.Jobs;
using AwoIkeaScraper.Scraper.Ikea.Results;
using AwoIkeaScraper.Shared;
using AwoIkeaScraper.Shared.Models;
using AwosFramework.Multithreading.Runners;
using Castle.Core.Configuration;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea
{
    public class IkeaScraper
	{
		private readonly ScrapingContext _db;
		private readonly ILogger _logger;
		private readonly IkeaScraperConfiguration _config;
		private readonly RunnerGroup<ScrapeJob, ScrapeResult<Product>, ScrapeEngine> _runners;
		private readonly List<Product> _products = new List<Product>();

		public IkeaScraper(ScrapingContext db, ILoggerFactory loggerFactory, IkeaScraperConfiguration config)
		{
			_db = db;
			_logger = loggerFactory.Create(typeof(IkeaScraper));
			_runners = new RunnerGroup<ScrapeJob, ScrapeResult<Product>, ScrapeEngine>("Ikea Scraper", (e, j) => e.ScrapeAsync(j).Result);
			_runners.OnError +=_runners_OnError;
			_runners.OnResult +=_runners_OnResult;
			_config = config;
		}

		private void _runners_OnResult(object sender, ScrapeResult<Product> result)
		{
			if (result.Jobs != null)
				_runners.QueueJobs(result.Jobs);

			if (result.Data != null)
				_products.Add(result.Data);
		}

		private void _runners_OnError(object source, ScrapeJob input, Exception ex)
		{
			if (input.RetryCount++ < _config.MaxRetries)
				_runners.QueueJob(input);
		}

		public async Task RunAsync()
		{
			for (int i = 0; i < _config.MaxThreads; i++)
			{
				new Runner<ScrapeJob, ScrapeResult<Product>, ScrapeEngine>(_runners, new ScrapeEngine());
			}

			var job = new ScrapeJobMainPage("https://www.ikea.com/ch/de/");
			_runners.QueueJob(job);
			await _runners.AwaitAllDone(); // await first crawls
			await _runners.AwaitAllDone(); // await detail crawls
		}

	}
}
