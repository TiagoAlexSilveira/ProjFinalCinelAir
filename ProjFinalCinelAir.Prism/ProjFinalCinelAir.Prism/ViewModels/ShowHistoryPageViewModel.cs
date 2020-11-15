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
    public class ShowHistoryPageViewModel :  ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        private bool _isRunning;
        private List<Client> _myClients;

        private ObservableCollection<ClientsItemViewModel> _clients;

        public ShowHistoryPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login Page";

            LoadClientsAsync();


        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public ObservableCollection<ClientsItemViewModel> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }



        private async void LoadClientsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Client>(
                url,
                "/api",
                "/Client");

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myClients = (List<Client>)response.Result;
            ShowClients();
        }

        private void ShowClients()
        {

            Clients = new ObservableCollection<ClientsItemViewModel>(_myClients.Select(p =>
            new ClientsItemViewModel(_navigationService)
            {
                FirstName = p.FirstName

            }).ToList());
            
           
        }

    }
}
