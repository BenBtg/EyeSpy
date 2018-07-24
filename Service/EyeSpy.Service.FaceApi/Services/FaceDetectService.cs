using System;
using System.Threading.Tasks;
using EyeSpy.Service.FaceApi.Models;

namespace EyeSpy.Service.FaceApi.Services
{
    public class FaceDetectService : BaseFaceService
    {
        private const string DetectEndpoint = "detect";

        public FaceDetectService(string endpoint, string secret) : base(endpoint, secret) { }

        public async Task<FaceDetectResponse> DetectFaceAsync(byte[] faceDetectImageBytes)
        {
            if (faceDetectImageBytes == null)
                throw new Exception($"Parameter {nameof(faceDetectImageBytes)} cannot be null");

            FaceDetectResponse result = null;

            try
            {
                result = await PostAsync<FaceDetectResponse>(DetectEndpoint, faceDetectImageBytes, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
            }
            catch
            {
                // TODO: Log error - this will fail if the model has not been trained
            }            

            return result;
        }
    }
}
