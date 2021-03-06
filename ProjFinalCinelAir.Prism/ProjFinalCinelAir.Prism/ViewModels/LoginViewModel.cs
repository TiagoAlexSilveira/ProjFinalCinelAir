﻿using Newtonsoft.Json;
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

            TokenRequest request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            Response response = await _apiService.GetTokenAsync(url, "api", "/Account/CreateToken", request);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "LoginError", "Accept");
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync($"/{nameof(MasterDetail)}/NavigationPage/{nameof(ShowHistoryPage)}");
            Password = string.Empty;

          
        }

    
    }
}
