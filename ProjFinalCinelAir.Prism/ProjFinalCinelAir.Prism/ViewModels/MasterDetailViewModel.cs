using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Models;
using ProjFinalCinelAir.Prism.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjFinalCinelAir.Prism.ViewModels
{
    public class MasterDetailViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
     

        public MasterDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
      
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }


        public string Email { get; set; }
       


        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
        {

            new Menu
            {
                Icon = "ic_history",
                PageName = $"{nameof(ShowHistoryPage)}",
                Title = "History",
                IsLoginRequired = true
            },
            new Menu
            {
                Icon = "ic_person",
                PageName = $"{nameof(ModifyUserPage)}",
                Title = "Modify user",
                IsLoginRequired = true
            },
            new Menu
            {
                Icon = "ic_exit_to_app",
                PageName = $"{nameof(Login)}",
                Title = "Logout"
            }
        };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title,
                    IsLoginRequired = m.IsLoginRequired
                }).ToList());
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
