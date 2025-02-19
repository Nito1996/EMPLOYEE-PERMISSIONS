using System;
using EmployeePermissions.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EmployeePermissions
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
            services.AddControllers();

            var connectionString = Configuration.GetConnectionString("Connection");
            services.AddDbContext<EmployeePermissionsDbContext>(options =>
                options.UseSqlServer(connectionString));


            // Swagger Configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Permissions API", Version = "v1" });
                c.MapType<Guid>(() => new OpenApiSchema { Type = "string", Format = "uuid" });
                c.SchemaFilter<GuidSchemaFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Swagger Configuration
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployeePermissions API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
