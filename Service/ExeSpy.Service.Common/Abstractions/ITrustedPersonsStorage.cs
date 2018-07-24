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
        Task<Detection> CreateDetectionAsync(BaseDetection detection, byte[] detectionImageData);
        Task<Detection> GetDetectionByIdAsync(string id);
        Task<IList<Detection>> GetDetectionsAsync();
    }
}