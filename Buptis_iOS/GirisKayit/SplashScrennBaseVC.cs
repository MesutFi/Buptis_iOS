using Buptis_iOS.Database;
using Buptis_iOS.Web_Service;
using CoreLocation;
using Foundation;
using MapKit;
using System;
using System.Threading.Tasks;
using UIKit;

namespace Buptis_iOS
{
    public partial class SplashScrennBaseVC : UIViewController
    {
        MKMapView CustomMapView;
        CLLocationManager locationManager;
        bool Actinmi = false;
        public SplashScrennBaseVC (IntPtr handle) : base (handle)
        {
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            new DataBase();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            //if (!Actinmi)
            //{
                locationManager = new CLLocationManager();
                locationManager.RequestWhenInUseAuthorization();
                CustomMapView = new MKMapView() { ShowsUserLocation = true };
                UserLocationCheck();
                Actinmi = true;
            //}
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        bool? CheckLocationPermission()
        {
            var CLAuthorizationStatuss = CLLocationManager.Status;

            if (CLAuthorizationStatuss == CLAuthorizationStatus.NotDetermined ||
                CLAuthorizationStatuss == CLAuthorizationStatus.Restricted ||
                CLAuthorizationStatuss == CLAuthorizationStatus.Denied)
            {
                return false;
            }
            else if (CLAuthorizationStatuss == CLAuthorizationStatus.AuthorizedAlways ||
                     CLAuthorizationStatuss == CLAuthorizationStatus.AuthorizedWhenInUse)
            {
                return true;
            }
            else
            {
                return null;
            }
        }
        UIAlertView alert = null;
        void ShowAlertForPermission()
        {
            if (alert == null)
            {
                InvokeOnMainThread(delegate () {
                    alert = new UIAlertView();
                    alert.Title = "Buptis";
                    alert.AddButton("Tamam");
                    alert.Message = "Buptis'i kullanmaya devam edebilmeniz için konum hizmetlerine izin vermelisiniz.";
                    alert.AlertViewStyle = UIAlertViewStyle.Default;
                    alert.Clicked += (object s, UIButtonEventArgs ev) =>
                    {
                        SurekliKontrol();
                        UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                        alert.Dispose();
                    };
                    alert.Show();
                });
            }
        }

        void UserLocationCheck()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                Task.Run(async delegate () {
                TekrarCalistir:
                    CLLocation KontrolNesnesi = null;
                    InvokeOnMainThread(delegate () {
                        KontrolNesnesi = CustomMapView.UserLocation.Location;
                    });
                    if (KontrolNesnesi == null)
                    {
                        var durum = CheckLocationPermission();
                        if (durum != null)
                        {
                            if (!(bool)CheckLocationPermission())
                            {
                                ShowAlertForPermission();
                            }
                            else
                            {
                                await Task.Delay(500);
                                goto TekrarCalistir;
                            }
                        }
                        else
                        {
                            await Task.Delay(500);
                            goto TekrarCalistir;
                        }
                    }
                    else
                    {
                        UserLocationn.UserLoc = KontrolNesnesi;
                        var UserInfoVarmidur = DataBase.MEMBER_DATA_GETIR();
                        if (UserInfoVarmidur.Count > 0)
                        {
                            InvokeOnMainThread(delegate () {
                                Console.WriteLine("=====>>>" +CustomMapView.UserLocation.Location.Coordinate.Latitude.ToString());
                                var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
                                appDelegate.SetRootLokasyonlarViewController();
                            });
                        }
                        else
                        {
                            InvokeOnMainThread(delegate()
                            {
                                Console.WriteLine("=====>>>" + CustomMapView.UserLocation.Location.Coordinate.Latitude.ToString());
                                var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
                                appDelegate.SetRootSplashViewController();
                            });
                        }
                    }
                });
            })).Start();
        }
        System.Threading.Timer _timer;
        void SurekliKontrol()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                _timer = new System.Threading.Timer((o) =>
                {
                    var durum = CheckLocationPermission();
                    if (durum != null)
                    {
                        if ((bool)CheckLocationPermission())
                        {
                            Console.WriteLine("KONTROL => true");
                            _timer.Dispose();
                            _timer = null;
                            UserLocationCheck();
                        }
                        else
                        {
                            Console.WriteLine("KONTROL => false");
                        }
                    }
                }, null, 0, 1000);
            })).Start();
        }
    }

    public static class UserLocationn
    {
        public static CLLocation UserLoc { get; set; } = null;
    }
}