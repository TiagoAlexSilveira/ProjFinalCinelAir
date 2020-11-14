using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class ShowHistoryPageViewModel :  ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        public ShowHistoryPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login Page";
        }
    }
}
