using System;
namespace afristoxAPI.Helpers
{
    public class PriceByTicker
    {
        public string Ticker { get; set; }
        public string CompanyName { get; set; }
        public decimal? OpeningPrice { get; set; }
        public decimal? ClosingPrice { get; set; }
        public decimal? PriceChange { get; set; }
        
    }
}
