using AwoIkeaScraper.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Shared
{
	public class ScrapingContext : DbContext
	{

		public ScrapingContext() : base()
		{

		}

		public ScrapingContext(DbContextOptions<ScrapingContext> options) : base(options) 
		{
		
		}


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (optionsBuilder.IsConfigured == false)
				optionsBuilder.UseNpgsql();

			base.OnConfiguring(optionsBuilder);
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>().HasOne(x => x.Currency).WithMany();
			base.OnModelCreating(modelBuilder);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
			var now = DateTime.UtcNow;
			foreach (var entry in entries)
			{
				if (entry.Entity is EntityBase trackable)
				{
					switch (entry.State)
					{
						case EntityState.Added:
							trackable.CreatedAt = now;
							trackable.UpdatedAt = now;
							break;
						case EntityState.Modified:
							trackable.UpdatedAt = now;
							break;
					}
				}
			}
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		public DbSet<Currency> Currencies { get; set; } 
		public DbSet<ScrapeError> Errors { get; set; }
		public DbSet<ScrapeEvent> Events { get; set; }
		public DbSet<Product> Products { get; set; }

	}
}
