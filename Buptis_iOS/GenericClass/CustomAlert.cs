using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Buptis_iOS.GenericClass
{
     public static class CustomAlert
    {
        static UIViewController GetView;
        

        public static void GetCustomAlert(UIViewController GetView2, string alertMessage)
        {
            GetView = GetView2;
                GetView.InvokeOnMainThread(delegate ()
                {
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "Buptis",
                        Message = alertMessage
                    };
                    alert.AddButton("Tamam");
                    alert.Show();
                });
        }
    }
}