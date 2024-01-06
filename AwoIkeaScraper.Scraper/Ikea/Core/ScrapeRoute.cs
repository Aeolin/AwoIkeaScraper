using AwoIkeaScraper.Scraper.Ikea.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public class ScrapeRoute<T> : IScrapeRoute where T : ScrapeController 
	{
		public MethodInfo MethodInfo { get; private set; }
		public Type ControllerType { get; private set; }	
		public Regex UriMatcher { get; private set; }
		public Regex TagMatcher { get; private set; }

		public ScrapeRoute(MethodInfo methodInfo, Type controllerType, string uriMatcher, string tagMatcher) : 
			this(methodInfo, 
				controllerType, 
				uriMatcher == null ? null : new Regex(uriMatcher, RegexOptions.Compiled), 
				tagMatcher == null ? null : new Regex(tagMatcher, RegexOptions.Compiled))
		{

		}

		public ScrapeRoute(MethodInfo methodInfo, Type controllerType, Regex uriMatcher, Regex tagMatcher)
		{
			MethodInfo=methodInfo;
			ControllerType=controllerType;
			UriMatcher=uriMatcher;
			TagMatcher=tagMatcher;

			if (methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>) && methodInfo.ReturnType.GenericTypeArguments[0].IsAssignableTo(typeof(IScrapeResult)))
				AsyncHandler = (ScrapeDelegateAsync)Delegate.CreateDelegate(typeof(ScrapeDelegateAsync), methodInfo);
			else if(methodInfo.ReturnType.IsAssignableTo(typeof(IScrapeResult)))
				Handler = (ScrapeDelegate)Delegate.CreateDelegate(typeof(ScrapeDelegate), methodInfo);
			else
				throw new ArgumentException("Invalid return type for scrape method, expected IScrapeResult");
		}

		private delegate Task<IScrapeResult> ScrapeDelegateAsync(T controller, ScrapeJob job);
		private delegate IScrapeResult ScrapeDelegate(T controller, ScrapeJob job);

		private ScrapeDelegateAsync AsyncHandler = null;
		private ScrapeDelegate Handler = null;

		public bool CanHandle(ScrapeJob job)
		{
			return UriMatcher?.IsMatch(job.Uri.AbsolutePath) ?? false || (job.Tag != null && (TagMatcher?.IsMatch(job.Tag) ?? false));
		}

		public async Task<IScrapeResult> HandleAsync(ScrapeController controller, ScrapeJob job)
		{
			if (AsyncHandler != null)
				return await AsyncHandler((T)controller, job);

			return Handler((T)controller, job);
		}
	}
}
