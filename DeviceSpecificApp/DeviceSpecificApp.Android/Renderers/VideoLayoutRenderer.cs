using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using DeviceSpecificApp.Controls;
using Android.Views.InputMethods;
using DeviceSpecificApp.Droid.Renderers;

[assembly: ExportRenderer(typeof(VideoLayout), typeof(VideoLayoutRenderer))]
namespace DeviceSpecificApp.Droid.Renderers
{
    public class VideoLayoutRenderer : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
            {
                return;
            }

            var stackLayout = (VideoLayout)this.Element;

            //mainActivity = Forms.Context as MainActivity;

            // var stackLayout = this.Control;

            var mainActivity = Forms.Context as MainActivity;

            var publiserLayout = new Android.Widget.LinearLayout(mainActivity)
            {
                Orientation = Orientation.Vertical,
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 200)
            };
            var subscriberLayout = new Android.Widget.LinearLayout(mainActivity)
            {
                Orientation = Orientation.Vertical,
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 200)
            };
            var loadingProggressBar = new Android.Widget.ProgressBar(mainActivity);
            loadingProggressBar.Visibility = ViewStates.Gone;
            subscriberLayout.AddView(loadingProggressBar);

            stackLayout.Children.Add(publiserLayout);
            stackLayout.Children.Add(subscriberLayout);

            mainActivity.SetLayouts(publiserLayout, subscriberLayout, loadingProggressBar);
        }
    }
}