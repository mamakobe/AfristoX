﻿using System;
namespace afristoxAPI.Models
{
    public class PriceClose
    {
        public Guid priceclose_id { get; set; }
        public Guid marketday_id { get; set; }
        public decimal price_close { get; set; }
        public Guid ticker_id { get; set; }
    }
}
