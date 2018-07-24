using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;


namespace EyeSpyService
{
    public class EyeSpyAPI
    {
        private static HttpClient _eyeSpyServiceClient { get; set; }

        public Func<Task<Stream>> GetImageStreamCallback { get; set; }

        public byte[] Data { get; set; }


        public EyeSpyAPI()
        {
            _eyeSpyServiceClient = new HttpClient();
            _eyeSpyServiceClient.DefaultRequestHeaders.Add("apikey", "ea3c0fb8-f7fc-4376-830a-d5c920098689");
            _eyeSpyServiceClient.BaseAddress = new Uri("https://eyespyservicehack2018.azurewebsites.net/api");
        
            //this.Data = data;
            //this.GetImageStreamCallback = () => Task.FromResult<Stream>(new MemoryStream(this.Data));
        }

        public async Task IdentifyFacesAsync(byte[] data)
        {
            var content = new ByteArrayContent(data);
            var response = await _eyeSpyServiceClient.PostAsync("api/detections", content);
            Console.WriteLine(response.StatusCode.ToString());
            Console.WriteLine(response.Content);

        }


    }
}
