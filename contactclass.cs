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

namespace NewsAndroidApp
{
    public class contactclass
    {
        public string ID;
        public string txtNews;
       


        public contactclass(string ID, string txtNews)
        {
            this.ID = ID;
            this.txtNews = txtNews;
         //   this.mobileNumber = mobile;
        }
    }
}