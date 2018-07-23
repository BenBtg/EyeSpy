using System;
using EyeSpyApp.Models;

namespace EyeSpyApp.ViewModels
{
    public class NewMemberViewModel : BaseViewModel
    {
        public HouseholdMember Member { get; }

        public NewMemberViewModel()
        {
            Title = "New Member";
            Member = new HouseholdMember();
        }
    }
}
