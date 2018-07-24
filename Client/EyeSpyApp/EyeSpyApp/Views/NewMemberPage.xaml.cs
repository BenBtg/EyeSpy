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
            var newMemberContext = (NewMemberViewModel)BindingContext;
            newMemberContext.OnSaveMemberCommandCompleted = async () => await Navigation.PopModalAsync();
        }
    }
}