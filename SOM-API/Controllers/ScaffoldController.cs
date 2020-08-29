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
            WriteFiles(model.SaveDestination, model.CodeTemplates); 
            return new JsonResult(model);
        }
        public List<CodeTemplate> CompileTemplates(ScaffoldViewModel model) {
            List<CodeTemplate> templates = new List<CodeTemplate>();
            foreach (CodeTemplate template in DocProvider.GetTemplates(path))
            {
                template.Content = template.Content.Replace("<#= ModelTypeName #>", model.ModelName);
                template.Content = template.Content.Replace("<#= ModelVariable #>", model.ModelName.ToLower());
                template.Content = template.Content.Replace("<#= Namespace #>", model.Namespace);
                templates.Add(template);
            }
            return templates;
        }
        [HttpGet("GetCodeTemplates/{ModelName}/{Namespace}")]
        public IActionResult GetCodeTemplates(string ModelName, string Namespace)
        {
            ScaffoldViewModel model = new ScaffoldViewModel();
            model.ModelName = ModelName;
            model.Namespace = Namespace; 
            model.CodeTemplates=CompileTemplates(model);
            return new JsonResult(model); 
        }

        public void WriteFiles(string Destination, List<CodeTemplate> templates)
        {
            Destination = string.IsNullOrWhiteSpace(Destination) ? dest : Destination ;
            foreach (CodeTemplate template in templates)
            {
                System.IO.File.WriteAllText($"{Destination}\\{template.Name}", template.Content);
            }
        }
    }
}
