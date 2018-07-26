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
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using EyeSpy.Service.Imaging.Services;

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
            var appInsightsInstrumentationKey = Configuration.GetValue<string>(ServiceConstants.AppInsightsKeySettings);
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
            services.AddSingleton<ITrustedPersonsImageService>(new ImageSharpTrustedPersonsImageService());

            services.AddSwaggerGen(i =>
            {
                i.SwaggerDoc("v1.0", new Info
                {
                    Title = "Eye Spy API",
                    Version = "v1.0"
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"apikey", new string[] { }},
                };

                i.AddSecurityDefinition("apikey", new ApiKeyScheme
                {
                    Description = "Api key header using the apikey scheme. Example: \"apikey: {key}\"",
                    Name = "apikey",
                    In = "header",
                    Type = "apiKey"
                });

                i.AddSecurityRequirement(security);

                i.MapType<Stream>(() => new Schema { Type = "file" });
            });

            services.AddMvc();
            services.AddApplicationInsightsTelemetry(appInsightsInstrumentationKey);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("default");
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(i =>
            {
                i.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Eye Spy Api v1.0");
                i.DocumentTitle = "Eye Spy Api Documentation";
                i.DocExpansion(DocExpansion.None);
            });

            app.UseMvc();
        }
    }
}