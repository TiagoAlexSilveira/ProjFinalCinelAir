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
        private readonly IApiService _apiService;
        private ObservableCollection <Miles_Status> _milesStatus;
        private bool _isRunning;

        public MilesStatusViewModel(INavigationService navigationService, IApiService apiService) :base(navigationService)
        {
            Title = "MilesStatus";
            _apiService = apiService;
            LoadMilesStatusAsync();
        }

        // Binding para o syncfusion (load)
        public bool IsRunning { get => _isRunning; set => SetProperty(ref _isRunning, value); }

        public ObservableCollection<Miles_Status> Miles_Status  // Propriedade que vai fazer o binding com a view
        {
            get => _milesStatus;

            set => SetProperty(ref _milesStatus, value);

        }
        private async void LoadMilesStatusAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept); // Estamos a passar o recurso da linguagem
                return;
            }

            IsRunning = true; // Inicializar o load do syncfusion

            string url = App.Current.Resources["UrlAPI"].ToString(); // Está nos recursos
            Response response = await _apiService.GetListAsync<Miles_Status>(
                url,
                "/api",
                "/MilesStatus?id=1");

            IsRunning = false;  // Parar o load do syncfusion

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept); // Error e accept vai buscar os recursos

                return;
            }

            List<Miles_Status> Miles_StatusList = (List<Miles_Status>)response.Result;
            Miles_Status = new ObservableCollection<Miles_Status>(Miles_StatusList);



        }
    }
}
