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
    public class CompilerViewModel
    { 
        public string ModelName { get; set; }
        public AppModel AppModel { get; set; }
        public string CompileFrom { get; set; } 
        public string CompileTo { get; set; }

    }
}
