using System.Collections.Specialized;
using System.Configuration;
using DTO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ReportsServer.Repositories;

namespace ReportServer
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ReportsServer", Version = "v1"});
            });
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string employeePath = appSettings[0];
            string tasksPath = appSettings[1];
            string editsPath = appSettings[2];
            string reportsPath = appSettings[3];
            string weeklyReportsPath = appSettings[4];
            services.AddSingleton<EmployeeRepository>(_ => new EmployeeRepository(employeePath));
            services.AddScoped<TaskRepository>(_ => new TaskRepository(tasksPath, editsPath));
            services.AddScoped<ReportsRepository>(_ => new ReportsRepository(reportsPath, weeklyReportsPath));
            services.AddScoped<EmployeeService>();
            services.AddScoped<TaskService>();
            services.AddScoped<ReportService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReportsServer v1"));
            }

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