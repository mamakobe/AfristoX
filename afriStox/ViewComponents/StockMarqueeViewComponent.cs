using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using afriStox.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace afriStox.ViewComponents
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class StockMarqueeViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
             AfristoxAPI _afristox = new AfristoxAPI();
            var results = await _afristox.TradesAsync();
            return View(results);

        }
    }
}
