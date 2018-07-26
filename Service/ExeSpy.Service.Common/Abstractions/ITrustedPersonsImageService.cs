using System.IO;

namespace EyeSpy.Service.Common.Abstractions
{
    public interface ITrustedPersonsImageService
    {
        byte[] ConvertImageToPng(byte[] originalImage);
        byte[] ConvertImageToPng(Stream originalImage);
        byte[] ConvertImageToJpg(byte[] originalImage);
        byte[] ConvertImageToJpg(Stream originalImage);
    }
}