using System;
using System.Collections.Generic;
using EyeSpyApp.Models;
using EyeSpyApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace EyeSpyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewMemberPage : ContentPage
    {
        public NewMemberPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            var newMemberContext = (NewMemberViewModel)BindingContext;
            newMemberContext.OnSaveMemberCommandCompleted = async () => await Navigation.PopModalAsync();
        }
    }
}