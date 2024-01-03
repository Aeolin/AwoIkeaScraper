using AwoIkeaScraper.Scraper.Ikea.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public abstract class ScrapeController
	{
		private ScrapeEngine _engine;
		private ScrapeJob _currentJob;
		
		protected ScrapeController(ScrapeEngine engine)
		{
			_engine = engine;
		}

		public IScrapeResult Follow(IEnumerable<ScrapeJob> jobs) => new FollowResult(jobs);

		public IScrapeResult OkFollow<T>(IEnumerable<ScrapeJob> jobs, T data) => new ScrapeResult<T>(jobs, data);

		public IScrapeResult Ok<T>(T data) => new ScrapeResult<T>(null, data);

		public IScrapeResult Fail(Exception exception) => new FailedResult(exception);
	}
}
