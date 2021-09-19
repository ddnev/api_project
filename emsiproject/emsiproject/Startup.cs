using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;
using Serilog.Sinks.SystemConsole.Themes;
// New Dependencies
using emsiproject.DataAccess;

namespace emsiproject
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            ConfigureLogging(env);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(t =>
            {
                t.ClearProviders();
                t.AddSerilog(dispose: true);
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "emsiproject", Version = "v1" });
            });

            // Register DbContext
            services.AddScoped<IDataHandler, DataHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "emsiproject v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureLogging(IWebHostEnvironment env)
        {
            var doconsolelogging = Convert.ToBoolean(Configuration["Logging:Serilog:ConsoleLoggingEnabled"]);
            var consolelogminlevel = Configuration["Logging:Serilog:ConsoleMinLevel"].ToLower();
            
            var cfg = new LoggerConfiguration().Enrich.WithExceptionDetails()
                .Enrich.FromLogContext().MinimumLevel.Verbose();
            
            // Set up console logging
            if (doconsolelogging)
            {
                LogEventLevel clevel = ParseMinLevel(consolelogminlevel);

                cfg.WriteTo.Logger(config => config.MinimumLevel.Verbose().WriteTo.Console(clevel,
                    "{Timestamp:HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}",
                    theme: SystemConsoleTheme.Colored));
            }

            Log.Logger = cfg.CreateLogger();
            Log.Information($"Emsi test project logging online with environment {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
        }



        private LogEventLevel ParseMinLevel(string minimum)
        {
            LogEventLevel level = LogEventLevel.Debug;
            switch (minimum)
            {
                case "debug":
                    level = LogEventLevel.Debug;
                    break;
                case "verbose":
                    level = LogEventLevel.Verbose;
                    break;
                case "information":
                    level = LogEventLevel.Information;
                    break;
                case "warning":
                    level = LogEventLevel.Warning;
                    break;
                case "error":
                    level = LogEventLevel.Error;
                    break;
                case "fatal":
                    level = LogEventLevel.Fatal;
                    break;
                default:
                    break;
            }

            return level;
        }
    }
}
