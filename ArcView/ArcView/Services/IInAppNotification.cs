using System;
using System.Collections.Generic;
using System.Text;

namespace ArcView.Services
{
    public interface IInAppNotification
    {
        void Show();
        void Dismiss();

        string Text { get; set; }
        int Duration { get; set; }
        bool Visible { get; }
    }
}
