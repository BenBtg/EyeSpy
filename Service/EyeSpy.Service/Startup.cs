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
using EyeSpy.Service.NotificationHub.Services;
using EyeSpy.Service.NotificationHub.Models;

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
            ApiKey = Configuration.GetValue<string>(ServiceConstants.EyeSpyKeySetting);
            var faceApiEndpoint = Configuration.GetValue<string>(ServiceConstants.FaceApiEndpointSetting);
            var faceApiSubscriptionKey = Configuration.GetValue<string>(ServiceConstants.FaceApiSubscriptionKeySetting);
            var azureStorageAccountName = Configuration.GetValue<string>(ServiceConstants.AzureStorageAccountNameSetting);
            var azureStorageAccountKey = Configuration.GetValue<string>(ServiceConstants.AzureStorageAccountKeySetting);
            var notificationHubNamespace = Configuration.GetValue<string>(ServiceConstants.NotificationHubNamespaceSetting);
            var notificationHubName = Configuration.GetValue<string>(ServiceConstants.NotificationHubNameSetting);
            var notificationHubKeyName = Configuration.GetValue<string>(ServiceConstants.NotificationHubKeyNameSetting);
            var notificationHubKey = Configuration.GetValue<string>(ServiceConstants.NotificationHubKeySetting);
            var hubConfig = new NotificationHubConfiguration(notificationHubNamespace, notificationHubName, notificationHubKeyName, notificationHubKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthOptions.DefaultScheme;
            }).AddApiKeyAuth(options =>
            {
                options.ApiKey = ApiKey;
            });

            services.AddSingleton<ITrustedPersonsFaceRecognition>(new FaceApiTrustedPersonsFaceRecognition(faceApiEndpoint, faceApiSubscriptionKey));
            services.AddSingleton<ITrustedPersonsStorage>(new AzureTrustedPersonsStorage(azureStorageAccountName, azureStorageAccountKey));
            services.AddSingleton<ITrustedPersonsNotifications>(new AzureTrustedPersonsNotifications(hubConfig));

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