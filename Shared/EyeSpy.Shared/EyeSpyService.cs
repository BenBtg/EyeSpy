using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyeSpy.Shared
{
    public class EyeSpyService : BaseHttpService, IEyeSpyService
    {
        public Task<List<Person>> GetTrustedPersons() => GetAsync<List<Person>>($"trustedpersons");

        public Task AddTrustedPerson(PersonData newPerson) => PostAsync<object>(
            $"trustedpersons?name={Uri.EscapeUriString(newPerson?.Name)}",
            modifyRequest: r =>
            {
                var streamContent = new StreamContent(newPerson?.ImageStream);
                r.Content = streamContent;
            });

        public Task<bool> ValidaPerson(PersonData person) => PostAsync<bool>(
            $"detections",
            modifyRequest: r =>
            {
                var streamContent = new StreamContent(person?.ImageStream);
                r.Content = streamContent;
            });
    }
}