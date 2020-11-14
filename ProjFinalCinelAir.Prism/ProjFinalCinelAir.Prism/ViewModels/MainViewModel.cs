using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private DelegateCommand _loginCommand;
        private readonly INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Main Page";


        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));

        private async void LoginAsync()
        {
            await _navigationService.NavigateAsync($"/NavigationPage/Login");
        }
    }

}
