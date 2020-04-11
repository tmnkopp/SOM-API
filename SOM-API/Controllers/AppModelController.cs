using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SOM.Compilers;
using SOM.Models;

namespace SOM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AppModelController : ControllerBase
    { 
        [HttpGet("{Model}")]
        public IActionResult GetTable(string model)
        { 
            return new JsonResult(GetAppModel(model));
        }
        private AppModel GetAppModel(string name) { 
            return new AppModel()
            {
                Name = name,
                AppModelItems = new TableEnumerator(name).Items().ToList()
            }; 
        }
    }
}