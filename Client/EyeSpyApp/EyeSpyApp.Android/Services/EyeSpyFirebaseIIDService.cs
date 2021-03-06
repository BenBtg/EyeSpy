﻿using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Iid;

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
            new NotificationHubService().SendRegistrationToServer(this, refreshedToken);
        }
    }
}