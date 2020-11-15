using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _email;

        public ModifyUserPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Modify Account Information";
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
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
