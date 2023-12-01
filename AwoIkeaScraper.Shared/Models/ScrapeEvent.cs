using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Shared.Models
{
	public class ScrapeEvent : EntityBase
	{
		public virtual IList<Product> GeneratedProducts { get; set; }
		public virtual IList<ScrapeError> Errors { get; set; }

		public DateTime ImportStarted { get; set; }
		public DateTime ImportEnded { get; set; }
	}
}
