using AwoIkeaScraper.Scraper.Ikea.Results;
using AwoIkeaScraper.Shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using ReInject;
using ReInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public class ScrapeEngine
	{
		private readonly HttpClient _client = new HttpClient();
		private IDependencyContainer _container;
		private ILogger _logger;
		private ScrapeRouter _router;

		public ScrapeEngine(IDependencyContainer container, ILoggerFactory factory, ScrapeRouter router)
		{
			_container = Injector.NewContainer(container);
			_client = _container.GetInstance<HttpClient>();
			_logger = factory.CreateLogger<ScrapeEngine>();
			_router = router;
		}

		public async Task<IScrapeResult> ScrapeAsync(ScrapeJob job)
		{
			var route = _router.Route(job);
			if(route == null)
			{
				return new FailedResult();
			}

			var type = route.ControllerType;
			var controller = (ScrapeController)_container.GetInstance(type);
			var response = await _client.SendAsync(job.Request);
			HtmlDocument html = null;
			JsonDocument json = null;

			if(response.Content.Headers.ContentType.MediaType == "text/html")
			{
				var rhtml = await response.Content.ReadAsStringAsync();
				html = new HtmlDocument();
				html.LoadHtml(rhtml);
			}

			if(response.Content.Headers.ContentType.MediaType == "application/json" || response.Content.Headers.ContentType.MediaType == "application/vnd.ikea.api+json")
			{
				var rjson = await response.Content.ReadAsStringAsync();
				json = JsonDocument.Parse(rjson);
			}

			controller.Setup(html, json, _client);
			var result = await route.HandleAsync(controller, job);
			_logger.LogInformation("Executed job[{0}]({1}) for url {2}, success: {3}", job.Id, job.Data, job.Uri, !result.Failed);
			return result;
		}
	}
}
