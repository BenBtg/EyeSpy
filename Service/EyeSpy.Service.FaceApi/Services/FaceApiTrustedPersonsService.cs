using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExeSpy.Service.Common.Abstractions;
using EyeSpy.Service.FaceApi.Models;

namespace EyeSpy.Service.FaceApi.Services
{
    public class FaceApiTrustedPersonsService : ITrustedPersonsService
    {
        private readonly string apiEndpoint;
        private readonly string subscriptionKey;
        private const string KnownPersonsGroupName = "known_persons_group";
        private bool initialized;
        private string personGroupId;
        private PersonGroupsService personGroupsService;
        private FaceDetectService faceDetectService;
        private FaceIdentifyService faceIdentifyService;

        public FaceApiTrustedPersonsService(string faceApiEndpoint, string faceApiSubscriptionKey)
        {
            if (string.IsNullOrWhiteSpace(faceApiEndpoint) || string.IsNullOrWhiteSpace(faceApiSubscriptionKey))
                throw new ArgumentNullException($"{nameof(faceApiEndpoint)} and {nameof(faceApiSubscriptionKey)} must have values assigned");

            this.apiEndpoint = faceApiEndpoint;
            this.subscriptionKey = faceApiSubscriptionKey;
            this.InitAsync().Wait(); // TODO: Remove blocking in favour of waiting for initialization to be completed upon first call
        }

        public async Task<bool> DetectIfPersonIsTrustedAsync(byte[] detectedPersonImageData)
        {
            var faceDetectResponse = (await this.faceDetectService.DetectFaceAsync(detectedPersonImageData))?.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(faceDetectResponse?.FaceId))
                return false;

            var detectedFaceId = faceDetectResponse.FaceId;

            var faceIdentifyRequest = new FaceIdentifyRequest
            {
                FaceIds = new List<string> { detectedFaceId },
                PersonGroupId = personGroupId,
                ConfidenceThreshold = 0.5f,
                MaxNumOfCandidatesReturned = 1
            };

            var faceIdentifyResponse = (await this.faceIdentifyService.IdentifyFaceAsync(faceIdentifyRequest))?.FirstOrDefault();

            return faceIdentifyResponse?.Candidates?.Count > 0;
        }

        private async Task InitAsync()
        {
            if (this.initialized)
                return;

            // Initialize foundational Face API services
            this.personGroupsService = new PersonGroupsService(this.apiEndpoint, this.subscriptionKey);
            this.faceDetectService = new FaceDetectService(this.apiEndpoint, this.subscriptionKey);
            this.faceIdentifyService = new FaceIdentifyService(this.apiEndpoint, this.subscriptionKey);

            // Create the 'known persons' PersonGroup if it does not exist and store the id
            personGroupId = (await this.personGroupsService.CreatePersonGroupIfNotExistsAsync(KnownPersonsGroupName))?.PersonGroupId;

            if (string.IsNullOrWhiteSpace(personGroupId))
                throw new Exception($"Unable to initialize the {KnownPersonsGroupName} Face Api Person Group");

            // Train the model
            var modelTrained = await this.personGroupsService.TrainModelAsync(personGroupId);

            if (!modelTrained)
                throw new Exception($"Unable to train the {KnownPersonsGroupName} Face Api Model");

            this.initialized = true;
        }
    }
}