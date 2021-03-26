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
using SOMAPI.Services;
using SOMData;
using SOMData.Models;
using Nelibur.ObjectMapper;
using Newtonsoft.Json.Serialization;
using SOM.Data;

namespace SOM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly ISchemaProvider _InfoSchemaService; 
        public SchemaController(
            ISchemaProvider InfoSchemaService )
        { 
            _InfoSchemaService = InfoSchemaService; 
        }
        [HttpGet("Table/{Name}")]
        public IActionResult GetTable(string Name)
        { 
            return new JsonResult(_InfoSchemaService.GetModel(Name) );
        }
        [HttpGet("Tables/{Filter}")]
        public IActionResult GetTables(string Filter)
        {
            string _filter = "";
            if (Filter != "*")
                _filter = Filter;
            return new JsonResult(_InfoSchemaService.GetTables(_filter));
        }
        [HttpGet("Model/{ModelName}")]
        public IActionResult GetModel(string ModelName)
        {
            return new JsonResult(_InfoSchemaService.GetModel(ModelName));
        } 
    } 
}