using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using AutoMapper;
using FixIt_Backend.MappingDefinition;
using FixIt_Interface;
using FixIt_Data.Context;
using FixIt_Model;
using FixIt_Service.CrudServices;
using FixIt_Model.Users;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using FixIt_Service;

namespace FixIt_Backend
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

           
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("FixIt-Data")));


            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                { Title = "Fix It",
                  Description = "Fix It API.",
                  Contact=new OpenApiContact
                  {
                      Name = "Prakash",
                      Email="prakash.timalsina@selu.edu"
                  },
                  Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                });

            });

            services.AddScoped<ICrudService<Category>, CategoryService>();
            services.AddScoped<ICrudService<Issue>, IssueService>();
            services.AddScoped<ICrudService<SubCategories>, SubCategoryService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<ICustomFilterService<Category>, CategoryService>();
            services.AddIdentity<User,Role>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var swaggerEndPoint = Configuration["SwaggerEndPoint"];
            var swaggerUiEndPoint = Configuration["SwaggerUIEndPoint"];
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerEndPoint, "Fix It V1.0");
                c.RoutePrefix = swaggerUiEndPoint;
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .WithExposedHeaders(new[] { "Content-Range" })
                       .AllowCredentials()
                       .AllowAnyHeader();
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
