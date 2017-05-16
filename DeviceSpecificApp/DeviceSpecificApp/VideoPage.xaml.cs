using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DeviceSpecificApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPage : ContentPage
    {
        private IPlaybackController PlaybackController => CrossMediaManager.Current.PlaybackController;

        private NetworkProvider networkProvider;

        private IDealer dealer;

        public VideoPage()
        {
            InitializeComponent();
            dealer= DependencyService.Get<IDealer>();
            dealer.ConnectToSession();

        }

        private void CallSomeone(object sender, EventArgs e)
        {
            dealer.VideoCallSomeone();
        }

        private void RejectCall(object sender, EventArgs e)
        {
            dealer.RejectVideoCall();
        }
    }
}
