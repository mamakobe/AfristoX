using System;
using System.Collections.Generic;
using System.Data;
using afristoxAPI.Models;
using Cassandra;
using Cassandra.Data.Linq;
namespace afristoxAPI.DAO
{
    public class DataAccess
    {
        public DataAccess()
        {
        }

        public void getAllStocks()
        {
           
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            Console.WriteLine("Connecting to Afristox Keyspace!");
            ISession session = cluster.Connect("afristox");
            Console.WriteLine("Connected to Afristox Keyspace!");

            DataTable dt = new DataTable();
            var tickers = new Table<Ticker>(session);

            IEnumerable<Ticker> tickerids = (from ticker in tickers  select ticker).Execute();

            DataRow dr = dt.NewRow();
            foreach (var ids in tickerids)
            {

                dr["Ticker_id"] = ids.ticker_id;
                dr["Ticker"] = ids.ticker;

            }

        }
    }
}
