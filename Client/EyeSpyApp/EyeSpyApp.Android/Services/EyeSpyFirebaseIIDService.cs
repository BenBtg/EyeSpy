using System;
using System.Collections.Generic;
using Android.App;
using Firebase.Iid;
using Android.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
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
            //var hub = new NotificationHub(
            //   "eyespynotificationhubhack2018",
            //   "Endpoint=sb://eyespynotificationnamespacehack2018.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=mRES0Z5YXlT4w2vPikBKyfnnM8Y9N3OGNp9TkPHOT6U=",
            //   this);

            //var tags = new List<string>() { };
            //var regId = hub.Register(token, tags.ToArray()).RegistrationId;

            //Log.Debug(TAG, $"Successful registration of ID {regId}");
        }
    }
}