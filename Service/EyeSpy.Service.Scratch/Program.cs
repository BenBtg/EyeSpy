using EyeSpy.Service.AzureStorage.Services;
using EyeSpy.Service.Common;
using EyeSpy.Service.Common.Abstractions;
using EyeSpy.Service.Common.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EyeSpy.Service.Scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestStorageAsync().Wait();
            var test = 0;
        }

        private static async Task TestStorageAsync()
        {
            ITrustedPersonsStorage trustedPersonsStorage = new AzureTrustedPersonsStorage("<account_name>", "<account_key>");

            byte[] trustedPersonImage;

            using (var filestream = File.Open(@"C:\Users\mikep\Desktop\FACE\johnparker.jpg", FileMode.Open))
                trustedPersonImage = filestream.ToBytes();

            var trustedPerson = await trustedPersonsStorage.CreateTrustedPersonAsync(new BaseTrustedPerson { Id = Guid.NewGuid().ToString().ToLower(), Name = "John Parker" }, trustedPersonImage);

            var y = 0;
        }
    }
}
