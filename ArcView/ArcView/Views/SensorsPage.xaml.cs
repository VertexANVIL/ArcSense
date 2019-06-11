using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArcView.Models;
using ArcView.Views;
using ArcView.ViewModels;

namespace ArcView.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class SensorsPage : ContentPage
    {
        readonly SensorsViewModel _viewModel;

        public SensorsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new SensorsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            //var item = args.SelectedItem as Item;
            //if (item == null)
            //    return;

            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            //ItemsListView.SelectedItem = null;
        }
    }
}