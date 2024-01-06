using AwoIkeaScraper.Scraper.Ikea.Core;
using AwoIkeaScraper.Scraper.Ikea.Results;
using AwoIkeaScraper.Shared;
using AwoIkeaScraper.Shared.Models;
using AwosFramework.Multithreading.Runners;
using Microsoft.Extensions.Logging;
using ReInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea
{
    public class IkeaScraper
	{
		private readonly ScrapingContext _db;
		private readonly ILogger _logger;
		private readonly IDependencyContainer _container;
		private readonly IkeaScraperConfiguration _config;
		private readonly RunnerGroup<ScrapeJob, IScrapeResult, ScrapeEngine> _runners;
		private readonly List<Product> _products = new List<Product>();

		public IkeaScraper(ScrapingContext db, ILoggerFactory loggerFactory, IkeaScraperConfiguration config, IDependencyContainer container)
		{
			_db = db;
			_logger = loggerFactory.CreateLogger<IkeaScraper>();
			_runners = new RunnerGroup<ScrapeJob, IScrapeResult, ScrapeEngine>("Ikea Scraper", (e, j) => e.ScrapeAsync(j).Result);
			_runners.OnError +=_runners_OnError;
			_runners.OnResult +=_runners_OnResult;
			_config = config;
			_container = container;
		}

		private void _runners_OnResult(object sender, IScrapeResult result)
		{
			if(_products.Count > 1000)
			{
				_runners.Dispose();
				_runners.Jobs.Clear();
				_runners.GetType().GetMethod("fireDone", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_runners, null);
				File.WriteAllText("products.json", JsonSerializer.Serialize(_products));
				_logger.LogInformation("got 1000 products, shutting down...");
			}

			if (result.Jobs != null)
				_runners.QueueJobs(result.Jobs);

			if (result.Data != null)
				_products.Add((Product)result.Data);
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
				new Runner<ScrapeJob, IScrapeResult, ScrapeEngine>(_runners, _container.GetInstance<ScrapeEngine>());
			}

			_runners.StartAll();
			var job = new ScrapeJob("https://www.ikea.com/ch/de/", "main-page");
			_runners.QueueJob(job);
			await _runners.AwaitAllDone(); // await first crawls
		}

	}
}
