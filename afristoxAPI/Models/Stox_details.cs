using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace afristoxAPI.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Stox_details
    {
        public decimal close { get; set; }
        public Guid company_id { get; set; }
        public DateTime date { get; set; }
        public decimal high { get; set; }
        public decimal open { get; set; }
        public decimal low { get; set;} 
        public int volume{ get; set; }
        public string company_name { get; set; }
        public Guid exchange_id { get; set; }
        public Guid id { get; set; }


    }

}
