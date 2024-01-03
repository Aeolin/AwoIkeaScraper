using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwoIkeaScraper.Scraper.Ikea.Core;

namespace AwoIkeaScraper.Scraper.Ikea.Results
{
    public class FailedResult : IScrapeResult
	{
		public IEnumerable<ScrapeJob> Jobs => Enumerable.Empty<ScrapeJob>();

		public bool Failed => true;

		public Exception Exception { get; }

		public object Data => null;

		public FailedResult(Exception exception = null)
		{
			Exception = exception;
		}

	}
}
