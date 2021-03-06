﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeSpy.Service.Common.Abstractions;
using EyeSpy.Service.Common.Models;
using EyeSpy.Service.FaceApi.Models;

namespace EyeSpy.Service.FaceApi.Services
{
    public class FaceApiTrustedPersonsFaceRecognition : ITrustedPersonsFaceRecognition
    {
        private readonly string apiEndpoint;
        private readonly string subscriptionKey;
        private const string KnownPersonsGroupName = "known_persons_group";
        private bool initialized;
        private string personGroupId;
        private PersonGroupsService personGroupsService;
        private FaceDetectService faceDetectService;
        private FaceIdentifyService faceIdentifyService;

        public FaceApiTrustedPersonsFaceRecognition(string faceApiEndpoint, string faceApiSubscriptionKey)
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

        public async Task<BaseTrustedPerson> CreateTrustedPersonAsync(string trustedPersonName, byte[] persistedFaceImageData)
        {
            var personGroupPerson = await this.personGroupsService.CreatePersonGroupPerson(new PersonGroupPerson { Name = trustedPersonName }, personGroupId);
            var persistedFaceResponse = await this.personGroupsService.CreatePersonGroupPersonPersistedFace(persistedFaceImageData, personGroupPerson.PersonId, personGroupId);
            await this.personGroupsService.TrainModelAsync(personGroupId);

            return new BaseTrustedPerson { Id = personGroupPerson.PersonId, Name = trustedPersonName };
        }

        public async Task<List<BaseTrustedPerson>> GetTrustedPersonsAsync()
        {
            var personGroupPersons = await this.personGroupsService.GetPersonGroupPersons(personGroupId);
            return personGroupPersons.Select(i => new BaseTrustedPerson { Id = i.PersonId, Name = i.Name }).ToList();
        }

        public async Task<BaseTrustedPerson> GetTrustedPersonByIdAsync(string id)
        {
            var personGroupPerson = await this.personGroupsService.GetPersonGroupPersonById(id, personGroupId);
            return new BaseTrustedPerson { Id = personGroupPerson.PersonId, Name = personGroupPerson.Name };
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

            //// Train the model - this will fail if there are no persons or faces to train it with
            //var modelTrained = await this.personGroupsService.TrainModelAndWaitCompletionAsync(personGroupId);

            //if (!modelTrained)
            //    throw new Exception($"Unable to train the {KnownPersonsGroupName} Face Api Model");

            this.initialized = true;
        }
    }
}