using EyeSpy.Service.AzureStorage.Services;
using EyeSpy.Service.Common;
using EyeSpy.Service.Common.Abstractions;
using EyeSpy.Service.Common.Models;
using EyeSpy.Service.FaceApi.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EyeSpy.Service.Scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //TestStorageAsync().Wait();
            //TestRetrievalAsync().Wait();
            TestAddPersonService().Wait();
        }

        private static async Task TestAddPersonService()
        {
            ITrustedPersonsService trustedPersonsService = new FaceApiTrustedPersonsService("<face_api_endpoint>", "<face_subscription_key>");

            byte[] trustedPersonImage;

            using (var filestream = File.Open(@"C:\Users\mikep\Desktop\FACE\nonmatchtest.png", FileMode.Open))
                trustedPersonImage = filestream.ToBytes();

            var trustedPerson = await trustedPersonsService.CreateTrustedPersonAsync("Ben Buttigieg", trustedPersonImage);
            var trustedPersons = await trustedPersonsService.GetTrustedPersonsAsync();
            var lastTrustedPerson = await trustedPersonsService.GetTrustedPersonByIdAsync(trustedPersons.LastOrDefault()?.Id); 

            

            var x = 0;
        }

        private static async Task TestRetrievalAsync()
        {
            ITrustedPersonsStorage trustedPersonsStorage = new AzureTrustedPersonsStorage("<account_name>", "<account_key>");
            var trustedPersons = await trustedPersonsStorage.GetTrustedPersonsAsync();
            var trustedPerson = await trustedPersonsStorage.GetTrustedPersonByIdAsync(trustedPersons.FirstOrDefault().Id);
            var detections = await trustedPersonsStorage.GetDetectionsAsync();
            var detection = await trustedPersonsStorage.GetDetectionByIdAsync(detections.FirstOrDefault().Id);
            var x = 0;
        }

        private static async Task TestStorageAsync()
        {
            ITrustedPersonsStorage trustedPersonsStorage = new AzureTrustedPersonsStorage("<account_name>", "<account_key>");

            byte[] trustedPersonImage;

            using (var filestream = File.Open(@"C:\Users\mikep\Desktop\FACE\joebloggs.jpg", FileMode.Open))
                trustedPersonImage = filestream.ToBytes();

            var trustedPerson = await trustedPersonsStorage.CreateTrustedPersonAsync(new BaseTrustedPerson { Id = Guid.NewGuid().ToString().ToLower(), Name = "Joe Bloggs" }, trustedPersonImage);

            var detection = await trustedPersonsStorage.CreateDetectionAsync(new BaseDetection { Id = Guid.NewGuid().ToString().ToLower() }, trustedPersonImage);
        }
    }
}
