using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Data;
using ProjFinalCinelAir.Prism.Models;
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
    
        private DelegateCommand _modifyUserCommand;
        private DelegateCommand _exitCommand;

        private readonly INavigationService _navigationService;
        private string _email;

        public ShowHistoryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "History Page";
           
        }

        public string Email { get => _email; set => SetProperty(ref _email, value);}

        


        public DelegateCommand ModifyUserCommand => _modifyUserCommand ?? (_modifyUserCommand = new DelegateCommand(Modify));

        public DelegateCommand ExitCommand => _exitCommand ?? (_exitCommand = new DelegateCommand(LogOut));


        private void LogOut()
        {
            //TODO: Pending
        }


        private async void Modify()
        {

           // Navigation.PushAsync(new NavigationPage(new nameof(ModifyUserPage)));
            // Passar isto
            NavigationParameters parameters = new NavigationParameters // Chave:valor
            {
               { "user", Email }
            };

            await _navigationService.NavigateAsync($"NavigationPage/{nameof(ModifyUserPage)}", parameters);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("user"))
            {
                Email = parameters.GetValue<string>("user");
                
              
            }
        }
    }
}
