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
		private readonly List<IScrapeRoute> _routes = new();

		public void AddRoute(IScrapeRoute route)
		{
			_routes.Add(route);
		}

		public IScrapeRoute Route(ScrapeJob job)
		{
			var route = _routes.FirstOrDefault(x => x.CanHandle(job));
			return route;
		}

	}
}
