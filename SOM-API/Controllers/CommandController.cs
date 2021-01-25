using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SOM.Parsers;
using SOM.Procedures;
using SOMData.Models;

namespace SOM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IConfiguration _config;
        public CommandController(IConfiguration config)
        {
            _config = config; 
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            ConsoleCommand model = new ConsoleCommand();
            model.ConsoleCommandID = 0;
            model.CommandLine = " -t 2";
            return new JsonResult(model);
        }
    } 
}