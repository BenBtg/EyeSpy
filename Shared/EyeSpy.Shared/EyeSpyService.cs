using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EyeSpy.Shared
{
    public class EyeSpyService : BaseHttpService, IEyeSpyService
    {
        public Task<List<Person>> GetTrustedPersons() => GetAsync<List<Person>>($"trustedpersons");

        public async Task AddTrustedPerson(PersonData newPerson)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidaPerson(PersonData person)
        {
            throw new NotImplementedException();
        }

    }
}