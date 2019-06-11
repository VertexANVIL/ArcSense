using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using ArcView.Services;
using Plugin.CurrentActivity;

namespace ArcView.Droid.Services
{
    internal class SnackBarNotifyService : IInAppNotifyService
    {
        public IInAppNotification Make(string value, int duration)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            var main = activity.FindViewById(Android.Resource.Id.Content);
            var obj = Snackbar.Make(main, value, duration);

            obj.View.SetBackgroundColor(Color.Red);

            return new SnackBarNotification(obj);
        }
    }
}