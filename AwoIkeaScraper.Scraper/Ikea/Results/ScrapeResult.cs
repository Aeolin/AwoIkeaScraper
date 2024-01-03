using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwoIkeaScraper.Scraper.Ikea.Core;

namespace AwoIkeaScraper.Scraper.Ikea.Results
{
    public class ScrapeResult<T> : IScrapeResult
	{
		public IEnumerable<ScrapeJob> Jobs { get; init; }
		public T Data { get; init; }
		public bool Failed => false;
		object IScrapeResult.Data => this.Data;
		public Exception Exception => null;


		public ScrapeResult(IEnumerable<ScrapeJob> jobs, T data)
		{
			Jobs = jobs;
			Data = data;
		}
	}
}
