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
    public class GHASE_API
    {
        HttpClient client;
        //The URL of the WEB API Service
        string url = "https://dev.kwayisi.org/apis/gse/live";
        public GHASE_API()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<StocksGhana>> GhanaStockListAsync()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                List<StocksGhana> _stocks = JsonConvert.DeserializeObject<List<StocksGhana>>(responseData);
                return _stocks;
            }
            return null;
        }
    }
}
