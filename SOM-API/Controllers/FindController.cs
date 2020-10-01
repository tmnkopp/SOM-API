using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SOM.Parsers;
using SOM.Procedures;

namespace SOM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FindController : ControllerBase
    {
        [HttpGet("{find}")]
        public string Find(string find, string context) {
            int lines = 3; 
            return  "";  
        }
    }

    public class RepoParser : BaseParser
    {
        public RepoParser()
        {
            Path = @"C:\Users\Tim\source\repos\";
            PathExclusions.AddRange(".spec,node_modules,.git,e2e,.vscode,dist".Split(new char[] { ',' })); 
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}