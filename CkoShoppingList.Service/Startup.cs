using System;
using System.Collections.Concurrent;
using CkoShoppingList.Service.Models;
using CkoShoppingList.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace CkoShoppingList.Service
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

            // DI
            services.AddSingleton(_configuration);
            services.AddSingleton<IStorageService, StorageService>();
            services.AddSingleton(new ConcurrentDictionary<string, int>(StringComparer.CurrentCultureIgnoreCase));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Shopping List API", Version = "v1" });
            });

            // Make sure all requests are authenticated
            services.AddMvc(setup =>
            {
                setup.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            var appSettings = serviceProvider.GetService<IOptions<AppSettings>>().Value;

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = appSettings.AuthorityUri.ToString(),
                AllowedScopes = appSettings.Scopes.Split(','),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog("nlog.config");

            app.UseExceptionHandler("/Error");

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger/ui";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.EnabledValidator(null);
            });
        }
    }
}