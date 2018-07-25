using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EyeSpy.Shared;
using EyeSpyApp.Helpers;
using Xamarin.Forms;
using System.Net.Http;

namespace EyeSpyApp.ViewModels
{
    public class DetectionDetailsViewModel : BaseViewModel
    {
        private string _detectionId;

        private Detection _detection;
        public Detection Detection
        {
            get { return _detection; }
            private set
            {
                SetProperty(ref _detection, value);
            }
        }

        public Command TrustPersonCommand { get; set; }

        public Action OnTrustPersonCommandCompleted { get; set; }

        public DetectionDetailsViewModel(string detectionId)
        {
            Title = "Alarmo! Alarmo!";
            _detectionId = detectionId;
            TrustPersonCommand = new Command(async () => await ExecuteTrustPersonCommand());
        }

        public DetectionDetailsViewModel(Detection detection)
            : this(detection.Id)
        {
            Detection = detection;
        }

        public async Task Init()
        {
            if (Detection == null)
                Detection = await EyeSpyService.Value.GetDetection(_detectionId);

            Detection.DetectionImageUrl = Detection.ImageReference.WithToken();
        }

        private async Task ExecuteTrustPersonCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var client = new HttpClient();
                var imageStream = await client.GetStreamAsync(Detection.DetectionImageUrl);
                var newPerson = new PersonData
                {
                    Name = $"Trusted Person {Environment.TickCount}",
                    ImageStream = imageStream,
                };

                await EyeSpyService.Value.AddTrustedPerson(newPerson);
                OnTrustPersonCommandCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
