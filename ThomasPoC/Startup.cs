using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using Swashbuckle.AspNetCore.Swagger;
using ThomasPoC.Data;
using ThomasPoC.Data.DAL;

namespace ThomasPoC
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddDbContext<CustomerDbContext>(opt => opt.UseInMemoryDatabase("ThomasCustomers"));

            services.AddOData();

            //Add support for XML output.
            services.AddMvc(
                options => options.OutputFormatters.Add(new XmlSerializerOutputFormatter())
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<ICustomerRepo, CustomerRepo>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Thomas PoC API",
                    Description = "A PoC of API in .Net Core 2.1",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Thomas Betong",
                        Email = string.Empty,
                        Url = "http://thomasbetong.se"
                    },
                    License = new License
                    {
                        Name = "Use under ...",
                        Url = "https://thomasconcrete.com/license"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thomas Customer API PoC v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc(b =>
            {
                b.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

        }


        //Odata test..
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customer");
            return builder.GetEdmModel();
        }


    }
}
