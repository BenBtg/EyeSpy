﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EyeSpy.Shared;
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

            MessagingCenter.Subscribe<NewMemberViewModel, HouseholdMember>(this, "AddMemberCompleted", (obj, item) =>
            {
                var _item = (HouseholdMember)item;
                Members.Add(_item);
            });
        }

        private async Task ExecuteLoadMembersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var trustedPersons = await EyeSpyService.Value.GetTrustedPersons();

                Members.Clear();
                trustedPersons?
                    .Select(tp => new HouseholdMember { Id = tp.Id, Text = tp.Name, ImageUrl = tp.ProfileUrl })
                    .ToList()
                    .ForEach(Members.Add);
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