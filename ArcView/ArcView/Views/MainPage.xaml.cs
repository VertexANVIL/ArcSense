using System;
using System.ComponentModel;
using ArcView.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArcView.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        readonly BaseViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new BaseViewModel();
        }
    }
}