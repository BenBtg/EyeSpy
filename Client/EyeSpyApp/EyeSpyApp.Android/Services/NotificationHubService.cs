using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Iid;
using Microsoft.Azure.NotificationHubs;

namespace EyeSpyApp.Android.Services
{
    public class NotificationHubService
    {
        public NotificationHubService()
        {
        }

        public async Task SendRegistrationToServer(Context context, string token)
        {
            if (context == null || string.IsNullOrWhiteSpace(token))
                return;

            try
            {
                var deviceId = global::Android.Provider.Settings.Secure.GetString(context.ContentResolver, global::Android.Provider.Settings.Secure.AndroidId);
                if (string.IsNullOrWhiteSpace(deviceId))
                    return;

                deviceId = Regex.Replace(deviceId.ToLower(), @"[^\w]+", string.Empty, RegexOptions.Singleline);
                var connection = "Endpoint=sb://eyespynotificationnamespacehack2018.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=6hUFdXZvxHmExKOi7iht3M8ZcAHxbejg0L/LzgezsXQ=";
                var client = NotificationHubClient.CreateClientFromConnectionString(connection, "eyespynotificationhubhack2018");
                var templates = new Dictionary<string, InstallationTemplate>
                {
                    { "EyeSpyTemplate", new InstallationTemplate() { Body = @"{""data"":{""title"":""$(title)"", ""message"":""$(message)"", ""detectionId"":""$(detectionId)"", ""imageReference"":""$(imageReference)""}}" } }
                };

                var installation = new Installation
                {
                    InstallationId = deviceId,
                    Platform = NotificationPlatform.Gcm,
                    PushChannel = token,
                    Tags = new List<string>() { "eyespy" },
                    Templates = templates,
                };

                await client.CreateOrUpdateInstallationAsync(installation);
                System.Diagnostics.Debug.WriteLine($"Successful created/updated installation with ID {installation?.InstallationId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to register installation: {ex}");
            }
        }
    }
}
