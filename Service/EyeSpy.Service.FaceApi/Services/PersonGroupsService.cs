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

        // TODO: Coalesce these operations!
        public async Task<bool> TrainModelAsync(string personGroupId)
        {
            bool success = false;
            var trainModelEndpoint = string.Format(PersonGroupsTrainModelTokenizedEndpoint, personGroupId);
            var trainModelStatusEndpoint = string.Format(PersonGroupsTrainModelStatusTokenizedEndpoint, personGroupId);

            try
            {
                await this.PostAsync(trainModelEndpoint, (request) => this.ConfigureRequestWithSubscriptionHeader(request));
                success = true;
            }
            catch
            {
                // TODO: Log exception here!
            }

            if (success)
            {
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
    }
}