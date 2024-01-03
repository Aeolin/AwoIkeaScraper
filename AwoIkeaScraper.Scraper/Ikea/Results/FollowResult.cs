using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Results
{
	public class FollowResult : IScrapeResult
	{
		public IEnumerable<ScrapeJob> Jobs { get; init; }

		public bool Failed => false;
		public Exception Exception => null;
		public object Data => null;

		public FollowResult(IEnumerable<ScrapeJob> jobs)
		{
			Jobs = jobs;
		}
	}
}
