using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using ArcView.Droid.Services;
using Xamarin.Forms.Platform.Android;

namespace ArcView.Droid
{
    [Activity(Label = "ArcView", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();

            //if (ContextCompat.CheckSelfPermission(ApplicationContext, Android.Manifest.Permission.AccessFineLocation) != Permission.Granted)
            //    ActivityCompat.RequestPermissions(this, new[] { Android.Manifest.Permission.AccessFineLocation }, 0);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            ////if (savedInstanceState == null)
            //{
            //    BluetoothLowEnergyAdapter.Init(this);
            //}

            var bluetooth = new BluetoothService();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            //Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(bluetooth));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}