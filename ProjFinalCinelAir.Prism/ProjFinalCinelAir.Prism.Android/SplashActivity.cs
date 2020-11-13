using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;

namespace ProjFinalCinelAir.Prism.Droid
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            System.Threading.Thread.Sleep(1800);
            StartActivity(typeof(MainActivity));
        }


    }
}
