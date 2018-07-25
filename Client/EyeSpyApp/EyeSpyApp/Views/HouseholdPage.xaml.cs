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
    public partial class HouseholdPage : ContentPage
    {
        public HouseholdViewModel ViewModel => (HouseholdViewModel)BindingContext;

        public HouseholdPage()
        {
            InitializeComponent();
        }

        async void OnMemberSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var member = args.SelectedItem as HouseholdMember;
            if (member == null)
                return;

            var memberDetailsContext = new MemberDetailsViewModel(member);
            var detailsPage = new MemberDetailsPage() { BindingContext = memberDetailsContext};
            await Navigation.PushAsync(detailsPage);
            MembersListView.SelectedItem = null;
        }

        async void AddMember_Clicked(object sender, EventArgs e)
        {
            var newMemberPage = new NewMemberPage();
            await Navigation.PushAsync(newMemberPage);
        }

        async Task OpenDetectionDetails(string detectionId)
        {
            var detectionDetailsViewModel = new DetectionDetailsViewModel(detectionId);
            await detectionDetailsViewModel.Init();
            var detectionDetails = new DetectionDetailsPage() { BindingContext = detectionDetailsViewModel };
            await Navigation.PushAsync(detectionDetails);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel.Members.Count == 0)
                ViewModel.LoadMembersCommand.Execute(null);

            var detectionsId = EyeSpyApp.Services.AppState.DetectionsId;
            if(!string.IsNullOrWhiteSpace(detectionsId))
            {
                EyeSpyApp.Services.AppState.DetectionsId = null;
                OpenDetectionDetails(detectionsId);
            }
        }
    }
}