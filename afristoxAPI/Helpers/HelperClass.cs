using System;
using System.Data;
using Newtonsoft.Json;
  
namespace afristoxAPI.Helpers
{
    public  class HelperClass
    {
        public string DataTableToJSONWithJSONNet(DataTable table) 
        {  
           string JSONString=string.Empty;  
           JSONString = JsonConvert.SerializeObject(table);  
           return JSONString;  
        } 
    }
}
