using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Data;
using ProjFinalCinelAir.Prism.Helpers;
using ProjFinalCinelAir.Prism.Requests;
using ProjFinalCinelAir.Prism.Responses;
using ProjFinalCinelAir.Prism.Services;
using ProjFinalCinelAir.Prism.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isEnabled;
        private string _password;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        

        public LoginViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login Page";
            IsEnabled = true;


        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(ForgotPasswordAsync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void LoginAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Insert email", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Insert password", "Accept");

                return;
            }


            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet) // Vê se o tlm tem ligação à internet
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "ConnectionError", "Accept");
                return;
            }
            
            string url = App.Current.Resources["UrlAPI"].ToString();
           
            Response response = await _apiService.GetUserAsync(url, "api", $"/Account/GetUser?email={Email}");
            IsRunning = false;
            IsEnabled = true;

            


            if (!response.IsSuccess) // Caso algo tenha corrido mal
            {
                await App.Current.MainPage.DisplayAlert("Error", "LoginError", "Accept");
                Password = string.Empty;
                return;
            }

            User user = (User)response.Result;

            if (user == null)
            {
                await App.Current.MainPage.DisplayAlert("Erro", $"O utilizador não existe", "Accept");
                return;
            }
         

            Settings.IsLogin = true;

            User newUser = new User
            {
                identification = user.identification

            };

        

            IsRunning = false; // Desactivar os botões --> Depois das validações estarem realizadas
            IsEnabled = true;

            // Passar isto
            NavigationParameters parameters = new NavigationParameters // Chave:valor
            {
               { "user", Email }
            };

         

            await _navigationService.NavigateAsync($"NavigationPage/{nameof(ShowHistoryPage)}", parameters);

       
            Password = string.Empty;
            IsRunning = false;
            IsEnabled = true;
        }

        private void ForgotPasswordAsync()
        {
            //TODO: Pending
        }

        private void RegisterAsync()
        {
            //TODO: Pending
        }
    }
}
