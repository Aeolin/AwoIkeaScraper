using ReInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public static class Extensions
	{
		public static IDependencyContainer MapScrapeControllers(this IDependencyContainer container)
		{
			var controllers = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.IsAssignableTo(typeof(ScrapeController)) && x.IsAbstract == false)
				.ToArray();

			var methods = controllers
				.SelectMany(x => x.GetMethods())
				.Where(x => x.GetCustomAttribute<RouteAttribute>() != null)
				.Select(x => new { Method = x, Route = x.GetCustomAttribute<RouteAttribute>() })
				.ToArray();

			var router = new ScrapeRouter();
			foreach(var method in methods)
			{
				var generic = typeof(ScrapeRoute<>).MakeGenericType(method.Method.DeclaringType);
				var route =  (IScrapeRoute)generic.GetConstructors().First().Invoke(new object[] { method.Method, method.Method.DeclaringType, method.Route.Path, method.Route.Tag });
				router.AddRoute(route);
			}

			container.AddSingleton(router);
			return container;
		}
	}
}
