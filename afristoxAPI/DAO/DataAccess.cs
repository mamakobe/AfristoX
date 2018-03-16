using System;
using System.Collections.Generic;
using System.Data;
using afristoxAPI.Helpers;
using afristoxAPI.Models;
using Cassandra;
using Cassandra.Data.Linq;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;

namespace afristoxAPI.DAO
{
   
        

    public  class DataAccess
    {
        public static RethinkDB R = RethinkDB.R;
            
        public IConnection conn()
            {

                var c = R.Connection()
                .Hostname("35.172.121.189")
                .Port(RethinkDBConstants.DefaultPort)
                .Timeout(60)
                .Connect();

            return c;

            }

       
        public string GetAllStocks()
        {
            
            var stocks = R.Db("afri_stoxDB").Table("stock_prices").EqJoin("company_id", 
                                                                          R.Db("afri_stoxDB").Table("trading_company")).Run(conn());



            return stocks.Dump();
        }


       
    }
}
