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
            //CrossMediaManager.Current.PlayingChanged += (sender, e) =>
            //{
            //    ProgressBar.Progress = e.Progress;
            //    Duration.Text = "" + e.Duration.TotalSeconds.ToString() + " seconds";
            //};
            //networkProvider  = new NetworkProvider();
            // var session = App.QbProvider.GetBaseSession();

        }

        //async void PlayClicked(object sender, System.EventArgs e)
        //{
        //    PlaybackController.Play();
        //    await networkProvider.GetSessionInfo();
        //    // fVideo.Source += "http://learnmusic.pbworks.com/w/file/fetch/69571495/Video%20Sample%201%20%28Small%29.m4v";
        //}

        //void PauseClicked(object sender, System.EventArgs e)
        //{
        //    PlaybackController.Pause();
        //}

        //void StopClicked(object sender, System.EventArgs e)
        //{
        //    PlaybackController.Stop();
        //}

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
