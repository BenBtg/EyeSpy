﻿using System;
using Android.App;
using Firebase.Iid;
using Android.Util;

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
        void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
        }
    }
}