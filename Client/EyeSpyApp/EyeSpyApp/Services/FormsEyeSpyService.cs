using System;
using EyeSpy.Shared;

[assembly: Xamarin.Forms.Dependency(typeof(EyeSpyApp.Services.FormsEyeSpyService))]

namespace EyeSpyApp.Services
{
    public class FormsEyeSpyService : EyeSpyService
    {
        public FormsEyeSpyService()
        {
        }
    }
}
