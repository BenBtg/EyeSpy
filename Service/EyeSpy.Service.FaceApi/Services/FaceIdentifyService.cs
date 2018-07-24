using System.Threading.Tasks;
using EyeSpy.Service.FaceApi.Models;

namespace EyeSpy.Service.FaceApi.Services
{
    public class FaceIdentifyService : BaseFaceService
    {
        private const string IdentityEndpoint = "identify";

        public FaceIdentifyService(string endpoint, string secret) : base(endpoint, secret) {}

        public async Task<FaceIdentifyResponse> IdentifyFaceAsync(FaceIdentifyRequest faceIdentifyRequest)
        {
            FaceIdentifyResponse result = null;

            try
            {
                result =  await PostAsync<FaceIdentifyResponse, FaceIdentifyRequest>(IdentityEndpoint, faceIdentifyRequest, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
            }
            catch
            {
                // TODO: Log error - note that this will fail if the model has not yet been trained
            }

            return result;            
        }
    }
}