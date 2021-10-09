using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using Nielsen.TechTest.Q1.Common;
using NLog.Extensions.Logging;
using NLog;

namespace Nielsen.TechTest.Q1.RobotHosting
{
    public class Startup
    {
        private const string Config_RobotBindingArea = "RobotBindingArea";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LogManager.Configuration = new NLogLoggingConfiguration(Configuration.GetSection("NLog"));
            services.AddLogging(logBuilder =>
            {
                logBuilder.AddNLog();
            });

            services.AddOptions<BoundingArea>().Bind(Configuration.GetSection(Startup.Config_RobotBindingArea));
            services.AddSingleton<ISimpleAsyncRobot<Location, MoveInstruction>, AreaBoundedRobotByConfig>();
            services.AddCors();
            services.AddControllers().AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            */

            app.UseExceptionHandler(thisApp =>
            {
                thisApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature.Error is CannotMoveException)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "plain/text";
                        await context.Response.WriteAsync(errorFeature.Error.Message);
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "plain/text";
                        await context.Response.WriteAsync(errorFeature.Error.ToString());
                    }
                });
            });

            app.UseCors();
            app.UseRouting();
            // actually does not need
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
