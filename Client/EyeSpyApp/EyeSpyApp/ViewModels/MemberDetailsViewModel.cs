using System;
using EyeSpyApp.Models;

namespace EyeSpyApp.ViewModels
{
    public class MemberDetailsViewModel : BaseViewModel
    {
        public HouseholdMember Member { get; }

        public MemberDetailsViewModel(HouseholdMember member = null)
        {
            Title = member?.Text;
            Member = member;
        }
    }
}
