using EyeSpy.Service.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EyeSpy.Service.Common.Abstractions
{
    public interface ITrustedPersonsService
    {
        Task<BaseTrustedPerson> CreateTrustedPersonAsync(string trustedPersonName, byte[] persistedFaceImageData);
        Task<List<BaseTrustedPerson>> GetTrustedPersonsAsync();
        Task<BaseTrustedPerson> GetTrustedPersonByIdAsync(string id);
        Task<bool> DetectIfPersonIsTrustedAsync(byte[] detectedPersonImageData);
    }
}