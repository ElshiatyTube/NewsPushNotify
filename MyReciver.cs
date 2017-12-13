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
using Android.Telephony;
using Android.Provider;
using Newtonsoft.Json.Linq;
using Android.Media;

namespace NewsAndroidApp
{
    [BroadcastReceiver(Exported = true, Label = "SMS Receiver")]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED", "com.alr.text" })]
    public class MyReciver : Android.Content.BroadcastReceiver
    {
       
        JArray jsonArray;
        string text;
        private const string Tag = "SMSBroadcastReceiver";
        private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";
        string jsonString;

        public override void OnReceive(Context context, Intent intent)
        {
            // Log.Info(Tag, "Intent received: " + intent.Action);
            // read the SendBroadcast data
            if (intent.Action == "com.alr.text")
            {
            //    string text = intent.GetStringExtra("MyData") ?? "Data not available";
           //     Toast.MakeText(context, text, ToastLength.Short).Show();


                //Intent intents = new Intent(context, typeof(MainActivity));
                //intents.AddFlags(ActivityFlags.NewTask);
                //context.StartActivity(intents);

                text = intent.GetStringExtra("MyData") ?? "Data not available";
                    Toast.MakeText(context, text, ToastLength.Short).Show();

                noti(context);

            }
            //read incomming sms
            //if (intent.Action == IntentAction)
            //{

            //    SmsMessage[] messages = Telephony.Sms.Intents.GetMessagesFromIntent(intent);

            //    var sb = new StringBuilder();

            //    for (var i = 0; i < messages.Length; i++)
            //    {

            //        sb.Append(string.Format("SMS From: {0}{1}Body: {2}{1}", messages[i].OriginatingAddress,
            //            System.Environment.NewLine, messages[i].MessageBody));
            //    }
            //    Toast.MakeText(context, sb.ToString(), ToastLength.Short).Show();
            //}
        }
        public void noti(Context cont)
        {

            Notification.Builder builder = new Notification.Builder(cont);
            builder.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification));
            ////.SetContentIntent(pendingIntent)
            builder.SetContentTitle("New Notify");
            builder.SetContentText(text);
            builder.SetSmallIcon(Resource.Drawable.BoketIcon);
            Notification notification = builder.Build();
            NotificationManager notificationManager = cont.GetSystemService(Context.NotificationService) as NotificationManager;

            //newWS.wsnew sw = new newWS.wsnew();

            //if (jsonString == null)
            //{
            //    //  tvresult.Text = "JsonString is Null";
            //    Toast.MakeText(cont, "JsonString is Null", ToastLength.Long).Show();
            //}
            //else
            //{
            //    jsonArray = JArray.Parse(jsonString);
            //    //    List<contactclass> contactItems = new List<contactclass>();
            //    int count = 0;
            //    while (count < jsonArray.Count)
            //    {
            //        contactclass contact = new contactclass(jsonArray[count]["ID"].ToString(), jsonArray[count]["txtNews"].ToString());
            //        //      Toast.MakeText(this, jsonArray[count]["name"].ToString(), ToastLength.Long).Show();
            //        //    contactItems.Add(contact);
            //        //   count++;
            //        notificationManager.Notify(count++, notification);
            //    }
            //    //  sw.NewsNotifyAsync(count)
            //}


          notificationManager.Notify(MyServices.id, notification);

        }
    }
}