﻿using System;
namespace afristoxAPI.Models
{
    public class PriceOpen
    {
        public Guid priceopen_id { get; set; }
        public Guid marketday_id { get; set; }
        public decimal price_open { get; set; }
        public Guid ticker_id { get; set; }
    }
}
