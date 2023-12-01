using AwoIkeaScraper.Scraper;
using AwoIkeaScraper.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReInject;

var container = Injector.GetContainer();

var config = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.AddEnvironmentVariables()
	.Build();

var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole().AddConfiguration(config.GetSection("Logging")); });

container.AddSingleton<IConfiguration>(config);
container.AddSingleton<ILoggerFactory>(loggerFactory);
container.AddDbContext<ScrapingContext>(x => x.UseLazyLoadingProxies()
	.UseLoggerFactory(loggerFactory)
	.UseNpgsql(config.GetConnectionString("DefaultDatabase"))
);