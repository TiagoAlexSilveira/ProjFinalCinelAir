using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjFinalCinelAir.Prism.Helpers;
using ProjFinalCinelAir.Prism.ItemViewModels;
using ProjFinalCinelAir.Prism.Models;
using ProjFinalCinelAir.Prism.Responses;
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

        private UserResponse _user;

        public MasterDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
      
            _navigationService = navigationService;
            LoadMenus();
            LoadUser();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }


        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string UserName { get; set; }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.userModel;
                UserName = User.Email;
                
            }
        }

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
                Icon = "ic_more_vert",
                PageName = $"{nameof(MilesStatus)}",
                Title = "Miles",
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

    }
}
