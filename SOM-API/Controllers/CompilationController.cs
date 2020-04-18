using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SOM.Models;
using SOM.Procedures;
using SOMAPI.Models;
using SOM.Extentions;
namespace SOM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompilationController : ControllerBase
    {
        [HttpPost]
        public IActionResult Compile([FromBody]CompilerViewModel model)
        {
            //AppModel appModel = GetAppModel(model.ModelName);
            //model.CompileTo = JsonConvert.SerializeObject(appModel, Formatting.Indented);
            model.CompileTo = CompileInjectables(model.CompileFrom);
     
            return new JsonResult(model);
        }
        [HttpGet("{model}")]
        public string GetModel(string ModelName)
        {
            AppModel appModel = GetAppModel(ModelName);
            return JsonConvert.SerializeObject(appModel, Formatting.Indented);
        }

        private AppModel GetAppModel(string name)
        {
            return new AppModel()
            {
                Name = name,
                AppModelItems = new TableEnumerator(name).Items
            };
        }

        private string CompileInjectables(string content)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = content.Split('\n');
            foreach (var line in lines)
            {
                string pattern = @"\[.*\]";
                Match match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    string procedureCall = match.Value.RemoveAsChars("[]"); 
                    ICompiler proc = (ICompiler)SOM.Procedures.Invoker.InvokeProcedure(procedureCall);
                    result.AppendFormat("{0}", line.Replace(match.Value, proc.Compile(content)));
                }
                else
                {
                    result.AppendFormat("{0}\n", line);
                }
            }
            return result.ToString();
        }
    }
}