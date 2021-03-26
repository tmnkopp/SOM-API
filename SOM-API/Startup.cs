using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SOMData;
using SOMAPI;
using SOMAPI.Services;
using SOM.Data;

namespace SOM_API
{
    public class Startup
    {
        public IHostingEnvironment HostingEnvironment { get; private set; }
        public IConfiguration Configuration { get; private set; } 
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.HostingEnvironment = env;
            this.Configuration = configuration;
        } 
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false)
            .AddJsonOptions(jsonOptions => {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
             
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            services.AddSingleton(Configuration);

            // ...
            var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
             
            services.AddDbContext<SOMDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("som"))
            );
            //SOM.Bootstrapper.Run();
            services.AddCors(o => o.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddScoped<ISchemaProvider, SchemaProvider>(); 
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
         
        } 
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("AllowAny");
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
