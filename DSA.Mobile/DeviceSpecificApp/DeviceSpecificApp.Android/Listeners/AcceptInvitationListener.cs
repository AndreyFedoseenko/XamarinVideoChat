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
using System.Diagnostics;

namespace DeviceSpecificApp.Droid.Listeners
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class AcceptInvitationListener : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Debugger.Break();
        }
    }
}