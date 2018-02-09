using System;
using System.Collections.Generic;
using System.Data;
using afristoxAPI.Models;
using Cassandra;
using Cassandra.Data.Linq;
namespace afristoxAPI.DAO
{
    public static class DataAccess
    {
       

        public static DataTable GetAllStocks()
        {
           
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            Console.WriteLine("Connecting to Afristox Keyspace!");
            ISession session = cluster.Connect("afristox");
            Console.WriteLine("Connected to Afristox Keyspace!");

            DataTable dt = new DataTable();
            dt.Columns.Add("Ticker", typeof(string));
            dt.Columns.Add("PriceOpen", typeof(decimal));
            dt.Columns.Add("PriceClose", typeof(decimal));
            dt.Columns.Add("PriceChange", typeof(decimal));
      
            var tickers = new Table<Ticker>(session);
            var closingprice = new Table<Price_Close>(session);
            var openingprice = new Table<Price_Open>(session);
            var changeprice = new Table<Price_Change>(session);

            IEnumerable<Ticker> tickerids = (from ticker in tickers  select ticker).Execute();



            foreach (var ids in tickerids)
            {
                DataRow dr = dt.NewRow();
                //dr["Ticker_id"] = ids.ticker_id;
                dr["Ticker"] = ids.ticker;

                IEnumerable<Price_Close> closing = (from price in closingprice where 
                                                    price.ticker_id == ids.ticker_id 
                                                    select price).AllowFiltering().Execute();
                foreach (var price in closing )
                {
                    dr["PriceClose"] = price.price_close;
                }

                IEnumerable<Price_Open> opening = (from price in openingprice where 
                                                   price.ticker_id == ids.ticker_id 
                                                   select price).AllowFiltering().Execute();
                foreach (var price in opening)
                {
                    dr["PriceOpen"] = price.price_open;
                }
                IEnumerable<Price_Change> change = (from price in changeprice where 
                                                    price.ticker_id == ids.ticker_id 
                                                    select price).AllowFiltering().Execute();
                foreach (var price in change)
                {
                    dr["PriceChange"] = price.price_change;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
