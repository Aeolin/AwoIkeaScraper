using AwoIkeaScraper.Scraper.Ikea.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Results
{
	public interface IScrapeResult
	{
		public IEnumerable<ScrapeJob> Jobs { get; }
		public bool Failed { get; }
		public Exception Exception { get; }
		public object Data { get; }
	}
}
