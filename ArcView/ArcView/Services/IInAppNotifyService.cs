using System;
using System.Collections.Generic;
using System.Text;

namespace ArcView.Services
{
    /// <summary>
    /// Provides a way to send in-app notifications,
    /// such as using the Android Snackbar API.
    /// </summary>
    public interface IInAppNotifyService
    {
        IInAppNotification Make(string value, int duration);
    }
}
