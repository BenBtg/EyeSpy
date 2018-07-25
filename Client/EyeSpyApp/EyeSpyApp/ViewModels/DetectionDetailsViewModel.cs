using EyeSpy.Shared;
using System.Threading.Tasks;
using EyeSpyApp.Helpers;

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

        public DetectionDetailsViewModel(string detectionId)
        {
            Title = "Alarmo! Alarmo!";
            _detectionId = detectionId;
        }

        public DetectionDetailsViewModel(Detection detection)
        {
            Title = "Alarmo! Alarmo!";
            _detectionId = detection.Id;
            Detection = detection;
        }

        public async Task Init()
        {
            if (Detection == null)
                Detection = await EyeSpyService.Value.GetDetection(_detectionId);

            Detection.DetectionImageUrl = Detection.ImageReference.WithToken();
        }
    }
}
