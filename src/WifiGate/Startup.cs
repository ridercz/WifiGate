using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;

namespace WifiGate {
    public class Startup {
        readonly IConfiguration Configuration;

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv) {
            // Setup configuration sources.

            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath);
            builder.AddJsonFile("config.json");
            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddOptions();
            services.Configure<WifiGateOptions>(this.Configuration);
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            if (env.IsDevelopment()) {
                // Development environment
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            else {
                // Production environment
                var forceHostName = this.Configuration["ForceHostName"];
                if (!string.IsNullOrWhiteSpace(forceHostName)) {
                    app.Use(async (context, next) => {
                        var currentHost = context.Request.Host;
                        if (currentHost.HasValue && currentHost.Value.StartsWith(forceHostName, StringComparison.OrdinalIgnoreCase)) {
                            await next.Invoke();
                        }
                        else {
                            context.Response.Redirect($"http://{forceHostName}/");
                        }
                    });
                }
            }

            // Enable static files
            app.UseStaticFiles();

            // Enable MVC
            app.UseMvc();
        }

    }
}
