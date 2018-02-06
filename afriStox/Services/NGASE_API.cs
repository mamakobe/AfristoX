using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using afriStox.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace afriStox.Services
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class NGASE_API
    {
        HttpClient client;
        //The URL of the WEB API Service
        string url = "https://www.nigerianelite.com/api/stocks";
        public NGASE_API()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<StocksNigeria>> GhanaStockListAsync()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                List<StocksNigeria> _stocks = JsonConvert.DeserializeObject<List<StocksNigeria>>(responseData);
                return _stocks;
            }
            return null;
        }
    }
}
