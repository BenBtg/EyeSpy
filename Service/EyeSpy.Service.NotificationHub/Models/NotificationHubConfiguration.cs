using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EyeSpy.Service.NotificationHub.Models
{
    public class NotificationHubConfiguration
    {
        public string Namespace { get; private set; }
        public string HubName { get; private set; }
        public string KeyName { get; private set; }
        public string Key { get; private set; }

        private string token;
        private DateTime tokenExpiry;

        public string BaseAddress
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Namespace) || string.IsNullOrWhiteSpace(this.HubName))
                    return string.Empty;

                return $"https://{this.Namespace}.servicebus.windows.net/{this.HubName}/";
            }
        }

        public NotificationHubConfiguration(string hubNamespace, string hubName, string keyName, string key)
        {
            if (string.IsNullOrWhiteSpace(hubNamespace) ||
                   string.IsNullOrWhiteSpace(hubName) ||
                   string.IsNullOrWhiteSpace(keyName) ||
                   string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException($"Constructor parameters must not be null or whitespace");

            this.Namespace = hubNamespace;
            this.HubName = hubName;
            this.KeyName = keyName;
            this.Key = key;
        }

        public string GetToken()
        {
            if (tokenExpiry >= DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(30)) && !string.IsNullOrWhiteSpace(token))
                return this.token;

            var expiryInMilliseconds = 3600;
            this.tokenExpiry = DateTime.UtcNow.AddHours((expiryInMilliseconds / 60) / 60); // NOTE: AddMilliseconds does not appear to work!
            var currentEpochValue = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = Convert.ToString((int)currentEpochValue.TotalSeconds + expiryInMilliseconds);            
            string stringToSign = HttpUtility.UrlEncode(this.BaseAddress) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(this.Key));
            var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
            var signature = Convert.ToBase64String(hashValue);
            var encodedSig = HttpUtility.UrlEncode(signature);

            this.token = String.Format(
                CultureInfo.InvariantCulture, 
                "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", 
                HttpUtility.UrlEncode(this.BaseAddress), 
                HttpUtility.UrlEncode(signature), 
                expiry, 
                this.KeyName);

            return this.token;
        }
    }
}