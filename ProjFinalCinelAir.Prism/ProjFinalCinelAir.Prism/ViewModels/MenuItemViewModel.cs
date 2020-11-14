using Prism.Commands;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Models;
using ProjFinalCinelAir.Prism.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public DelegateCommand SelectMenuCommand =>
            _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));


        private async void SelectMenuAsync()
        {
            if (PageName=="Login")
            {
                await _navigationService.NavigateAsync($"/NavigationPage/{PageName}");
            }
            else
            {
                await _navigationService.NavigateAsync($"/{nameof(MasterDetail)}/NavigationPage/{PageName}");
            }
            
        }
    }
}
