using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ArcView.Services;
using ArcView.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ArcView
{
    public partial class App : Application
    {
        internal static IServiceProvider Services;
        private readonly IBluetoothService _bluetooth;

        public App(IBluetoothService bluetooth)
        {
            InitializeComponent();
            _bluetooth = bluetooth;

            var services = new ServiceCollection();
            services.AddSingleton(bluetooth);
            services.AddSingleton(new BluetoothClient(bluetooth));

            Services = services.BuildServiceProvider();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            MainPage.Navigation.PushModalAsync(new ConnectPage(), true);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            _bluetooth.Disconnect();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            // TODO: should do short check first
            MainPage.Navigation.PushModalAsync(new ConnectPage(), true);
        }
    }
}
