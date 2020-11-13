using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Data;
using ProjFinalCinelAir.Prism.Responses;
using ProjFinalCinelAir.Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class TicketsPageViewModel : ViewModelBase
    {

        private readonly IApiService _apiService;
        private ObservableCollection<Ticket> _ticket;
        private bool _isRunning;

        public TicketsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Tickets";
            _apiService = apiService;
            LoadTicketsAsync();
        }

        // Binding para o syncfusion (load)
        public bool IsRunning { get => _isRunning; set => SetProperty(ref _isRunning, value); }

        public ObservableCollection<Ticket> Tickets  // Propriedade que vai fazer o binding com a view
        {
            get => _ticket;

            set => SetProperty(ref _ticket, value);

        }
        private async void LoadTicketsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection.", "Accept");
                return;
            }

            IsRunning = true; // Inicializar o load do syncfusion

            string url = App.Current.Resources["UrlAPI2"].ToString(); // Está nos recursos
            Response response = await _apiService.GetListAsync<Ticket>(
                url,
                "/ticket",
                "/2020-10-29");

            IsRunning = false;  // Parar o load do syncfusion

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            List<Ticket> TicketsList = (List<Ticket>)response.Result;
            Tickets = new ObservableCollection<Ticket>(TicketsList);



        }
    }
}
