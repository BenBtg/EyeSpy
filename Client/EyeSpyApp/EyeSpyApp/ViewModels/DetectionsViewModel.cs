using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using EyeSpy.Shared;
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
                Detections.Clear();
                detections.ForEach(Detections.Add);
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