using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoIkeaScraper.Shared.Models
{
	public class Currency
	{
		[Key]
		public string CurrencyCode { get; set; }
	}
}
