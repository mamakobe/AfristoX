using System;
namespace afristoxAPI.Models
{
    public class PriceChange
    {
        public Guid pricechange_id { get; set; }
        public Guid marketday_id { get; set; }
        public decimal price_change { get; set; }
        public Guid ticker_id { get; set; }
    }
}
