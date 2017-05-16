using System;
using System.ComponentModel;
using CoreGraphics;
using DeviceSpecificApp.iOS;
using UIKit;
using Xamarin.Forms;
using DeviceSpecificApp.iOS.Renderes;
using CoreAnimation;
using DeviceSpecificApp;
using DeviceSpecificApp.Controls;
using Xamarin.Forms.Platform.iOS;

namespace DeviceSpecificApp.iOS.Renderers
{
    public class VideoViewRenderer : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
            {
                return;
            }

            var stackLayout = (VideoLayout)this.Element;

            var appDelegate = AppDelegate.SelfDelegate;

            var publiserLayout = new UIStackView(new CGRect(0, 0, 200, 200));
            var subscriberLayout = new UIStackView(new CGRect(0, 0, 200, 200));

            stackLayout.Children.Add(publiserLayout);
            stackLayout.Children.Add(subscriberLayout);

            appDelegate.VideoProvider.SetLayuots(publiserLayout,subscriberLayout);
        }
    }
}
