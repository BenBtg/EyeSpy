using System.IO;
using EyeSpy.Service.Common;
using EyeSpy.Service.Common.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace EyeSpy.Service.Imaging.Services
{
    public class ImageSharpTrustedPersonsImageService : ITrustedPersonsImageService
    {
        public byte[] ConvertImageToPng(byte[] originalImage)
        {
            if (Image.DetectFormat(originalImage) == ImageFormats.Png)
                return originalImage;

            return this.ConvertImageToPng(this.LoadImageFromBytes(originalImage));
        }

        public byte[] ConvertImageToPng(Stream originalImage)
        {
            if (Image.DetectFormat(originalImage) == ImageFormats.Png)
                return originalImage.ToBytes();

            return this.ConvertImageToPng(this.LoadImageFromStream(originalImage));
        }

        public byte[] ConvertImageToJpg(byte[] originalImage)
        {
            if (Image.DetectFormat(originalImage) == ImageFormats.Jpeg)
                return originalImage;

            return this.ConvertImageToJpg(this.LoadImageFromBytes(originalImage));
        }

        public byte[] ConvertImageToJpg(Stream originalImage)
        {
            if (Image.DetectFormat(originalImage) == ImageFormats.Jpeg)
                return originalImage.ToBytes();

            return this.ConvertImageToJpg(this.LoadImageFromStream(originalImage));
        }

        private Image<Rgba32> LoadImageFromBytes(byte[] originalImage)
        {
            Image<Rgba32> image = null;

            try
            {
                image = Image.Load(originalImage);
            }
            catch
            {
                // TODO: Log error
            }

            return image;
        }

        private Image<Rgba32> LoadImageFromStream(Stream originalImage)
        {
            Image<Rgba32> image = null;

            try
            {
                image = Image.Load(originalImage);
            }
            catch
            {
                // TODO: Log error
            }

            return image;
        }

        private byte[] ConvertImageToPng(Image<Rgba32> image)
        {
            if (image == null)
                return null;

            byte[] transformedImage = null;

            using (var memoryStream = new MemoryStream())
            {
                image.SaveAsPng(memoryStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder() { CompressionLevel = 9, PngColorType = SixLabors.ImageSharp.Formats.Png.PngColorType.Rgb });
                transformedImage = memoryStream.ToArray();
            }

            return transformedImage;
        }

        private byte[] ConvertImageToJpg(Image<Rgba32> image)
        {
            if (image == null)
                return null;

            byte[] transformedImage = null;

            using (var memoryStream = new MemoryStream())
            {
                image.SaveAsJpeg(memoryStream);
                transformedImage = memoryStream.ToArray();
            }

            return transformedImage;
        }
    }
}