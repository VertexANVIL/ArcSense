#define TEST_DATA_MODE

using System;
using ArcDataCore.Analysis;
using ArcDataCore.TxRx;
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

            // Add data processor stuff
            services.AddSingleton<DataIngestProcessor>();

            // Build services and start threads
            Services = services.BuildServiceProvider();
            var processor = Services.GetRequiredService<DataIngestProcessor>();
            processor.Reciever = new TestReciever(); // TODO: TEST
            processor.Start();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
#if !TEST_DATA_MODE
            if (MainPage.Navigation.ModalStack.Count == 0)
                MainPage.Navigation.PushModalAsync(new ConnectPage(), true);
#endif
        }

        protected override void OnSleep()
        {
#if !TEST_DATA_MODE
            // Handle when your app sleeps
            _bluetooth.Disconnect();
#endif
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            // TODO: should do short check first
#if !TEST_DATA_MODE
            if (MainPage.Navigation.ModalStack.Count == 0)
                MainPage.Navigation.PushModalAsync(new ConnectPage(), true);
#endif
        }
    }
}
