﻿using System;
using System.Reflection.Metadata;
using AbstractionProvider;
using AbstractionProvider.Configurations;
using AbstractionProvider.Interfaces;
using AbstractionProvider.Interfaces.Repositories;
using AbstractionProvider.Interfaces.Services;
using BusinessLayer;
using DataAccessLayer;
using ExternalDataService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using PresentationLayer;
using PresentationLayer.Controllers;
using PeriodicSportService = ExternalDataService.PeriodicSportService;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebApp
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
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddControllersAsServices();
            
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                                                                      builder =>
                                                                      {
                                                                          builder.AllowAnyMethod().AllowAnyHeader()
                                                                                 .WithOrigins(Configuration.GetSection("AppURL").Value)
                                                                                 .AllowCredentials();
                                                                      }));
            services.AddSignalR().AddJsonProtocol(options => {
                                                      options.PayloadSerializerSettings.ContractResolver =
                                                                          new DefaultContractResolver();
                                                  });

            //Can be hide with abstraction implemented in separated Class Library with different methods of getting config settings
            //For the purpose of the task I don't think that's necessary
            services.AddOptions();
            services.Configure<BusinessConfig>(Configuration.GetSection("Constants"));
            
            //Can be hide with abstraction implemented in separated Class Library with different methods of caching
            //For the purpose of the task I don't think that's necessary
            services.AddMemoryCache();
            services.AddTransient<CacheProvider>();

            services.AddDbContext<UltraplayTaskDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IRepository, Repository>();

            services.AddTransient<IUpdateSportDataService, UpdateSportDataHub>();

            services.AddSingleton<IHostedService, PeriodicSportService>();

            services.AddTransient<IExternalSportService, HttpSportService>();
            
            services.AddTransient<ISportService, SportService>();

            services.AddTransient<ViewRenderer>();
            services.AddTransient<HomeController>();
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
                           {
                               routes.MapHub<UpdateSportDataHub>(Configuration.GetSection("HubUrl").Value);
                           });
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
