using Prism;
using Prism.Ioc;
using ProjFinalCinelAir.Prism.ViewModels;
using ProjFinalCinelAir.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using ProjFinalCinelAir.Prism.Services;
using Syncfusion.Licensing;

namespace ProjFinalCinelAir.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzUwOTYxQDMxMzgyZTMzMmUzMGhYK0lzZzZMbDlCMlpXLyswNUdldDYwRVg0dktDQUNtQk0wVTZQNnVDTU09");
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MilesStatus, MilesStatusViewModel>();
            containerRegistry.RegisterForNavigation<TicketsPage, TicketsPageViewModel>();
            containerRegistry.RegisterForNavigation<MasterDetailPage, MasterDetailPageViewModel>();
        }
    }
}
