using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea
{
	public abstract class ScrapeJob
	{
		public int RetryCount { get; set; }
		public Guid Id { get; init; }
		public Uri Uri { get; init; }


		public ScrapeJob(string uri)
		{
			RetryCount=0;
			Id = Guid.NewGuid();
			Uri=new Uri(uri);
		}
	}
}
