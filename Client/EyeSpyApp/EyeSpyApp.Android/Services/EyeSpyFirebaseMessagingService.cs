using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Util;
using Firebase.Messaging;
using System.Collections.Generic;
using EyeSpyApp.Helpers;

namespace EyeSpyApp.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class EyeSpyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "EyeSpyFirebaseMessagingService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification()?.Body);
            SendNotification(message.GetNotification()?.Body, message.Data);
        }

        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (string key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var title = "Alarmo! Alarmo!";
            if (data.ContainsKey("title"))
            {
                var titleOverride = data["title"];
                if (!string.IsNullOrWhiteSpace(titleOverride))
                    title = titleOverride;
            }

            var message = "Unrecognized Person Detected!";
            if (data.ContainsKey("message"))
            {
                var messageOverride = data["message"];
                if (!string.IsNullOrWhiteSpace(messageOverride))
                    message = messageOverride;
            }

            var channelId = "DetectionsChannel_01";
            var uri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.ic_logo)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetAutoCancel(true)
                .SetChannelId(channelId)
                .SetContentIntent(pendingIntent);

            var imageReference = data.ContainsKey("imageReference") ? data["imageReference"] : null;
            if (!string.IsNullOrWhiteSpace(imageReference))
            {
                imageReference = imageReference.WithToken();
                //todo: set image notification style
            }

            var notification = notificationBuilder.Build();
            var notificationManager = NotificationManager.FromContext(this);

            var channel = new NotificationChannel(channelId, "Detections Channel", NotificationImportance.High);
            notificationManager.CreateNotificationChannel(channel);
            notificationManager.Notify(0, notification);
        }
    }
}