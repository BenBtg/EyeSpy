using System.Collections.Generic;
using System.Threading.Tasks;
using EyeSpy.Service.Common.Models;

namespace EyeSpy.Service.Common.Abstractions
{
    public interface ITrustedPersonsStorage
    {
        Task<TrustedPerson> CreateTrustedPersonAsync(BaseTrustedPerson trustedPerson, byte[] trustedPersonImageData);
        Task<TrustedPerson> GetTrustedPersonByIdAsync(string id);
        Task<IList<TrustedPerson>> GetTrustedPersonsAsync();
    }
}