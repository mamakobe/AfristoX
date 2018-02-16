using System;
using ScrapySharp.Network;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using Cassandra;
using Cassandra.Data.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace afristoX.Backend.DataFetch
{

    class MainClass
    {
       
        public static void Main(string[] args)
        {

            ScrapGhanaSE();

        }

        public static async Task GetNaigeriaDataAsync()
        {
           // Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            //Console.WriteLine("Connecting to Afristox Keyspace!");
            //ISession session = cluster.Connect("afristox");
            //Console.WriteLine("Connected to Afristox Keyspace!");

            HttpClient client;
            //The URL of the WEB API Service
            string url = "https://www.nigerianelite.com/api/stocks";
            string urlyesterday = "https://www.nigerianelite.com/api/NSE?date=" + DateTime.Today.AddDays(-1);
            // string _returnvalue ="";
            client = new HttpClient();
            client.BaseAddress = new Uri(urlyesterday);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            List<Guid> _ticker = new List<Guid>();
            Guid MarketDayID = GetMarketDayID();
            DataTable dt = new DataTable();
            dt.Columns.Add("Ticker_id", typeof(Guid));
            dt.Columns.Add("Ticker", typeof(string));
            dt.Columns.Add("PriceOpen", typeof(decimal));
            dt.Columns.Add("PriceOpenID", typeof(Guid));
            dt.Columns.Add("PriceClose", typeof(decimal));
            dt.Columns.Add("PriceCloseID", typeof(Guid));
            dt.Columns.Add("PriceChange", typeof(decimal));
            dt.Columns.Add("PriceChangeID", typeof(Guid));
            dt.Columns.Add("MarketDayID", typeof(Guid));


            HttpResponseMessage responseMessage = await client.GetAsync(urlyesterday);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

              //  List<StocksNigeria> _stocks = JsonConvert.DeserializeObject<List<StocksNigeria>>(responseData);
                //return _stocks;
            }

        }
        //Get Data From GSE Website
        public static void ScrapGhanaSE()
        {
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            Console.WriteLine("Connecting to Afristox Keyspace!");
            ISession session = cluster.Connect("afristox");
            Console.WriteLine("Connected to Afristox Keyspace!");
            // string _returnvalue ="";

            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has settings you can access in setup
            Browser.AllowMetaRedirect = true;
            WebPage PageResult = Browser.NavigateToPage(new Uri("https://gse.com.gh/Market-Statistics/shares"));
            HtmlNode TitleNode = PageResult.Html.CssSelect(".navbar-brand").First();
            string PageTitle = TitleNode.InnerText;
            var Table = PageResult.Html.CssSelect("#tr-excel").First();

            List<Guid> _ticker = new List<Guid>();
            Guid MarketDayID = GetMarketDayID();
            DataTable  dt = new DataTable();
            dt.Columns.Add("Ticker_id", typeof(Guid));
            dt.Columns.Add("Ticker", typeof(string));
            dt.Columns.Add("PriceOpen", typeof(decimal));
            dt.Columns.Add("PriceOpenID", typeof(Guid));
            dt.Columns.Add("PriceClose", typeof(decimal));
            dt.Columns.Add("PriceCloseID", typeof(Guid));
            dt.Columns.Add("PriceChange", typeof(decimal));
            dt.Columns.Add("PriceChangeID", typeof(Guid));
            dt.Columns.Add("MarketDayID", typeof(Guid));

           
            foreach (var row in Table.SelectNodes("tbody/tr"))
            {
                DataRow dr = dt.NewRow();
                foreach (var cell in row.SelectNodes("td[2]"))
                {
                    var tickers = new Table<Ticker>(session);
                    IEnumerable<Ticker> TickerIDs = (from ticker in tickers where ticker.ticker == cell.InnerText select ticker).AllowFiltering().Execute();
                    foreach (var ids in TickerIDs)
                    {

                        dr["Ticker_id"] = ids.ticker_id;
                        dr["Ticker"] = cell.InnerText;

                    }

                }
                foreach (var cellopen in row.SelectNodes("td[6]"))
                {
                    dr["PriceOpen"] = cellopen.InnerText;
                    dr["PriceOpenID"] = Guid.NewGuid();
                }
                foreach (var cellclose in row.SelectNodes("td[7]"))
                {
                    dr["PriceClose"] = cellclose.InnerText;
                    dr["PriceCloseID"] = Guid.NewGuid();
                   
                }
                foreach (var cellchange in row.SelectNodes("td[8]"))
                {
                    dr["PriceChange"] = cellchange.InnerText;
                    dr["PriceChangeID"] = Guid.NewGuid();

                }

               
                dr["MarketDayID"] = MarketDayID;
                dt.Rows.Add(dr);

            }


            foreach (DataRow row in dt.Rows)
            {
                // Insert afristox.price_close
                session.Execute("insert into afristox.price_close (priceclose_id, marketday_id, price_close, ticker_id) " +
                                "values ("+ row["PriceCloseID"] +","+ row["MarketDayID"]+ "," + row["PriceClose"]+ "," + row["Ticker_id"]+ ")");

                // Insert afristox.price_open
                session.Execute("insert into afristox.price_open (priceopen_id, marketday_id, price_open, ticker_id) " +
                                "values (" + row["PriceOpenID"] + "," + row["MarketDayID"] + "," + row["PriceOpen"] + "," + row["Ticker_id"] + ")");


                // Insert afristox.price_change
                session.Execute("insert into afristox.price_change (pricechange_id, marketday_id, price_change, ticker_id) " +
                                "values (" + row["PriceChangeID"] + "," + row["MarketDayID"] + "," + row["PriceChange"] + "," + row["Ticker_id"] + ")");


            }
        }

      //Get Todays ID
       public static Guid GetMarketDayID()
        {
            // Connect to the demo keyspace on our cluster running at 127.0.0.1
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            Console.WriteLine("Connecting to Afristox Keyspace!");
            ISession session = cluster.Connect("afristox");
            Console.WriteLine("Connected to Afristox Keyspace!");
            Guid _returnvalue;
            // Get Today's Date ID
            var _marketDayID = session.Execute("select marketday_id from afristox.market_day where market_day = toDate(now()) ALLOW FILTERING;");
            foreach (var row in _marketDayID)
            {
                _returnvalue = row.GetValue<Guid>("marketday_id");

            }


            return _returnvalue;

        }


    }
}
