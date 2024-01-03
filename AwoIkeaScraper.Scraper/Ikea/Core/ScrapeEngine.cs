using AwoIkeaScraper.Scraper.Ikea.Jobs;
using AwoIkeaScraper.Scraper.Ikea.Results;
using AwoIkeaScraper.Shared.Models;
using HtmlAgilityPack;
using ReInject;
using ReInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public class ScrapeEngine
	{
		private readonly HttpClient _client = new HttpClient();
		private IDependencyContainer _container;
		private ScrapeRouter _router;

		public ScrapeEngine(IDependencyContainer container, ScrapeRouter router)
		{
			_container = Injector.NewContainer(container);
			_client = _container.GetInstance<HttpClient>();
			_router = router;
		}

		public Task<IScrapeResult> ScrapeAsync(ScrapeJob job)
		{
			var route = _router.Route(job);

		}
	}
}
