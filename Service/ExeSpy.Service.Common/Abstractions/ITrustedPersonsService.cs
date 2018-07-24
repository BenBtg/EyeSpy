using System.Threading.Tasks;

namespace EyeSpy.Service.Common.Abstractions
{
    public interface ITrustedPersonsService
    {
        Task<bool> DetectIfPersonIsTrustedAsync(byte[] detectedPersonImageData);
    }
}