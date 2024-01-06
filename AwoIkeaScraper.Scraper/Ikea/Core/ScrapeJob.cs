using AwoIkeaScraper.Scraper.Ikea.Results;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public class ScrapeJob : IScrapeJob
	{
		public int RetryCount { get; set; }
		public Guid Id { get; init; }
		public Uri Uri { get; init; }
		public HttpRequestMessage Request { get; set; }
		public string Tag { get; set; }
		public ScrapeJob Parent { get; init; }
		public IScrapeResult Result { get; set; }
		public object Data { get; set; }

		public T GetData<T>() => (T)Data;

		public ScrapeJob(string uri, string tag = null, ScrapeJob parent = null, object data = null, HttpRequestMessage request = null) : this(new Uri(uri), tag, parent, data, request)
		{
			
		}

		public ScrapeJob(Uri uri, string tag = null, ScrapeJob parent = null, object data = null, HttpRequestMessage request = null)
		{
			RetryCount = 0;
			Id = Guid.NewGuid();
			Uri = uri;
			Tag=tag;
			Parent = parent;
			Data=data;
			Request=request ?? new HttpRequestMessage(HttpMethod.Get, uri);
		}
	}
}
