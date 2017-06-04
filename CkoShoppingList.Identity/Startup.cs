using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CkoShoppingList.Identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CkoShoppingList.Identity
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Options
            services.AddOptions();
            services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

            // Get app settings
            var sp = services.BuildServiceProvider();
            var appSettingsOptions = sp.GetService<IOptions<AppSettings>>();

            // Configure identity server
            services.AddIdentityServer()
                .AddInMemoryClients(IdentityConfig.GetClients(appSettingsOptions.Value))
                .AddInMemoryApiResources(IdentityConfig.GetApiResources(appSettingsOptions.Value))
                .AddTemporarySigningCredential();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }
    }
}