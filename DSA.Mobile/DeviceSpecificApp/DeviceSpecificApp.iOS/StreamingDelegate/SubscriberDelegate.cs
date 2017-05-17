using DeviceSpecificApp.iOS.Providers;
using OpenTok;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace DeviceSpecificApp.iOS.StreamingDelegate
{
    public class SubscriberDelegate : OTSubscriberKitDelegate
    {
        private VideoChatProvider provider;

        public SubscriberDelegate(VideoChatProvider provider)
        {
            this.provider = provider;
        }

        public override void DidConnectToStream(OTSubscriber subscriber)
        {
            Debug.WriteLine("SubscriberDelegate:DidConnectToStream: " + subscriber.Stream.StreamId);

            //this.provider._subscriber.View.Frame = new RectangleF(0, 0, _this.Frame.Width, _this.Frame.Height);
            this.provider._subscriber.View.Frame = new RectangleF(0, 0, 200, 200);

            this.provider.SubscriberView.AddSubview(this.provider._subscriber.View);
        }

        public override void DidFailWithError(OTSubscriber subscriber, OTError error)
        {
            var msg = String.Format("SubscriberDelegate:DidFailWithError: Stream {0}, Error: {1}", subscriber.Stream.StreamId, error.Description);

            Debug.WriteLine(msg);

            this.provider.RaiseOnError(msg);
        }
    }
}
