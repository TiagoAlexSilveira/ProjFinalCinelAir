using Prism.Commands;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Data;
using ProjFinalCinelAir.Prism.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class ClientsItemViewModel : Client
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectClientCommand;

        public ClientsItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectClientCommand => _selectClientCommand ??
            (_selectClientCommand = new DelegateCommand(SelectClientAsync));

        private async void SelectClientAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
               { "client", this }
            };

            //Redirecionar para detalhes do client, não implementado
            await _navigationService.NavigateAsync(nameof(ModifyUserPage), parameters);
        }

    }
}
