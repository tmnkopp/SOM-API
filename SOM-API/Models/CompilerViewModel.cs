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
        public int CompilationWorkspaceId { get; set; }
        public string CompilationTitle { get; set; }
        public string ModelName { get; set; }
        public AppModel AppModel { get; set; }
        public string Command { get; set; }
        public string CommandParams { get; set; } 
        public string CompileFrom { get; set; } 
        public string CompileTo { get; set; }
        public string WrapExpression { get; set; }
        public string ReplaceTerms { get; set; }
        public string ParseLines { get; set; }
    }
}
