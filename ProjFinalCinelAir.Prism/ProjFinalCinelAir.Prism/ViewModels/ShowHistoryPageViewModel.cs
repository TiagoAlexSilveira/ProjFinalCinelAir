using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Data;
using ProjFinalCinelAir.Prism.Helpers;
using ProjFinalCinelAir.Prism.Models;
using ProjFinalCinelAir.Prism.Responses;
using ProjFinalCinelAir.Prism.Services;
using ProjFinalCinelAir.Prism.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class ShowHistoryPageViewModel :  ViewModelBase
    {
    
     

        private readonly INavigationService _navigationService;
        private UserResponse _user;
        private List<Transaction> _transactions;

        public ShowHistoryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "History Page";
            LoadUser();
        }

       

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public List<Transaction> Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.userModel;
                Transactions = User.Transactions;

            }
        }








    }
}
