using System;
using System.IO;
using System.Threading.Tasks;
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
            newMemberContext.OnMemberImageSelected = async () => await RenderSelectedImage();
        }

        public async Task RenderSelectedImage()
        {
            NewMemberImage.Source = null;
            var newMemberContext = (NewMemberViewModel)BindingContext;
            if (!newMemberContext.IsMemberImageStreamDefined)
                return;
            
            var selectedImage = newMemberContext.MemberImageStream;
            var imageCopy = new MemoryStream();
            await selectedImage.CopyToAsync(imageCopy);
            imageCopy.Seek(0, SeekOrigin.Begin);
            selectedImage.Seek(0, System.IO.SeekOrigin.Begin);
            NewMemberImage.Source = ImageSource.FromStream(() => imageCopy);
        }

        private void Handle_Tapped(object sender, System.EventArgs e)
        {
            SelectImageAsync();
        }

        private async Task SelectImageAsync()
        {
            const string OptionPickFromLibrary = "Pick from library";
            const string OptionTakePhoto = "Take a photo";
            var selectedOption = await DisplayActionSheet("Select new member image", "Cancel", null, new[] { OptionPickFromLibrary, OptionTakePhoto });
            var newMemberContext = (NewMemberViewModel)BindingContext;
            switch (selectedOption)
            {
                case OptionPickFromLibrary:
                    newMemberContext.PickImageCommand.Execute(null);
                    break;
                case OptionTakePhoto:
                    newMemberContext.TakePhotoCommand.Execute(null);
                    break;
            }
        }
    }
}