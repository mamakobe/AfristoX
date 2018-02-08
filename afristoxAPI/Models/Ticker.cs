using System;
namespace afristoxAPI.Models
{
	public class Ticker
	{
		public Guid ticker_id {get;set;}
        public Guid coexchange_id { get; set; }
        public string ticker { get; set; }
	}
}
