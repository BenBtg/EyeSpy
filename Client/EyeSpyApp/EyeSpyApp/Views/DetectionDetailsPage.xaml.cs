using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EyeSpyApp.ViewModels;

namespace EyeSpyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetectionDetailsPage : ContentPage
    {
        public DetectionDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var detectionDetailsViewModel = (DetectionDetailsViewModel)BindingContext;
            detectionDetailsViewModel.OnTrustPersonCommandCompleted = async () => await Navigation.PopAsync();
        }
    }
}
