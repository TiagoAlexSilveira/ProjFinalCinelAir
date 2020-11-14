using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class MasterDetailPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;


        public MasterDetailPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {
            await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{PageName}");
        }

    }
}
