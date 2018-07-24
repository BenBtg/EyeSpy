using System;
using System.Collections.Generic;
using EyeSpy.Service.Common;
using EyeSpy.Service.Common.Abstractions;
using EyeSpy.Service.Authentication;
using EyeSpy.Service.Extensions;
using EyeSpy.Service.FaceApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EyeSpy.Service.AzureStorage.Services;

namespace EyeSpy.Service
{
    public class Startup
    {
        private static string ApiKey { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ApiKey = Configuration.GetValue<string>(ServiceConstants.EyeSpyKey);
            var faceApiEndpoint = Configuration.GetValue<string>(ServiceConstants.FaceApiEndpoint);
            var faceApiSubscriptionKey = Configuration.GetValue<string>(ServiceConstants.FaceApiSubscriptionKeySettingName);
            var azureStorageAccountName = Configuration.GetValue<string>(ServiceConstants.AzureStorageAccountName);
            var azureStorageAccountKey = Configuration.GetValue<string>(ServiceConstants.AzureStorageAccountKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthOptions.DefaultScheme;
            }).AddApiKeyAuth(options =>
            {
                options.ApiKey = ApiKey;
            });

            services.AddSingleton<ITrustedPersonsService>(new FaceApiTrustedPersonsService(faceApiEndpoint, faceApiSubscriptionKey));
            services.AddSingleton<ITrustedPersonsStorage>(new AzureTrustedPersonsStorage(azureStorageAccountName, azureStorageAccountKey));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}