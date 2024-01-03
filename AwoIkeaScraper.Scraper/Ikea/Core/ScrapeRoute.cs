using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public class ScrapeRoute
	{
		public MethodInfo MethodInfo { get; internal set; }
		public Type ControllerType { get; internal set; }
			
		internal bool CanHandle(ScrapeJob job)
		{
			throw new NotImplementedException();
		}
	}
}
