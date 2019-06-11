using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using ArcView.Services;

namespace ArcView.Droid.Services
{
    internal class SnackBarNotification : IInAppNotification
    {
        private readonly Snackbar _snackbar;
        internal SnackBarNotification(Snackbar snackbar)
        {
            _snackbar = snackbar;
        }

        public void Show() => _snackbar.Show();

        public void Dismiss() => _snackbar.Dismiss();

        public string Text
        {
            get => throw new NotImplementedException();
            set => _snackbar.SetText(value);
        }

        public int Duration
        {
            get => _snackbar.Duration;
            set => _snackbar.SetDuration(value);
        }

        public bool Visible => _snackbar.IsShown;
    }
}