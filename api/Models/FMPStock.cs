using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api.Models
{
    public class FMPStock
    {
        public int Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonProperty("companyName")]
        public string CompanyName { get; set; } = string.Empty;

        [JsonProperty("price")]
        public decimal Purchase { get; set; } // Maps API's "price" to your Purchase field

        [JsonProperty("lastDividend")]
        public decimal LastDiv { get; set; } // Maps API's "lastDividend" to your LastDiv field

        [JsonProperty("industry")]
        public string Industry { get; set; } = string.Empty;

        [JsonProperty("marketCap")]
        public long MarketCap { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}