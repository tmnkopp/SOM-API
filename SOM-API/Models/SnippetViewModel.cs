using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SOM.Models;

namespace SOMAPI.Models
{
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class SnippetViewModel
    { 
        public string Source { get; set; }
        public string Header { get; set; }
        public string Body { get; set; } 
    }
}
