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
using System.Threading.Tasks;
using System.Threading;

namespace NewsAndroidApp
{
    [Service]
    class MyServices : IntentService
    {
      public static  int id = 0;
        Boolean IsDone = false;
        protected override void OnHandleIntent(Intent intent)
        {

        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            newWS.wsnew sw = new newWS.wsnew();
            sw.NewsNotifyCompleted += Sw_NewsNotifyCompleted;
            // countine
            new Task(() =>
            {
                while (true)
                {
                    if (IsDone == false)
                    {
                        IsDone = true;
                        sw.NewsNotifyAsync(id);
                    }
                    
                    Thread.Sleep(10000);
                 


                }

            }).Start();

            return StartCommandResult.Sticky;
        }

        private void Sw_NewsNotifyCompleted(object sender, newWS.NewsNotifyCompletedEventArgs e)
        {
            if (!e.Result.ID.ToString().Equals("0"))
            {
                id = Convert.ToInt32(e.Result.ID.ToString());
                Intent intents = new Intent();
                intents.SetAction("com.alr.text");
                intents.PutExtra("MyData", "New news add: "+e.Result.Message);
                SendBroadcast(intents);
            }
            IsDone = false;
        }
    }
}