using AwoIkeaScraper.Scraper.Ikea.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public class ScrapeRouter
	{
		private readonly List<ScrapeRoute> _routes = new();

		public void AddRoute(ScrapeRoute route)
		{
			_routes.Add(route);
		}

		public ScrapeRoute Route(ScrapeJob job)
		{
			var route = _routes.FirstOrDefault(x => x.CanHandle(job));
			return route;
		}

	}
}
