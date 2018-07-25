using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Iid;
using Microsoft.Azure.NotificationHubs;

namespace EyeSpyApp.Android.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class EyeSpyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "EyeSpyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }

        async Task SendRegistrationToServer(string token)
        {
            try
            {
                var connection = "Endpoint=sb://eyespynotificationnamespacehack2018.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=6hUFdXZvxHmExKOi7iht3M8ZcAHxbejg0L/LzgezsXQ=";
                var client = NotificationHubClient.CreateClientFromConnectionString(connection, "eyespynotificationhubhack2018");
                var installation = new Installation
                {
                    InstallationId = token,
                    Platform = NotificationPlatform.Gcm,
                    PushChannel = token,
                    Tags = new List<string>() { "eyespy" },
                };
                await client.CreateOrUpdateInstallationAsync(installation);

                var registeredInstallation = await client.GetInstallationAsync(token);

                Log.Debug(TAG, $"Successful created/updated installation with ID {registeredInstallation?.InstallationId}");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to register installation: {ex}");
            }
        }
    }
}