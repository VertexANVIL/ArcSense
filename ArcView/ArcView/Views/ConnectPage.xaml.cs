using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcView.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArcView.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage : ContentPage
    {
        private readonly ConnectViewModel _viewModel;

        public ConnectPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ConnectViewModel();
        }

        private void ConnectPage_OnAppearing(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                bool result;

                // Keep trying to connect indefinitely
                // with a pause of 5 seconds in between attempts

                do
                {
                    result = await _viewModel.Bluetooth.TryConnectAsync();
                    if (!result) await Task.Delay(TimeSpan.FromSeconds(5));
                } while (result == false);

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync(true);
                });
            }).ConfigureAwait(false);
        }
    }
}