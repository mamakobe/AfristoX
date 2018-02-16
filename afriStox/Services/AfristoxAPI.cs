using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using afriStox.Models;
using Newtonsoft.Json;

namespace afriStox.Services
{
    public class AfristoxAPI
    {
        HttpClient client;
        //http://localhost:5000/api/values
        //The URL of the WEB API Service
        string url = "http://localhost:5000/api/values";
        public AfristoxAPI()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<ShareTrades>> TradesAsync()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                List<ShareTrades> _stocks = JsonConvert.DeserializeObject<List<ShareTrades>>(responseData);
                return _stocks;
            }
            return null;
        }
    }
}
