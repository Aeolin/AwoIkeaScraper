using Microsoft.EntityFrameworkCore;
using ReInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper
{
	public static class Extensions
	{
		public static IDependencyContainer AddDbContext<T> (this IDependencyContainer container, Action<DbContextOptionsBuilder<T>> configure = null) where T : DbContext
		{
			var builder = new DbContextOptionsBuilder<T>();
			configure?.Invoke(builder);
			var settings = builder.Options;
			container.AddSingleton(settings);
			container.AddTransient<T>();
			return container;
		}
	}
}
