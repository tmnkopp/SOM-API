﻿using System;
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
using SOMAPI;

namespace SOM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompilationController : ControllerBase
    { 
        private string path;
        private string dest;
        private readonly IConfiguration _config;
        private readonly ISchemaProvider _InfoSchemaService;
        private readonly IRepository<CompilationWorkspace> _CompWorkRepo;
        public CompilationController(
              ISchemaProvider InfoSchemaService
            , IRepository<CompilationWorkspace> CompWorkRepo
            , IConfiguration config)
        {
            _config = config;
            _InfoSchemaService = InfoSchemaService;
            _CompWorkRepo = CompWorkRepo;
            var appSettings = config.GetSection(nameof(AppSettings)).Get<AppSettings>();
            path = appSettings.SourceDir;
            dest = appSettings.DestDir;
        }
        [HttpGet("Get/{ID}")]
        public IActionResult GetCompilation(int ID)
        {
            
            CompilationWorkspace entity = _CompWorkRepo.GetById(ID);
            TinyMapper.Bind<CompilationWorkspace, CompilerViewModel>();
            return new JsonResult(TinyMapper.Map<CompilerViewModel>(entity));
        }
        [HttpGet("GetAll/")]
        public IActionResult GetAll()
        {
            List<CompilationWorkspace> entities = _CompWorkRepo.Table.ToList();
            TinyMapper.Bind<List<CompilationWorkspace>, List<CompilerViewModel>>();
            return new JsonResult(TinyMapper.Map<List<CompilerViewModel>>(entities));
        }
        [HttpPost]
        public IActionResult InsertCompilation([FromBody]CompilerViewModel model)
        {
            TinyMapper.Bind<CompilerViewModel, CompilationWorkspace>(); 
            var cw = TinyMapper.Map<CompilationWorkspace>(model);
            _CompWorkRepo.Insert(cw); 
            TinyMapper.Bind<CompilationWorkspace, CompilerViewModel>(); 
            return new JsonResult(TinyMapper.Map<CompilerViewModel>(cw));
        }
        [HttpPut]
        public IActionResult UpdateCompilation([FromBody]CompilerViewModel model)
        {
            TinyMapper.Bind<CompilerViewModel, CompilationWorkspace>(); 
            var cw = TinyMapper.Map<CompilationWorkspace>(model);
            _CompWorkRepo.Update(cw);
            TinyMapper.Bind<CompilationWorkspace, CompilerViewModel>();
            return new JsonResult(TinyMapper.Map<CompilerViewModel>(cw));
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
        [HttpGet("GetSnippets/{Filename}")]
        public List<string> GetSnippets(string Filename)
        {
            List<string> snippets = new List<string>();
            foreach (CodeTemplate template in DocProvider.GetTemplates(path))
            {
                if (template.Name.Contains(Filename))
                {  
                    string[] lines = template.Content.Split('~');
                    snippets = new List<string>(lines);
                } 
            } 
            return snippets;
        } 
    }
}