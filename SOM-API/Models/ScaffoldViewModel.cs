using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SOMData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace SOMAPI.Models
{
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class ScaffoldViewModel
    {
        public ScaffoldViewModel()
        {
            CodeTemplates = new List<CodeTemplate>();
        }
        public string ModelName { get; set; }
        public string Namespace { get; set; }
        public string SaveDestination { get; set; } 
        public List<CodeTemplate> CodeTemplates{ get; set; }
    }
}

 