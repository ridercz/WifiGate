using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace WifiGate {
    public class Startup {

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            // Enable console logginh
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();

            if (env.IsDevelopment()) {
                // Development environment
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            else {
                // Production environment
                app.Use(async (context, next) => {
                    var currentHost = context.Request.Host;
                    if (currentHost.HasValue && currentHost.Value.StartsWith("www.wifigate-login.local", StringComparison.OrdinalIgnoreCase)) {
                        await next.Invoke();
                    }
                    else {
                        context.Response.Redirect("http://www.wifigate-login.local/");
                    }
                });
            }

            // Enable static files
            app.UseStaticFiles();

            // Enable MVC
            app.UseMvc();
        }

    }
}
