using System;
using System.Collections.Generic;
using EyeSpyApp.Models;
using EyeSpyApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EyeSpyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewMemberPage : ContentPage
    {
        public NewMemberPage()
        {
            InitializeComponent();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            var newMemberContext = (NewMemberViewModel)BindingContext;
            MessagingCenter.Send(this, "AddMember", newMemberContext.Member);
            await Navigation.PopModalAsync();
        }
    }
}