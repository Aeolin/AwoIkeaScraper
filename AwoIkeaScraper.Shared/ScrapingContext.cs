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

		public DbSet<Currency> Currencies { get; set; } 
		public DbSet<ScrapeError> Errors { get; set; }
		public DbSet<ScrapeEvent> Events { get; set; }
		public DbSet<Product> Products { get; set; }

	}
}
