using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EyeSpy.Shared
{
    public interface IEyeSpyService
    {
        Task<List<Person>> GetTrustedPersons();
        Task<Person> GetTrustedPerson(string id);
        Task AddTrustedPerson(PersonData newPerson);

        Task<List<Detection>> GetDetections();
        Task<Detection> GetDetection(string id);
        Task<bool> Detect(PersonData person);
    }
}
