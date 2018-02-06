﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace afriStox.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class StocksGhana
    {
        public string name { get; set; }
        public double change { get; set; }
        public double price { get; set; }
        public double volume { get; set; }
       
    }
}
