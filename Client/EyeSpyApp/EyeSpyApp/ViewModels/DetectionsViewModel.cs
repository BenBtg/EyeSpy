using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EyeSpy.Shared;
using EyeSpyApp.Helpers;
using Xamarin.Forms;

namespace EyeSpyApp.ViewModels
{
    public class DetectionsViewModel : BaseViewModel
    {
        public ObservableCollection<Detection> Detections { get; set; }
        public Command LoadDetectionsCommand { get; set; }

        public DetectionsViewModel()
        {
            Title = "Detections";
            Detections = new ObservableCollection<Detection>();
            LoadDetectionsCommand = new Command(async () => await ExecuteLoadDetectionsCommand());
        }

        private async Task ExecuteLoadDetectionsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var detections = await EyeSpyService.Value.GetDetections();
                detections = detections.OrderByDescending(d => d.DetectionTimestamp ?? DateTime.MinValue).ToList();
                Detections.Clear();
                detections.ForEach(d=> 
                {
                    d.DetectionImageUrl = d.ImageReference.WithToken();
                    Detections.Add(d);
                });
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