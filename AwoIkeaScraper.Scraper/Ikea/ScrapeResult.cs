using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea
{
	public class ScrapeResult<T>
	{
		public IEnumerable<ScrapeJob> Jobs { get; init; }
		public T Data { get; init; }

		private ScrapeResult(IEnumerable<ScrapeJob> jobs, T data)
		{
			Jobs=jobs;
			Data=data;
		}

		public static ScrapeResult<T> OfResult(T data) => new ScrapeResult<T>(null, data);
		public static ScrapeResult<T> Follow(params ScrapeJob[] jobs) => new ScrapeResult<T>(jobs, default);	
	}
}
