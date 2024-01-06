using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Shared.Models
{
	[Index(nameof(ProductNumber), nameof(ScrapeEventId), IsUnique = true)]
	public class Product : EntityBase
	{
		[JsonIgnore]
		public virtual Guid? ScrapeEventId { get; set; } = null;
		[JsonIgnore]
		public virtual ScrapeEvent ScrapeEvent { get; set; }

		public int? Length { get; set; }
		public int? Width { get; set; }
		public int? Height { get; set; }

		public string ProductNumber { get; set; }
		public string Url { get; set; }
		public string ImageUrl { get; set; }
		public string Name { get; set; }
		public string ShortDescription { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		[JsonIgnore]
		public virtual Currency Currency { get; set; }
		public string Category { get; set; }
	}
}
