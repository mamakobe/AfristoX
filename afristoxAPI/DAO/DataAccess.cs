using System;
using System.Collections.Generic;
using System.Data;
using afristoxAPI.Helpers;
using afristoxAPI.Models;
using Cassandra;
using Cassandra.Data.Linq;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using System.Linq;
using System.Threading.Tasks;
using RethinkDb.Driver.Model;
using Newtonsoft.Json;

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

       
        public List<Stox_details> GetAllStocks()
        {
            
           var stocks = R.Db("afri_stoxDB").Table("stock_prices").EqJoin("company_id", 
                                                                                           R.Db("afri_stoxDB").Table("trading_company"))
                                           .Without("{ right: 'id'}").Zip().OrderBy("company_id")


                                           .Run(conn());
           stocks = stocks.ToObject<List<Stox_details>>();
          
           // List<Stox_details> _stocks = JsonConvert.DeserializeObject<List<Stox_details>>(stocks);

           // var results = stocks.ToList();

            return stocks;
        }


       
    }
}
