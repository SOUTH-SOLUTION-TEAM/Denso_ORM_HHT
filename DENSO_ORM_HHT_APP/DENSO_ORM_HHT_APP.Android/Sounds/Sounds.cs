using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
[assembly: Xamarin.Forms.Dependency(typeof(DENSO_ORM_HHT_APP.Droid.Sounds.Sounds))]

namespace DENSO_ORM_HHT_APP.Droid.Sounds
{
    class Sounds : CommonClass.SoundPlay
    {
        private MediaPlayer _mediaPlayer;
        public void PlaySound(string MsgType)
        {
            if (MsgType == "Error")
            {
                _mediaPlayer = MediaPlayer.Create(Android.App.Application.Context, Resource.Drawable.ErrorSound);
                _mediaPlayer.Looping = true;
                _mediaPlayer.Start();
                var ctx = Android.OS.Vibrator.FromContext(Android.App.Application.Context);
                if (ctx.HasVibrator)
                {
                    ctx.Vibrate(50000);
                }

            }
            if (MsgType == "Success")
            {
                MediaPlayer _medialPlayer = MediaPlayer.Create(Android.App.Application.Context, Resource.Drawable.SuccessSound);
                _medialPlayer.Start();
                var ctx = Android.OS.Vibrator.FromContext(Android.App.Application.Context);
                if (ctx.HasVibrator)
                {
                    ctx.Vibrate(1000);
                }
            }
        }


        public void StopSound()
        {
            var ctx = Android.OS.Vibrator.FromContext(Android.App.Application.Context);

            ctx.Cancel();
            if(_mediaPlayer!=null)
            _mediaPlayer.Stop();
            // _medialPlayer.Stop();
        }
    }
}