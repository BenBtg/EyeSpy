using System.Threading.Tasks;
using EyeSpy.Service.Common.Models;

namespace EyeSpy.Service.Common.Abstractions
{
    public interface ITrustedPersonsStorage
    {
        Task<bool> CreateTrustedPersonAsync(TrustedPerson trustedPerson, byte[] trustedPersonImageData);
    }
}