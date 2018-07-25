using System;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using EyeSpy.Shared;

namespace EyeSpyApp.Helpers
{
    public static class ImageTokenHelper
    {
        public static string WithToken(this string resourceKey)
        {
            if (string.IsNullOrWhiteSpace(resourceKey))
                return null;

            var result = $"{Constants.EyeSpyApiUrl}images?{resourceKey}&apikey={Constants.EyeSpyApiKey}";
            return result;
        }
    }
}
