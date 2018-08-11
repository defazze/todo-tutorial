using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TodoTutorial.Data;

namespace TodoTutorial
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "API для списка важных дел", Version = "v1" });
            });

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            ConfigureDatabaseService(services, connectionString);
        }

        private void ConfigureDatabaseService(IServiceCollection services, string connectionString)
        {
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<TodoDbContext>(options =>
                {
                    options.UseNpgsql(connectionString,
                        npgSqlOptions =>
                        {
                            npgSqlOptions.MigrationsAssembly(typeof(TodoDbContext).Assembly.GetName().Name);
                        });
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API для списка важных дел");
            });
        }
    }
}
