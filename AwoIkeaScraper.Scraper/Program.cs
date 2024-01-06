using AwoIkeaScraper.Scraper;
using AwoIkeaScraper.Scraper.Ikea;
using AwoIkeaScraper.Scraper.Ikea.Core;
using AwoIkeaScraper.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReInject;
using ReInject.Interfaces;

var container = Injector.GetContainer();

var config = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.AddEnvironmentVariables()
	.Build();

var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole().AddConfiguration(config.GetSection("Logging")); });

container.AddSingleton<IConfiguration>(config);
container.AddSingleton<IDependencyContainer>(container);
container.AddSingleton<ILoggerFactory>(loggerFactory);
container.AddSingleton<IkeaScraperConfiguration>(config.GetSection("ScrapingSettings").Get<IkeaScraperConfiguration>());
//container.AddDbContext<ScrapingContext>(x => x.UseLazyLoadingProxies()
//	.UseLoggerFactory(loggerFactory)
//	.UseNpgsql(config.GetConnectionString("DefaultDatabase"))
//);

container.AddLazySingleton<ScrapingContext>(() => null);

container.MapScrapeControllers();
var scraper = container.GetInstance<IkeaScraper>();
await scraper.RunAsync();