using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using EyeSpyApp.Models;
using EyeSpyApp.Views;
using Xamarin.Forms;

namespace EyeSpyApp.ViewModels
{
    public class HouseholdViewModel : BaseViewModel
    {
        public ObservableCollection<HouseholdMember> Members { get; set; }
        public Command LoadMembersCommand { get; set; }

        public HouseholdViewModel()
        {
            Title = "Household";
            Members = new ObservableCollection<HouseholdMember>();
            LoadMembersCommand = new Command(async () => await ExecuteLoadMembersCommand());

            MessagingCenter.Subscribe<NewMemberViewModel, HouseholdMember>(this, "AddMember", async (obj, item) =>
            {
                var _item = item as HouseholdMember;
                Members.Add(_item);
                await DataStore.AddItemAsync(_item);
            });
        }

        private async Task ExecuteLoadMembersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Members.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Members.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}