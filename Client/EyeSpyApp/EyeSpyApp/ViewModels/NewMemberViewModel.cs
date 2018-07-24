using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using EyeSpy.Shared;
using EyeSpyApp.Models;

namespace EyeSpyApp.ViewModels
{
    public class NewMemberViewModel : BaseViewModel
    {
        public HouseholdMember Member { get; }

        private Stream _memberImageStream;
        public Stream MemberImageStream
        {
            get { return _memberImageStream; }
            set
            {
                if (SetProperty(ref _memberImageStream, value))
                {
                    OnPropertyChanged(nameof(IsMemberImageStreamDefined));
                    OnPropertyChanged(nameof(CanSaveMember));
                }
            }
        }

        public bool CanSaveMember => IsNotBusy && IsMemberImageStreamDefined;

        public bool IsMemberImageStreamDefined => MemberImageStream != null;

        public Action OnSaveMemberCommandCompleted { get; set; }

        public Action OnMemberImageSelected { get; set; }

        public ICommand PickImageCommand { get; } 

        public ICommand TakePhotoCommand { get; } 

        public ICommand SaveMemberCommand { get; } 

        public NewMemberViewModel()
        {
            PickImageCommand = new Command(async () => await OnPickImageCommandExecuted());
            TakePhotoCommand = new Command(async () => await OnTakePhotoCommandExecuted());
            SaveMemberCommand = new Command(async () => await OnSaveMemberCommandExecuted());

            Title = "New Member";
            Member = new HouseholdMember();
        }

        private async Task OnPickImageCommandExecuted()
        {
            var takenImages = await ImagePickerService.Value.PickImage();
            var first = takenImages?.FirstOrDefault();
            await ApplySelectedImage(first);
        }

        private async Task OnTakePhotoCommandExecuted()
        {
            var takenPhoto = await ImagePickerService.Value.TakePhoto();
            await ApplySelectedImage(takenPhoto);
        }

        private async Task ApplySelectedImage(Stream inputImage)
        {
            if (inputImage == null)
                return;

            // copy as managed stream
            var imageCopy = new MemoryStream();
            await inputImage.CopyToAsync(imageCopy);
            imageCopy.Seek(0, SeekOrigin.Begin);
            MemberImageStream = imageCopy;
            OnMemberImageSelected?.Invoke();
        }

        private async Task OnSaveMemberCommandExecuted()
        {
            if (IsBusy || !IsMemberImageStreamDefined)
                return;

            try
            {
                IsBusy = true;
                OnPropertyChanged(nameof(CanSaveMember));

                var newMember = new PersonData()
                {
                    Name = Member.Text,
                    ImageStream = MemberImageStream,
                };

                await EyeSpyService.Value.AddTrustedPerson(newMember);

                MemberImageStream.Dispose();
                MemberImageStream = null;
                OnMemberImageSelected?.Invoke();
                MessagingCenter.Send(this, "AddMemberCompleted", Member);
                OnSaveMemberCommandCompleted?.Invoke();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(CanSaveMember));
            }
        }
    }
}
