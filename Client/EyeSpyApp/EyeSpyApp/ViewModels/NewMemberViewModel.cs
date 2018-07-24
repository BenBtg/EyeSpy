using System;
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

        public Stream MemberImageStream { get; set; }

        public Action OnSaveMemberCommandCompleted { get; set; }

        public ICommand SaveMemberCommand { get; } 

        public NewMemberViewModel()
        {
            SaveMemberCommand = new Command(async () => await OnSaveMemberCommandExecuted());

            Title = "New Member";
            Member = new HouseholdMember();
        }

        private async Task OnSaveMemberCommandExecuted()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var newMember = new PersonData()
                {
                    Name = Member.Text,
                    ImageStream = MemberImageStream,
                };

                await EyeSpyService.Value.AddTrustedPerson(newMember);

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
            }
        }
    }
}
