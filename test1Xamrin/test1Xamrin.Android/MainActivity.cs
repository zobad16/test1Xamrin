using System;
using Android.Views;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;

namespace test1Xamrin.Droid
{
    [Activity(Label = "FatturareOnline Self", Icon = "@drawable/icon", Theme = "@style/MySplash", LaunchMode = LaunchMode.SingleInstance,
          MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnApplyThemeResource(global::Android.Content.Res.Resources.Theme theme, int resId, bool first)
        {
            //https://forums.xamarin.com/discussion/18946/no-title-bar-on-android
            base.OnApplyThemeResource(theme, Resource.Style.MyTheme, first);
            //this.ActionBar.SetIcon(global::Android.Resource.Color.Transparent); 
            //nota: la linea di cui sopra produce l'errore requestFeature() must be called before adding content nelle versione di android <=6
            //nota: risolto mettendo <item name="android:icon">@android:color/transparent</item> in styles.xml
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            LoadApplication(new App());

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [Obsolete]
        public override void OnBackPressed()
        {
            //possibile modo per decidere: occorre capire quando una form è aperta in modo modale
            //var md = Xamarin.Forms.Application.Current.MainPage as MasterDetailPage;

            //if (md != null && !md.IsPresented &&
            //  (
            //  !(md.Detail is NavigationPage) || (((NavigationPage)md.Detail).Navigation.NavigationStack.Count == 1 && ((NavigationPage)md.Detail).Navigation.ModalStack.Count == 0)
            //  ))

            //  MoveTaskToBack(true);
            //else
            //  base.OnBackPressed();


            //base.OnBackPressed();
        }
    }
}