using System;
using System.Net.Http;
using System.Threading.Tasks;
using EyeSpy.Service.Common.Abstractions;
using EyeSpy.Service.Common.Models;
using EyeSpy.Service.Common.Services;
using EyeSpy.Service.NotificationHub.Models;

namespace EyeSpy.Service.NotificationHub.Services
{
    public class AzureTrustedPersonsNotifications : BaseHttpService, ITrustedPersonsNotifications
    {
        private const string MessagesEndpoint = "messages?api-version=2015-01";
        private const string NotificationFormatHeaderKey = "ServiceBusNotification-Format";
        private const string NotificationFormatHeaderValue = "template";
        private const string NotificationContentTypeHeaderValue = "application/json;charset=utf-8.";
        private readonly NotificationHubConfiguration hubConfiguration;

        public AzureTrustedPersonsNotifications(NotificationHubConfiguration hubConfiguration) : base(hubConfiguration.BaseAddress)
        {
            this.hubConfiguration = hubConfiguration ?? throw new ArgumentNullException($"{nameof(hubConfiguration)} cannot be null");
        }

        public async Task<bool> SendDetectionNotificationAsync<T>(T notification) where T : BaseNotification, new()
        {
            try
            {
                await PostAsync<T>(MessagesEndpoint, notification, (request) => this.ConfigureRequestWithHeaders(request));
            }
            catch
            {
                // TODO: Log exception
                return false;
            }                

            return true;
        }

        private void ConfigureRequestWithHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", this.hubConfiguration.GetToken());
            request.Headers.Add("ContentType", NotificationContentTypeHeaderValue);
            request.Headers.Add(NotificationFormatHeaderKey, NotificationFormatHeaderValue);
        }
    }
}