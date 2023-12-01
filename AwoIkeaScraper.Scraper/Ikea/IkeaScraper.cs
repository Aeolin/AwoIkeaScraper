using AwoIkeaScraper.Shared;
using AwosFramework.Multithreading.Runners;
using Castle.Core.Configuration;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Scraper.Ikea
{
    public class IkeaScraper
    {
        private readonly ScrapingContext _db;
        private readonly ILogger _logger;
        private readonly IkeaScraperConfiguration _config;

        public IkeaScraper(ScrapingContext db, ILoggerFactory loggerFactory, IkeaScraperConfiguration config)
        {
            _db = db;
            _logger = loggerFactory.Create(typeof(IkeaScraper));
            _config = config;
        }
    }
}
