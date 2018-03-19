using System;
using System.ComponentModel.DataAnnotations;

namespace afriStox.Models
{
    public class ShareTrades
    {
        public decimal close { get; set; }
        public Guid company_id { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime date { get; set; }
        public decimal high { get; set; }
        public decimal open { get; set; }
        public decimal low { get; set; }
        public int volume { get; set; }
        public string company_name { get; set; }
        public Guid exchange_id { get; set; }
        public Guid id { get; set; }
    }
}
