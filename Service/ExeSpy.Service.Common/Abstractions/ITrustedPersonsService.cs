using System.Threading.Tasks;

namespace ExeSpy.Service.Common.Abstractions
{
    public interface ITrustedPersonsService
    {
        Task<bool> DetectIfPersonIsTrustedAsync(byte[] detectedPersonImageData);
    }
}