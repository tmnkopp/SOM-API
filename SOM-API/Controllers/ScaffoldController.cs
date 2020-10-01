using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SOMAPI.Models;
using SOMData.Models;
using SOMData.Providers;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using SOM.Procedures;
using Newtonsoft.Json;

namespace SOMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScaffoldController : ControllerBase
    {
        private readonly IConfiguration _config;
        private string path;
        private string dest;
        public ScaffoldController(IConfiguration config)
        {
            _config = config;
            path = _config.GetValue<string>("AppSettings:SourceDir");
            dest = _config.GetValue<string>("AppSettings:DestDir");
        }
     
        [HttpGet("GetScaffold/{ID}")]
        public IActionResult GetScaffold(int ID)
        {
            ScaffoldViewModel model = new ScaffoldViewModel();
            model.SaveDestination = dest;
            model.ModelName = "{0}";
            model.Namespace = "{1}";
            model.CodeTemplates = CompileTemplates(model);
            return new JsonResult(model);
        }
        [HttpPost]
        [Route("Load")]
        public IActionResult Load([FromBody] ScaffoldViewModel model)
        {
            model.CodeTemplates.Clear();
            model.CodeTemplates = CompileTemplates(model); 
            return new JsonResult(model);
        }
        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody] ScaffoldViewModel model)
        {
            model.CodeTemplates.Clear();
            model.CodeTemplates = CompileTemplates(model);
            WriteFiles(model.ModelName, model.SaveDestination, model.CodeTemplates);
            return new JsonResult(model);
        }
        public List<CodeTemplate> CompileTemplates(ScaffoldViewModel model) {

            Dictionary<string, string> KeyVals = new Dictionary<string, string>(); 
            KeyVals.Add("<#= ModelTypeName #>", model.ModelName);
            KeyVals.Add("<#= ModelVariable #>", model.ModelName.ToLower());
            KeyVals.Add("<#= Namespace #>", model.Namespace);
            string json = JsonConvert.SerializeObject(KeyVals);
             
            List<IInterpreter> compilers = new List<IInterpreter>() {
                new ModelTemplateInterpreter(),
                new ModuloInterpreter(),
                new KeyValInterpreter(json)
            };

            List<CodeTemplate> templates = new List<CodeTemplate>(); 
            foreach (CodeTemplate template in DocProvider.GetTemplates(path))
            {
                foreach (IInterpreter compiler in compilers)
                    template.Content = compiler.Interpret(template.Content); 
                templates.Add(template);
            }
            return templates;
        } 
        [HttpGet("GetCodeTemplates/{ModelName}/{Namespace}")]
        public IActionResult GetCodeTemplates(string ModelName, string Namespace)
        {
            ScaffoldViewModel model = new ScaffoldViewModel();
            model.SaveDestination = dest;
            model.ModelName = ModelName;
            model.Namespace = Namespace; 
            model.CodeTemplates=CompileTemplates(model);
            return new JsonResult(model); 
        }

        private void WriteFiles(string ModelName, string Destination, List<CodeTemplate> templates)
        {
            Destination = string.IsNullOrWhiteSpace(Destination) ? dest : Destination ;
            foreach (CodeTemplate template in templates)
            {
                string name = template.Name;
                Match match = Regex.Match(name, @".*_(.*)");
                if (match.Success) 
                    name = match.Groups[1].Value;
 
                System.IO.File.WriteAllText($"{Destination}\\{ModelName}{name}", template.Content);
            }
        }
    }
}
