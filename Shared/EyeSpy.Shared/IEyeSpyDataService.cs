using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EyeSpy.Shared
{
    public interface IEyeSpyService
    {
        Task<List<Person>> GetTrustedPersons();
        Task AddTrustedPerson(PersonData newPerson);
        Task<bool> ValidaPerson(PersonData person);
    }
}
