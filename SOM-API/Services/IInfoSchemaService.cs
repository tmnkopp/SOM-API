using System.Collections.Generic;
using SOM.Models;

namespace SOMAPI.Services
{
    public interface IInfoSchemaService
    {
        AppModel GetAppModel(string ModelName);
        List<AppModelItem> GetAppModelItems(string ModelName);
        IList<string> GetTables(string Filter);
    }
}