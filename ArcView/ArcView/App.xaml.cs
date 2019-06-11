#undef TEST_DATA_MODE

using System;
using ArcDataCore.Analysis;
using ArcDataCore.Helpers;
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
        private readonly BluetoothNotifyService _bns;

        public App(IServiceCollection services)
        {
            InitializeComponent();

            //var services = new ServiceCollection();
            //s//ervices.AddSingleton(bl);
            //services.AddSingleton(no);

            services.AddSingleton<BluetoothNotifyService>();
            services.AddSingleton<BluetoothReciever>();

            // Add data processor stuff
            services.AddSingleton<DataIngestProcessor>();
            services.AddSingleton<ISensorDataRepository, SensorDataRepository>();
            services.AddVirtualSensors();

            // Build services and start threads
            Services = services.BuildServiceProvider();

            var processor = Services.GetRequiredService<DataIngestProcessor>();
            processor.DataIngested += data => MessagingCenter.Send(processor, "ingested");

#if TEST_DATA_MODE
            processor.Reciever = new TestReciever(); // TODO: TEST
#else
            processor.Reciever = Services.GetRequiredService<BluetoothReciever>();
#endif
            processor.Start();

            _bns = Services.GetRequiredService<BluetoothNotifyService>();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            _bns.TryConnect();
        }

        protected override void OnSleep()
        {
#if !TEST_DATA_MODE
            Services.GetRequiredService<IBluetoothService>().Disconnect();
#endif
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            // TODO: should do short check first
            //_bns.TryConnect();
        }
    }
}
