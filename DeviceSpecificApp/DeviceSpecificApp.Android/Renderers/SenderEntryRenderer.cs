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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using DeviceSpecificApp.Controls;
using Android.Views.InputMethods;
using DeviceSpecificApp.Droid.Renderers;

[assembly: ExportRenderer(typeof(SenderEntry), typeof(SenderEntryRenderer))]
namespace DeviceSpecificApp.Droid.Renderers
{
    public class SenderEntryRenderer : EntryRenderer
    {
        SenderEntry element;
        //MainActivity mainActivity;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
            {
                return;
            }

            element = (SenderEntry)this.Element;

            //mainActivity = Forms.Context as MainActivity;

            var textField = this.Control;
            textField.EditorAction += HandleOKButton;
        }

        private void HandleOKButton(object sender, TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == ImeAction.Done)
            {
                e.Handled = true;
                element.OnTextSended();
                element.Unfocus();
                //var inputManager = (InputMethodManager)mainActivity.GetSystemService(Context.InputMethodService);
                //inputManager.HideSoftInputFromWindow(((EditText)sender).WindowToken, HideSoftInputFlags.None);
            }
        }
    }
}