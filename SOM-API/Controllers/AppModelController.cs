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
using SOMAPI.Models;
using SOM.Extentions;
using SOMAPI.Services;

namespace SOM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class AppModelController : ControllerBase
    {
        private readonly IInfoSchemaService _InfoSchemaService;
        public AppModelController(IInfoSchemaService InfoSchemaService)
        {
            _InfoSchemaService = InfoSchemaService;
        }
        [HttpGet("{Model}")]
        public IActionResult GetModels(string model)
        { 
            return new JsonResult(_InfoSchemaService.GetAppModel(model));
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
         
 
       // [HttpPost]
       // public IActionResult InjectModel([FromBody]CompilerViewModel model)
       // {
       //     string compileFrom = model.CompileFrom;
       //     StringBuilder sb = new StringBuilder();
       //     foreach (var item in GetAppModel(compileFrom).AppModelItems)
       //     {
       //         sb.Append($"{item.Name}\n");
       //     }
       //     AppModel appModel = GetAppModel(compileFrom);
       //     model.CompileTo = JsonConvert.SerializeObject(appModel, Formatting.Indented) ;
       //     return new JsonResult(appModel);
       // }
       
    }
}