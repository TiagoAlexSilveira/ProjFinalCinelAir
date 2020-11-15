using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Data;
using ProjFinalCinelAir.Prism.Helpers;
using ProjFinalCinelAir.Prism.Responses;
using ProjFinalCinelAir.Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class MilesStatusViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private UserResponse _user;
        private string _milesStatus;
        private string _milesBonus;

        private string _firsttName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private string _fullName;


        public MilesStatusViewModel(INavigationService navigationService, IApiService apiService) :base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Miles Page";
            LoadUser();
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string MilesStatus
        {
            get => _milesStatus;
            set => SetProperty(ref _milesStatus, value);
        }

        public string MilesBonus
        {
            get => _milesBonus;
            set => SetProperty(ref _milesBonus, value);
        }

        public string FirstName
        {
            get => _firsttName;
            set => SetProperty(ref _firsttName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.userModel;
                MilesStatus = User.MilesStatus;
                MilesBonus = User.MilesBonus;
                FirstName = User.FirstName;
                Email = User.Email;
                PhoneNumber = User.PhoneNumber;
                FullName = $"{User.FirstName} {User.LastName}";


            }
        }
    }
}
