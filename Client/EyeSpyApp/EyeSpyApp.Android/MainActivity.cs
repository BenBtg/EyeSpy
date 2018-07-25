using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Graphics;
using Android.OS;
using Firebase.Iid;
using ImageCircle.Forms.Plugin.Droid;
using EyeSpyApp.Android.Services;

namespace EyeSpyApp.Droid
{
    [Activity(
        Label = "EyeSpy App", 
        Icon = "@mipmap/icon", 
        Theme = "@style/EyeSpy", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        // Field, property, and method for Picture Picker
        public static readonly int PickImageId = 1000;
        public static readonly int CameraImageId = 500;
        public static readonly string filePath = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Pictures/" + "image.jpg";
        public TaskCompletionSource<List<System.IO.Stream>> PickImageTaskCompletionSource { set; get; }
        public TaskCompletionSource<System.IO.Stream> CameraImageTaskCompletionSource { set; get; }

        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            CheckAppPermissions();
            UpdateNotificationHubInstallation();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();

            LoadApplication(new App());
        }

        private void CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.Camera, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage, Manifest.Permission.Camera };
                    RequestPermissions(permissions, requestCode: 1);
                }
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {

                    var inputUris = new List<global::Android.Net.Uri>();
                    if (intent.Data != null)
                        inputUris.Add(intent.Data);

                    if (intent.ClipData != null && intent.ClipData.ItemCount > 0)
                    {
                        for (int i = 0; i < intent.ClipData.ItemCount; i++)
                            inputUris.Add(intent.ClipData.GetItemAt(i).Uri);
                    }

                    var result = new List<System.IO.Stream>();
                    foreach (var inputUri in inputUris)
                    {
                        System.IO.Stream stream = ContentResolver.OpenInputStream(inputUri);
                        var bmp = BitmapFactory.DecodeStream(stream);
                        MemoryStream ms = new MemoryStream();
                        bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                        ms.Seek(0L, SeekOrigin.Begin);
                        result.Add(ms);
                    }

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(result);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
            else
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    var bmp = BitmapFactory.DecodeFile(filePath);
                    MemoryStream ms = new MemoryStream();
                    bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                    ms.Seek(0L, SeekOrigin.Begin);
                    // Set the Stream as the completion of the Task
                    CameraImageTaskCompletionSource.SetResult(ms);
                }
                else
                {
                    CameraImageTaskCompletionSource.SetResult(null);
                }
            }
        }

        private Task UpdateNotificationHubInstallation()
        {
            if (!IsPlayServicesAvailable())
                return Task.FromResult(false);

            return new NotificationHubService().SendRegistrationToServer(this, FirebaseInstanceId.Instance.Token);
        }

        private bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            var result = true;
            var resultMessage = "Google Play Services is available."; 
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    resultMessage = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                    resultMessage = "This device is not supported";
                result = false;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(FirebaseInstanceId.Instance.Token);
            }

            System.Diagnostics.Debug.WriteLine(resultMessage);
            return result;
        }
    }
}

