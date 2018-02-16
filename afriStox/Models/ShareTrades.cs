using System;
namespace afriStox.Models
{
    public class ShareTrades
    {
        public string ticker { get; set; }
        public string companyName { get; set; } 
        public decimal openingPrice { get; set; }
        public decimal closingPrice { get; set; }
        public decimal priceChange { get; set; }
    }
}
