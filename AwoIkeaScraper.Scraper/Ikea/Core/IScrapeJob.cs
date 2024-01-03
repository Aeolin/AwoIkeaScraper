
namespace AwoIkeaScraper.Scraper.Ikea.Core
{
	public interface IScrapeJob
	{
		Guid Id { get; init; }
		int RetryCount { get; set; }
		Uri Uri { get; init; }
	}
}