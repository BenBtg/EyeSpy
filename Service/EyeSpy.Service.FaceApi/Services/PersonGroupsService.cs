using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeSpy.Service.FaceApi.Models;

namespace EyeSpy.Service.FaceApi.Services
{
    public class PersonGroupsService : BaseFaceService
    {
        private const string PersonGroupsEndpoint = "persongroups";
        private const string PersonGroupsPersonsTokenizedEndpoint = "persongroups/{0}/persons";
        private const string PersonGroupsPersonsByIdTokenizedEndpoint = "persongroups/{0}/persons/{1}";
        private const string PersistedFacesTokenizedEndpoint = "persongroups/{0}/persons/{1}/persistedFaces";
        private const string PersonGroupsTrainModelTokenizedEndpoint = "persongroups/{0}/train";
        private const string PersonGroupsTrainModelStatusTokenizedEndpoint = "persongroups/{0}/training";

        public PersonGroupsService(string endpoint, string secret) : base(endpoint, secret) { }

        public async Task<List<PersonGroupResult>> GetPersonGroupsAsync()
        {
            return await GetAsync<List<PersonGroupResult>>(PersonGroupsEndpoint, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
        }

        public async Task<PersonGroupResult> CreatePersonGroup(PersonGroup personGroup)
        {
            PersonGroupResult personGroupResult = null;

            try
            {
                personGroupResult = await PostAsync<PersonGroupResult, PersonGroup>(PersonGroupsEndpoint, personGroup, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
            }
            catch
            {
                // TODO: Log exception here
            }

            return personGroupResult;
        }        

        public async Task<PersonGroupResult> CreatePersonGroupIfNotExistsAsync(string personGroupName)
        {
            PersonGroupResult personGroupResult = (await this.GetPersonGroupsAsync())?.Where(i => i.Name == personGroupName).FirstOrDefault();

            if (personGroupResult == null)
                personGroupResult = await this.CreatePersonGroup(new PersonGroup { Name = personGroupName });

            return personGroupResult;
        }

        public async Task<PersonGroupPersonResult> GetPersonGroupPersonById(string id, string personGroupId)
        {
            PersonGroupPersonResult personGroupPersonResult = null;
            var personGroupPersonsByIdEndpoint = string.Format(PersonGroupsPersonsByIdTokenizedEndpoint, personGroupId, id);

            try
            {
                personGroupPersonResult = await GetAsync<PersonGroupPersonResult>(personGroupPersonsByIdEndpoint, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
            }
            catch
            {
                // TODO: Log exception
            }

            return personGroupPersonResult;
        }

        public async Task<List<PersonGroupPersonResult>> GetPersonGroupPersons(string personGroupId)
        {
            List<PersonGroupPersonResult> personGroupPersonResults = null;
            var personGroupPersonsEndpoint = string.Format(PersonGroupsPersonsTokenizedEndpoint, personGroupId);

            try
            {
                personGroupPersonResults = await GetAsync<List<PersonGroupPersonResult>>(personGroupPersonsEndpoint, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
            }
            catch
            {
                // TODO: Log exception
            }

            return personGroupPersonResults;
        }

        public async Task<BasePersonGroupPersonResult> CreatePersonGroupPerson(PersonGroupPerson personGroupPerson, string personGroupId)
        {
            BasePersonGroupPersonResult personGroupPersonResult = null;
            var personGroupPersonsEndpoint = string.Format(PersonGroupsPersonsTokenizedEndpoint, personGroupId);

            try
            {
                personGroupPersonResult = await PostAsync<BasePersonGroupPersonResult, PersonGroupPerson>(personGroupPersonsEndpoint, personGroupPerson, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
            }
            catch
            {
                // TODO: Log exception
            }

            return personGroupPersonResult;
        }      
        
        public async Task<PersistedFaceResult> CreatePersonGroupPersonPersistedFace(byte[] persistedFaceImageData, string personGroupPersonId, string personGroupId)
        {
            PersistedFaceResult persistedFaceResult = null;
            var personGroupPersonsPersistedFacesEndpoint = string.Format(PersistedFacesTokenizedEndpoint, personGroupId, personGroupPersonId);

            try
            {
                persistedFaceResult = await PostAsync<PersistedFaceResult>(personGroupPersonsPersistedFacesEndpoint, persistedFaceImageData, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
            }
            catch
            {
                // TODO: Log exception
            }

            return persistedFaceResult;
        }

        // TODO: Coalesce these operations!
        public async Task<bool> TrainModelAndWaitCompletionAsync(string personGroupId)
        {
            bool success = await this.TrainModelAsync(personGroupId);

            if (success)
            {
                var trainModelStatusEndpoint = string.Format(PersonGroupsTrainModelStatusTokenizedEndpoint, personGroupId);
                var timeoutTicks = DateTime.UtcNow.AddMilliseconds(10000).Ticks;
                ModelTrainingResult modelTrainingResult = null;

                do
                {
                    await Task.Delay(250);
                    modelTrainingResult = await this.GetAsync<ModelTrainingResult>(trainModelStatusEndpoint, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
                }
                while (DateTime.UtcNow.Ticks < timeoutTicks && modelTrainingResult?.Status != "succeeded");

                success = modelTrainingResult?.Status == "succeeded";
            }

            return success;
        }

        public async Task<bool> TrainModelAsync(string personGroupId)
        {
            var trainModelEndpoint = string.Format(PersonGroupsTrainModelTokenizedEndpoint, personGroupId);

            try
            {
                await this.PostAsync(trainModelEndpoint, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
            }
            catch
            {
                // TODO: Log exception here!
                return false;
            }

            return true;
        }
    }
}