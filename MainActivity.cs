using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views;
using System;
using Newtonsoft.Json.Linq;
using Android.Content;

namespace NewsAndroidApp
{
    [Activity(Label = "NewsAndroidApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        EditText NewTXT;
        Button btn_Send;
        ListView LV;
        List<contactclass> contactItems;
        string jsonString;
       JArray jsonArray;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            NewTXT = FindViewById<EditText>(Resource.Id.editText1);
            btn_Send = FindViewById<Button>(Resource.Id.button1);

            LV = FindViewById<ListView>(Resource.Id.listView1);
            // LV.Adapter = new HomeScreenAdapter(this, contactItems);
            //   LV.ItemClick += OnListItemClick;  // to be defined
            contactItems = new List<contactclass>();
            newWS.wsnew webser = new newWS.wsnew();
            webser.GetJsontxtNewsAsync();
            webser.GetJsontxtNewsCompleted += Webser_GetJsontxtNewsCompleted;

            btn_Send.Click += delegate
            {
                newWS.wsnew webser2 = new newWS.wsnew();
                webser2.SendNewsAsync(NewTXT.Text);
                webser2.SendNewsCompleted += Webser2_SendNewsCompleted;
            };


            StartService(new Intent(this, typeof(MyServices)));
        }

        private void Webser2_SendNewsCompleted(object sender, newWS.SendNewsCompletedEventArgs e)
        {
            Toast.MakeText(this, "News was add sucsessful", ToastLength.Long).Show();
        }

        private void Webser_GetJsontxtNewsCompleted(object sender, newWS.GetJsontxtNewsCompletedEventArgs e)
        {
            //     throw new System.NotImplementedException();
            //throw new System.NotImplementedException();
            try
            {
                jsonString = e.Result.ToString();
                PopulateListView();

            }
            catch { Toast.MakeText(this, "Error Internt", ToastLength.Long).Show(); }
        }
        public class HomeScreenAdapter : BaseAdapter<contactclass>
        {
            List<contactclass> items;
            Activity context;
            public HomeScreenAdapter(Activity context, List<contactclass> items)
                : base()
            {
                this.context = context;
                this.items = items;
            }
            public override long GetItemId(int position)
            {
                return position;
            }
            public override contactclass this[int position]
            {
                get { return items[position]; }
            }
            public override int Count
            {
                get { return items.Count; }
            }
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var item = items[position];
                View view = convertView;
                if (view == null) // no view to re-use, create new
                    view = context.LayoutInflater.Inflate(Resource.Layout.CustonTicket, null);
            ///    view.FindViewById<TextView>(Resource.Id.Text).Text = item.txtNews;
                view.FindViewById<TextView>(Resource.Id.tvMobileNumber).Text = item.txtNews;
              //  view.FindViewById<TextView>(Resource.Id.tvMobileNumber).Text = item.mobileNumber;
                return view;
            }
        }

        private void PopulateListView()
        {
            if (jsonString == null)
            {
                //  tvresult.Text = "JsonString is Null";
                Toast.MakeText(this, "JsonString is Null", ToastLength.Long).Show();
            }
            else
            {
                try
                {
                    //  jsonobject = JObject.Parse(jsonString);

                    jsonArray = JArray.Parse(jsonString);
                    List<contactclass> contactItems = new List<contactclass>();
                    int count = 0;
                    while (count < jsonArray.Count)
                    {
                        contactclass contact = new contactclass(jsonArray[count]["ID"].ToString(), jsonArray[count]["txtNews"].ToString());
                        //      Toast.MakeText(this, jsonArray[count]["name"].ToString(), ToastLength.Long).Show();
                        contactItems.Add(contact);
                        count++;
                    }

                    //   lvcontact.Adapter = new ContactAdapter(this, contactItems);
                    LV.Adapter = new HomeScreenAdapter(this, contactItems);
                    LV.ItemClick += OnListItemClick;
                }
                catch (System.Exception exception)
                {

                    Toast.MakeText(this, "Some error"+exception, ToastLength.Long).Show();
                }


            }
        }
        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //var LV = sender as ListView;
            //var t = contactItems[e.Position];
            Toast.MakeText(this, "Hello", ToastLength.Long).Show();
        }
    }
}

