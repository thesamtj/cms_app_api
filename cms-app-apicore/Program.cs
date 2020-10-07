using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace cms_app_apicore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    int zero = 0;
                    int result = 100 / zero;
                }
                catch (DivideByZeroException ex)
                {
                    Log.Error("An error occured while sending the database {Error} {StackTrace} {InnerException} {Source}", ex.Message, ex.StackTrace, ex.InnerException, ex.Source);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                     .Enrich.FromLogContext()
                     .Enrich.WithProperty("Application", "CMS_APP_API")
                     .Enrich.WithProperty("MachineName", Environment.MachineName)
                     .Enrich.WithProperty("CurrentManagedThreadId", Environment.CurrentManagedThreadId)
                     .Enrich.WithProperty("OSVersion", Environment.OSVersion)
                     .Enrich.WithProperty("Version", Environment.Version)
                     .Enrich.WithProperty("UserName", Environment.UserName)
                     .Enrich.WithProperty("ProcessId", Process.GetCurrentProcess().Id)
                     .Enrich.WithProperty("ProcessName", Process.GetCurrentProcess().ProcessName)
                     .WriteTo.Console(theme: CustomConsoleTheme.VisualStudioMacLight)
                     .WriteTo.File(formatter: new CustomTextFormatter(), path: Path.Combine(hostingContext.HostingEnvironment.ContentRootPath + $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}", $"cms_app_api_{DateTime.Now:yyyyMMdd}.txt"))
                    .ReadFrom.Configuration(hostingContext.Configuration));
                    webBuilder.UseStartup<Startup>();
                });
    }
}
