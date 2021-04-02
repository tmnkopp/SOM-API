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
            var appSettings = _config.GetSection(nameof(AppSettings)).Get<AppSettings>();
            path = appSettings.SourceDir;
            dest = appSettings.DestDir;
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
             
            List<ICompilable> compilers = new List<ICompilable>() {
                new ModelTemplateInterpreter(_config),
                new ModuloInterpreter(),
                new KeyValReplacer(json)
            };

            List<CodeTemplate> templates = new List<CodeTemplate>(); 
            foreach (CodeTemplate template in DocProvider.GetTemplates(path))
            {
                foreach (ICompilable compiler in compilers)
                    template.Content = compiler.Compile(template.Content); 
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
