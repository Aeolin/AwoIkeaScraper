using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea
{
	public class IkeaScraperConfiguration
	{
		public int MaxThreads { get; set; } = Environment.ProcessorCount;
		public int BulkSize { get; set; } = 1000;
		public int MaxRetries { get; set; } = 3;
	}
}
