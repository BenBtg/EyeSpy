using System.Net.Http;
using System.Threading.Tasks;
using ExeSpy.Service.Common.Services;

namespace EyeSpy.Service.FaceApi.Services
{
    public class BaseFaceService : BaseHttpService
    {
        private const string SubscriptionHeaderKey = "Ocp-Apim-Subscription-Key";
        private readonly string Secret;

        public BaseFaceService(string endpoint, string secret) : base(endpoint)
        {
            this.Secret = secret;
        }

        internal void ConfigureRequestWithSubscriptionHeader(HttpRequestMessage request)
        {
            request.Headers.Add(SubscriptionHeaderKey, this.Secret);
        }
    }
}