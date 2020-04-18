using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SOM.Procedures;
using SOM.Models;
using SOM.Procedures;
using SOMAPI.Models;
using SOM.Extentions;
namespace SOM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class AppModelController : ControllerBase
    { 
        [HttpGet("{Model}")]
        public IActionResult GetTable(string model)
        { 
            return new JsonResult(GetAppModel(model));
        } 

        [HttpGet("Inject/{content}")]
        public IActionResult Inject( string content )
        {
            Match match = Regex.Match(content, "\\[ModelInject .*\\]");
            string result = "";
            if (match.Success)
            {
                ICompiler comp = (ICompiler)Invoker.InvokeProcedure(match.Value.RemoveAsChars("[]"));
                result = comp.Compile(content); 
            } 
            return new JsonResult(result);
        }

        private List<string> GetInjectList() {
            List<string> Injectables = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(Injectable).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();

            return Injectables;
        }

        private AppModel GetAppModel(string name) { 
            return new AppModel()
            {
                Name = name,
                AppModelItems = new TableEnumerator(name).Items 
            }; 
        }
 
        [HttpPost]
        public IActionResult InjectModel([FromBody]CompilerViewModel model)
        {
            string compileFrom = model.CompileFrom;
            StringBuilder sb = new StringBuilder();
            foreach (var item in GetAppModel(compileFrom).AppModelItems)
            {
                sb.Append($"{item.Name}\n");
            }
            AppModel appModel = GetAppModel(compileFrom);
            model.CompileTo = JsonConvert.SerializeObject(appModel, Formatting.Indented) ;
            return new JsonResult(appModel);
        }
       
    }
}