using System;
using System.Collections.Generic;
using System.Data;
using afristoxAPI.Helpers;
using afristoxAPI.Models;
using Cassandra;
using Cassandra.Data.Linq;

namespace afristoxAPI.DAO
{
    public static class DataAccess
    {
       

        public static List<PriceByTicker> GetAllStocks()
        {
           
            //Connect to Cassandra
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            ISession session = cluster.Connect("afristox");
           
            //Lets create a flat table that will be easily accessible
            DataTable dt = new DataTable();
            dt.Columns.Add("Ticker", typeof(string));
            dt.Columns.Add("CompanyName", typeof(string));
            dt.Columns.Add("PriceOpen", typeof(decimal));
            dt.Columns.Add("PriceClose", typeof(decimal));
            dt.Columns.Add("PriceChange", typeof(decimal));
      
            var tickers = new Table<Ticker>(session);
            var company = new Table<Company>(session);
            var co_exchange = new Table<Co_Exchange>(session);  
            var closingprice = new Table<Price_Close>(session);
            var openingprice = new Table<Price_Open>(session);
            var changeprice = new Table<Price_Change>(session);


                /* Collect The ticker ID and Co_exchange ID
                 * TickerID will help us get the Ticker Symbol
                 * coexchangeID will help get the companyID
                 * companyID will give us the Company Name 
                 * Twende Sasa */
                IEnumerable<Ticker> tickerids = (from ticker in tickers select ticker).Execute();

                //Loop through the ticker and coexchangeID
                foreach (var ids in tickerids)
                {
                    //Initialize the Datatable row
                    DataRow dr = dt.NewRow();
                    //Insert the ticker into a data row
                    dr["Ticker"] = ids.ticker;

                    //Now lets get the company Name
                    //Lets begin by fetching the company ID
                    IEnumerable<Co_Exchange> coexchange = (from exchange in co_exchange
                                                           where exchange.coexchange_id == ids.coexchange_id
                                                           select exchange).AllowFiltering().Execute();

                    //Finally lets head over to the company table to get the company name by company ID
                    foreach (var _company in coexchange)
                    {
                        IEnumerable<Company> companyname = (from co in company
                                                            where co.company_id == _company.company_id
                                                            select co).AllowFiltering().Execute();
                        //Kuja hapa! Now we shall grab the company name and put it in the Datatable
                        foreach (var coname in companyname)
                        {
                            dr["CompanyName"] = coname.company_name;
                        }

                    }

                    //Get and pupulate the closing price
                    IEnumerable<Price_Close> closing = (from cprice in closingprice
                                                        where
                            cprice.ticker_id == ids.ticker_id
                                                        select cprice).AllowFiltering().Execute();


                    foreach (var _closingprice in closing)
                    {

                        dr["PriceClose"] = _closingprice.price_close;
                    }

                    //Get and pupulate the opening price
                    IEnumerable<Price_Open> opening = (from oprice in openingprice
                                                       where
                           oprice.ticker_id == ids.ticker_id
                                                       select oprice).AllowFiltering().Execute();
                    foreach (var _openprice in opening)
                    {
                        dr["PriceOpen"] = _openprice.price_open;
                    }
                    //Get and pupulate the price change 
                    IEnumerable<Price_Change> change = (from cprice in changeprice
                                                        where
                             cprice.ticker_id == ids.ticker_id
                                                        select cprice).AllowFiltering().Execute();
                    foreach (var _pricechange in change)
                    {
                        dr["PriceChange"] = _pricechange.price_change;
                    }
                    //Add data to the table row
                    dt.Rows.Add(dr);
                }

                //Moody why are you doing this you might ask. Well I need to present the Data in Json, without having to use other libraries, the easist way
                //Move the Data from the Datatable to list 

                List<PriceByTicker> priceList = new List<PriceByTicker>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PriceByTicker prices = new PriceByTicker();
                    prices.Ticker = dt.Rows[i]["Ticker"].ToString();
                    prices.CompanyName = dt.Rows[i]["CompanyName"].ToString();
                    prices.OpeningPrice = (dt.Rows[i]["PriceOpen"] != System.DBNull.Value) ? Convert.ToDecimal(dt.Rows[i]["PriceOpen"]) : 0.00m;
                    prices.ClosingPrice = (dt.Rows[i]["PriceClose"] != System.DBNull.Value) ? Convert.ToDecimal(dt.Rows[i]["PriceClose"]) : 0.00m;
                    prices.PriceChange = (dt.Rows[i]["PriceChange"] != System.DBNull.Value) ? Convert.ToDecimal(dt.Rows[i]["PriceChange"]) : 0.00m;


                    priceList.Add(prices);
                }

                //Now all we have to do is return a list and WebAPI will take care of the rest. You Like?
                return priceList;
           
        }
    }
}
