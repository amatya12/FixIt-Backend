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
                  Version = "v1" });
            });

            services.AddScoped<ICrudService<Category>, CategoryService>();

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
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
