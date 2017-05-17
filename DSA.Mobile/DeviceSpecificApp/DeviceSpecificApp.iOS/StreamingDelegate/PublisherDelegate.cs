using DeviceSpecificApp.iOS.Providers;
using OpenTok;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DeviceSpecificApp.iOS.StreamingDelegate
{
    public class PublisherDelegate : OTPublisherKitDelegate
    {
        private VideoChatProvider provider;

        public PublisherDelegate(VideoChatProvider provider)
        {
            this.provider = provider;
        }

        public override void DidFailWithError(OTPublisher publisher, OTError error)
        {
            var msg = String.Format("PublisherDelegate:DidFailWithError: Error: {0}", error.Description);

            Debug.WriteLine(msg);

            provider.RaiseOnError(msg);

            provider.CleanupPublisher();
        }


        public override void StreamCreated(OTPublisher publisher, OTStream stream)
        {
            Debug.WriteLine("PublisherDelegate:StreamCreated: " + stream.StreamId);

            // If Subscribe To Self is true: Our own publisher is now visible to
            // all participants in the OpenTok session. We will attempt to subscribe to
            // our own stream. Expect to see a slight delay in the subscriber video and
            // an echo of the audio coming from the device microphone.
            if (provider._subscriber == null && this.provider.SubscribeToSelf)
            {
                provider.DoSubscribe(stream);
            }
        }
        public override void StreamDestroyed(OTPublisher publisher, OTStream stream)
        {
            Debug.WriteLine("PublisherDelegate:StreamDestroyed: " + stream.StreamId);

            provider.CleanupSubscriber();

            provider.CleanupPublisher();
        }
    }
}
