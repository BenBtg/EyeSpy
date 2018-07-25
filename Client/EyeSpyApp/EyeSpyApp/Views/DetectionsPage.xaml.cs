using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeSpyApp.Models;
using EyeSpyApp.ViewModels;
using EyeSpyApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EyeSpyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetectionsPage : ContentPage
    {
        public DetectionsViewModel ViewModel => (DetectionsViewModel)BindingContext;

        public DetectionsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel.Detections.Count == 0)
                ViewModel.LoadDetectionsCommand.Execute(null);
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            DetectionsListView.SelectedItem = null;
        }
    }
}