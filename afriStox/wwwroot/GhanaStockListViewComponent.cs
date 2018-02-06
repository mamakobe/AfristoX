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
    public class NigeriaStockListViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            NGASE_API _ghana = new NGASE_API();
            var results = await _ghana.GhanaStockListAsync();
            return View(results);

        }
    }
}
