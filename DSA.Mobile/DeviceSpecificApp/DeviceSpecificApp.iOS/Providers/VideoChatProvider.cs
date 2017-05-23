using System;
using System.Collections.Generic;
using System.Text;
using OpenTok;
using System.Drawing;
using DeviceSpecificApp.iOS.StreamingDelegate;
using UIKit;
using System.Diagnostics;
using DeviceSpecificApp.Model;

namespace DeviceSpecificApp.iOS.Providers
{
    public class VideoChatProvider
    {
        public OTSession _session;
        public OTPublisher _publisher;
        public OTSubscriber _subscriber;

        private SessionInfo session;

        public EventHandler OnHangup;
        public EventHandler<OnErrorEventArgs> OnError;

        public UIStackView PublisherView { get; set; }

        public UIStackView SubscriberView { get; set; }

        public bool SubscribeToSelf { get; set; }

        public VideoChatProvider()
        {
            this.OnHangup += (sender, e) =>
            {
                Debug.WriteLine("OnHangup: User tapped the hangup button.");
            };

            this.OnError += (sender, e) =>
            {
                Debug.WriteLine(e.Message);

                this.ShowAlert(e.Message);
            };
        }

        public void SetLayuots(UIStackView publisherLayout, UIStackView subscriberLayout)
        {
            this.PublisherView = publisherLayout;
            this.SubscriberView = subscriberLayout;
        }

        public void DoConnect()
        {
            OTError error;

            //var session = await App.NetworkProvider.GetSessionInfo();
            session = new SessionInfo()
            {
                ApiKey = 45842692,
                SessionId = "2_MX40NTg0MjY5Mn5-MTQ5NDkyNzQ5MjYyM35wcGMybmNyQjJqRDNLSUJzbURLc2JtTEt-UH4",
                Token = "T1==cGFydG5lcl9pZD00NTg0MjY5MiZzaWc9M2E2ZTE5YTU4YTAzMmNkNDUwODliNjI5ODlmNjExMjU3MDY4YzBlMTpzZXNzaW9uX2lkPTJfTVg0ME5UZzBNalk1TW41LU1UUTVORGt5TnpRNU1qWXlNMzV3Y0dNeWJtTnlRakpxUkROTFNVSnpiVVJMYzJKdFRFdC1VSDQmY3JlYXRlX3RpbWU9MTQ5NDkyNzYzNSZub25jZT0wLjM2NzI5MzU5MzE5MzUwNTk2JnJvbGU9cHVibGlzaGVyJmV4cGlyZV90aW1lPTE0OTQ5NDkyMzM="
            };

            _session = new OTSession(session.ApiKey.ToString(), session.SessionId, new SessionDelegate(this));

            _session.ConnectWithToken(session.Token, out error);

            if (error != null)
            {
                this.RaiseOnError(error.Description);
            }
        }

        public void DoDisconnect()
        {
            this.CleanupSubscriber();

            this.CleanupPublisher();

            if (_session != null)
            {

                if (_session.SessionConnectionStatus == OTSessionConnectionStatus.Connected)
                {

                    _session.Disconnect();
                }

                _session.Delegate = null;
                _session.Dispose();
                _session = null;
            }
        }

        /**
         * Sets up an instance of OTPublisher to use with this session. OTPubilsher
         * binds to the device camera and microphone, and will provide A/V streams
         * to the OpenTok session.
         */
        public void DoPublish()
        {
            _publisher = new OTPublisher(new PublisherDelegate(this), UIDevice.CurrentDevice.Name,OTCameraCaptureResolution.Medium,OTCameraCaptureFrameRate.OTCameraCaptureFrameRate30FPS);

            OTError error;

            _session.Publish(_publisher, out error);

            if (error != null)
            {
                this.RaiseOnError(error.Description);
            }

            _publisher.View.Frame = new RectangleF(0, 0, 100, 100);
            _publisher.View.Layer.CornerRadius = 50;
            _publisher.View.Layer.MasksToBounds = true;

            this.PublisherView.AddSubview(_publisher.View);
        }

        /**
         * Cleans up the publisher and its view. At this point, the publisher should not
         * be attached to the session any more.
         */
        public void CleanupPublisher()
        {
            if (_publisher != null)
            {
                _publisher.View.RemoveFromSuperview();
                _publisher.Delegate = null;
                _publisher.Dispose();
                _publisher = null;
            }
        }

        /**
         * Instantiates a subscriber for the given stream and asynchronously begins the
         * process to begin receiving A/V content for this stream. Unlike doPublish, 
         * this method does not add the subscriber to the view hierarchy. Instead, we 
         * add the subscriber only after it has connected and begins receiving data.
         */
        public void DoSubscribe(OTStream stream)
        {
            _subscriber = new OTSubscriber(stream, new SubscriberDelegate(this));

            OTError error;

            _session.Subscribe(_subscriber, out error);

            if (error != null)
            {
                this.RaiseOnError(error.Description);
            }
        }

        /**
         * Cleans the subscriber from the view hierarchy, if any.
         * NB: You do *not* have to call unsubscribe in your controller in response to
         * a streamDestroyed event. Any subscribers (or the publisher) for a stream will
         * be automatically removed from the session during cleanup of the stream.
         */
        public void CleanupSubscriber()
        {
            if (_subscriber != null)
            {
                _subscriber.View.RemoveFromSuperview();
                _subscriber.Delegate = null;
                _subscriber.Dispose();
                _subscriber = null;
            }
        }

        public void RaiseOnError(string message)
        {
            OnErrorEventArgs e = new OnErrorEventArgs(message);

            this.OnError(this, e);
        }

        private void ShowAlert(string message)
        {
            var alert = new UIAlertView("Alert", message, null, "Ok", null);

            alert.Show();
        }
    }
}
