using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	[AttributeUsage(AttributeTargets.Method)]
	public class RouteAttribute : Attribute
	{
		public string Tag { get; set; }
		public string Path { get; set; }
	}
}
