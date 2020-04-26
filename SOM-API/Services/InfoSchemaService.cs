using SOM.Data;
using SOM.Models;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOMAPI.Services
{
    public class InfoSchemaService : IInfoSchemaService
    {
        public IList<string> GetTables(string Filter)
        {
            string sql = $" SELECT CONVERT(NVARCHAR(5), ROW_NUMBER() OVER (ORDER BY TABLE_NAME))  K, TABLE_NAME V FROM INFORMATION_SCHEMA.TABLES ";
            if (Filter != "") 
                sql += $" WHERE TABLE_NAME LIKE '%{Filter}%' "; 
            KeyValDBReader reader = new KeyValDBReader(sql);
            reader.ExecuteRead();
            return reader.Data.Values.ToList();   
        }
        public List<AppModelItem> GetAppModelItems(string ModelName)
        {
            return GetAppModel(ModelName).AppModelItems.ToList(); 
        }
        public AppModel GetAppModel(string ModelName)
        {
            List<AppModelItem> _AppModelItems = new List<AppModelItem>();
            if (ModelName.Contains(".")) {  
                _AppModelItems = new TypeEnumerator(Type.GetType(ModelName)).Items;
                ModelName = ModelName.Split(".")[ModelName.Split(".").Length - 1]; 
            }
            else
                _AppModelItems = new TableEnumerator(ModelName).Items;
            return new AppModel()
            {
                ModelName = ModelName,   AppModelItems = _AppModelItems
            };
        }
    }
}
