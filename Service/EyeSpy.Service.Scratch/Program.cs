using EyeSpy.Service.AzureStorage.Services;
using EyeSpy.Service.Common;
using EyeSpy.Service.Common.Abstractions;
using EyeSpy.Service.Common.Models;
using EyeSpy.Service.FaceApi.Services;
using EyeSpy.Service.NotificationHub.Models;
using EyeSpy.Service.NotificationHub.Services;
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
            //TestAddPersonService().Wait();
            TestNotification().Wait();
        }

        private static async Task TestNotification()
        {
            var hubConfig = new NotificationHubConfiguration("EyeSpyNotificationNamespaceHack2018", "EyeSpyNotificationHubHack2018", "DefaultFullSharedAccessSignature", "6hUFdXZvxHmExKOi7iht3M8ZcAHxbejg0L/LzgezsXQ=");
            ITrustedPersonsNotifications trustedPersonsNotifications = new AzureTrustedPersonsNotifications(hubConfig);

            var baseAddressTest = hubConfig.BaseAddress;

            var tokenTestPart1 = hubConfig.GetToken();
            var tokenTestPart2 = hubConfig.GetToken();

            var testSend = await trustedPersonsNotifications.SendDetectionNotificationAsync<DetectionNotification>(new DetectionNotification
            {
                Title = "Eye Spy Alert",
                Message = "Unrecognized person detected",
                DetectionId = "195d6b0d-9ddb-4266-9db8-9b554ad0a8eb",
                ImageReference = "path=dtcontainer&name=195d6b0d-9ddb-4266-9db8-9b554ad0a8eb"
            });

            var x = 0;
        }

        private static async Task TestAddPersonService()
        {
            ITrustedPersonsFaceRecognition trustedPersonsService = new FaceApiTrustedPersonsFaceRecognition("<face_api_endpoint>", "<face_subscription_key>");

            byte[] trustedPersonImage;

            using (var filestream = File.Open(@"C:\Users\mikep\Desktop\FACE\nonmatchtest.png", FileMode.Open))
                trustedPersonImage = filestream.ToBytes();

            var trustedPerson = await trustedPersonsService.CreateTrustedPersonAsync("Ben Buttigieg", trustedPersonImage);
            var trustedPersons = await trustedPersonsService.GetTrustedPersonsAsync();
            var lastTrustedPerson = await trustedPersonsService.GetTrustedPersonByIdAsync(trustedPersons.LastOrDefault()?.Id); 
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
