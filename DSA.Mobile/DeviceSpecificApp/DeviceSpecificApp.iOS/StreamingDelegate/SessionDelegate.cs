//using OpenTok;
using DeviceSpecificApp.iOS.Providers;
using OpenTok;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DeviceSpecificApp.iOS.StreamingDelegate
{
    public class SessionDelegate : OTSessionDelegate
    {
        private VideoChatProvider provider;

        public SessionDelegate(VideoChatProvider provider)
        {
            this.provider = provider;
        }

        public override void DidConnect(OTSession session)
        {
            Debug.WriteLine("SessionDelegate:DidConnect: " + session.SessionId);

            // provider.DoPublish();
        }

        public override void DidFailWithError(OTSession session, OTError error)
        {
            var msg = "SessionDelegate:DidFailWithError: " + session.SessionId;

            Debug.WriteLine(msg);

            this.provider.RaiseOnError(msg);
        }

        public override void DidDisconnect(OTSession session)
        {
            var msg = "SessionDelegate:DidDisconnect: " + session.SessionId;

            Debug.WriteLine(msg);
        }

        public override void ConnectionCreated(OTSession session, OTConnection connection)
        {
            Debug.WriteLine("SessionDelegate:ConnectionCreated: " + connection.ConnectionId);
        }

        public override void ConnectionDestroyed(OTSession session, OTConnection connection)
        {
            Debug.WriteLine("SessionDelegate:ConnectionDestroyed: " + connection.ConnectionId);

            this.provider.CleanupSubscriber();
        }

        public override void StreamCreated(OTSession session, OTStream stream)
        {
            Debug.WriteLine("SessionDelegate:StreamCreated: " + stream.StreamId);

            if (this.provider._subscriber == null && !this.provider.SubscribeToSelf)
            {
                this.provider.DoSubscribe(stream);
            }
        }

        public override void StreamDestroyed(OTSession session, OTStream stream)
        {
            Debug.WriteLine("SessionDelegate:StreamDestroyed: " + stream.StreamId);

            this.provider.CleanupSubscriber();
        }
    }
}
