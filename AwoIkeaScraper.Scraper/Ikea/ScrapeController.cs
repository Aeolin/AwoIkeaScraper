using AwoIkeaScraper.Scraper.Ikea.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea
{
	public abstract class ScrapeController
	{
		private ScrapeEngine _engine;
		private ScrapeJob _currentJob;
		private IScrapeResult _result;

		protected ScrapeController(ScrapeEngine engine)
		{
			_engine=engine;
		}

		public async Task<IScrapeResult> ScrapeAsync(ScrapeJob job)
		{
			_currentJob = job;
			_result = null;
			//await _engine.ScrapeAsync(job, this);
			if (_result is null)
				throw new InvalidOperationException("ScrapeAsync did not set a result");

			return _result;
		}

		public void Follow(IEnumerable<ScrapeJob> jobs)
		{
			_result = new FollowResult(jobs);
		}

		public void OkFollow<T>(IEnumerable<ScrapeJob> jobs, T data)
		{
			_result = new ScrapeResult<T>(jobs, data);
		}

		public void Ok<T>(T data)
		{
			_result = new ScrapeResult<T>(null, data);
		}

		public void Fail(Exception exception)
		{
			_result = new FailedResult(exception);
		}
	}
}
