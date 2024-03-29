﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Facebook.API.Helpers;

namespace Facebook.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) //tu kolwjność serwisów jest bez znazcenia
        {
            services.AddDbContext<DataContext>(x=>x.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddJsonOptions(opt=>{
                        opt.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
            services.AddCors();  //bład dostepu do api
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySetting"));
            services.AddAutoMapper();
            services.AddTransient<Seed>(); //klasa seed odczytuje i dodaje dane do bazy gdy jest pusta
            services.AddScoped<IAuthRepository,AuthRepository>();  //rejestraacja repozytorium auth scoped coś dla srwisów pomiedzy lekkimi a cieższymi
            //dodajemy po czym ma przeprowadzić autntyfikacje i jej opcje
            //tych opcji do konca nie ogarniam 
            services.AddScoped<IGenericRepository,GenericRepository>();
            services.AddScoped<IUserRepository,UserRepository>();
               
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>{
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                
                            };
                        IdentityModelEventSource.ShowPII = true;
                        //showPII pokazuje błąd tokena w konsoli

                        });
        }

         public void ConfigureDevelopmentServices(IServiceCollection services) //tu kolwjność serwisów jest bez znazcenia
        {
            services.AddDbContext<DataContext>(x=>x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddJsonOptions(opt=>{
                        opt.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
            services.AddCors();  //bład dostepu do api
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySetting"));
            services.AddAutoMapper();
            services.AddTransient<Seed>(); //klasa seed odczytuje i dodaje dane do bazy gdy jest pusta
            services.AddScoped<IAuthRepository,AuthRepository>();  //rejestraacja repozytorium auth scoped coś dla srwisów pomiedzy lekkimi a cieższymi
            //dodajemy po czym ma przeprowadzić autntyfikacje i jej opcje
            //tych opcji do konca nie ogarniam 
            services.AddScoped<IGenericRepository,GenericRepository>();
            services.AddScoped<IUserRepository,UserRepository>();
               
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>{
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                
                            };
                        IdentityModelEventSource.ShowPII = true;
                        //showPII pokazuje błąd tokena w konsoli

                        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,Seed seeder) //tu ważna jest klejność
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => 
                {
                    builder.Run(async context => 
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
           
            seeder.SeedUsers();
            app.UseCors(x=>x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); //bład dostepu do api
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(routes=>{
                routes.MapSpaFallbackRoute(
                    name: "spa",
                     defaults: new {controller="Fallback", action="Index"});
            });
        }
    }
}




