using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace EyeSpyApp.Services
{
    public interface IImagePickerService
    {
        Task<List<Stream>> PickImage();
        Task<Stream> TakePhoto();
    }
}
