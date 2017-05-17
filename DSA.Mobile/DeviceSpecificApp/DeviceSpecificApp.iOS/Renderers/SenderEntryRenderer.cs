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

[assembly: ExportRenderer(typeof(SenderEntry), typeof(SenderEntryRenderer))]
namespace DeviceSpecificApp.iOS.Renderes
{
    public class SenderEntryRenderer : EntryRenderer
    {
        SenderEntry element;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
            {
                return;
            }

            element = (SenderEntry)this.Element;

            var textField = this.Control;

            textField.ShouldReturn += TextFieldShouldReturn;
        }

        private bool TextFieldShouldReturn(UITextField tf)
        {
            element.OnTextSended();
            return true;
        }
    }
}
