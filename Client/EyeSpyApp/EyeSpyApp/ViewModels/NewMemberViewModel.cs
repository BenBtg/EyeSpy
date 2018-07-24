using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using EyeSpyApp.Models;

namespace EyeSpyApp.ViewModels
{
    public class NewMemberViewModel : BaseViewModel
    {
        public HouseholdMember Member { get; }

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

                //TODO: submit to the backend
                await Task.Delay(2000);

                //notify other view models to udpate state
                MessagingCenter.Send(this, "AddMember", Member);
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
