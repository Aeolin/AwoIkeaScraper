using AwoIkeaScraper.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Shared
{
	public class ScrapeError : EntityBase
	{ 
		public string Url { get; set; }
		public string Exception { get; set; }
		public virtual ScrapeEvent ScrapeEvent { get; set; }
	}
}
