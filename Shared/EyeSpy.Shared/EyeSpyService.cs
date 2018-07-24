using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EyeSpy.Shared
{
    public class EyeSpyService : BaseHttpService, IEyeSpyService
    {
        public Task<List<Person>> GetTrustedPersons() => GetAsync<List<Person>>($"trustedpersons");

        public Task AddTrustedPerson(PersonData newPerson)
        {
            //TODO: implement

            return Task.FromResult(true);
        }

        public Task<bool> ValidaPerson(PersonData person)
        {
            //TODO: implement

            return Task.FromResult(true);
        }

    }
}