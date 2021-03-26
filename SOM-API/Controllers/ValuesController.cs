using System;   
using System.Collections.Generic;
using System.Linq;  
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; 
using SOMAPI.Models; 
using SOMAPI.Services;
using SOMData;
using SOMData.Models;
using Nelibur.ObjectMapper;
using Microsoft.Extensions.Configuration;
using SOMData.Providers;
using SOM.Data;

namespace SOM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ISchemaProvider _InfoSchemaService;
        private string _conn = "";
        public ValuesController(
            ISchemaProvider InfoSchemaService,
            IConfiguration _configuration)
        {
            _InfoSchemaService = InfoSchemaService;
            _conn = _configuration.GetConnectionString("default"); 
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var schema = _InfoSchemaService.GetTables("");
            var json = new Dictionary<string, string>();
            foreach (var item in schema)
                json.Add(item, item);
            
            return new JsonResult(json);
        }  
    }
}
