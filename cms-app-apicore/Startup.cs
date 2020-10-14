using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataService;
using FunctionalService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelService;

namespace cms_app_apicore
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
            /*---------------------------------------------------------------------------------------------------*/
            /*                              DB CONNECTION OPTIONS                                                */
            /*---------------------------------------------------------------------------------------------------*/
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("CmsCoreNg_DEV"), x => x.MigrationsAssembly("cms-app-apicore")));

            services.AddDbContext<DataProtectionKeysContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DataProtectionKeysContext"), x => x.MigrationsAssembly("cms-app-apicore")));
            /*---------------------------------------------------------------------------------------------------*/
            /*                             Functional SERVICE                                                    */
            /*---------------------------------------------------------------------------------------------------*/
            services.AddTransient<IFunctionalSvc, FunctionalSvc>();
            services.Configure<AdminUserOptions>(Configuration.GetSection("AdminUserOptions"));
            services.Configure<AppUserOptions>(Configuration.GetSection("AppUserOptions"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
