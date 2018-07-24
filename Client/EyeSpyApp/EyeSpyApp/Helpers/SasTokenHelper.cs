using System;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using EyeSpy.Shared;

namespace EyeSpyApp.Helpers
{
    public static class SasTokenHelper
    {
        public static string WithSasToken(this string resourceUri, string keyName, string key = Constants.EyeSpyTokenKey)
        {
            if (string.IsNullOrWhiteSpace(resourceUri))
                return null;

            var sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var week = 60 * 60 * 24 * 7;
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + week);
            var stringToSign = Uri.EscapeUriString(resourceUri) + "\n" + expiry;
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture, "sr={0}&sig={1}&se={2}&skn={3}", 
                                         Uri.EscapeUriString(resourceUri),
                                         Uri.EscapeUriString(signature), 
                                         expiry, 
                                         keyName);

            var result = $"{resourceUri}?{sasToken}";
            return result;
        }
    }
}
