using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using ImageCircle.Forms.Plugin.iOS;

namespace EyeSpyApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ApplyStyle();

            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        private void ApplyStyle()
        {
            UINavigationBar.Appearance.Translucent = false;
            UINavigationBar.Appearance.TintColor = UIColor.White;//.FromRGB(0x8b, 0xc3, 0x4a);
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(0x8b, 0xc3, 0x4a);
            UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGB(0x8b, 0xc3, 0x4a);

            UITabBar.Appearance.BackgroundColor = UIColor.FromRGB(0x8b, 0xc3, 0x4a);
            UITabBar.Appearance.TintColor = UIColor.White;
        }
    }
}
