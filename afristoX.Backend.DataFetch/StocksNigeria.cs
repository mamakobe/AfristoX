using System;
namespace afristoX.Backend.DataFetch
{
    public class StocksNigeria
    {
        public string ticker { get; set; }
        public string company { get; set; }
        public DateTime date { get; set; }
        public double? price { get; set; }
        public double? volume { get; set; }
        public double? marketCap { get; set; }
        public double? sharesOut { get; set; }
        public double? dividend { get; set; }
        public double? yield { get; set; }
        public double? pe { get; set; }
        public double? eps { get; set; }
        public double? roi { get; set; }
        public double? roe { get; set; }
    }
}
