
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Buptis_iOS.GenericClass;
using Buptis_iOS.LokasyondakiKisiler;
using Buptis_iOS.Lokasyonlar;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using Newtonsoft.Json;
using UIKit;
using Xamarin.Essentials;

namespace Buptis_iOS.LokasyonDetay
{
    public partial class LokasyonDetayBaseVC : UIViewController
    {
        #region Tanimlamalar 
        MapView mapView;
        public Mekanlar_Location  GelenMekan;
        #endregion
        public LokasyonDetayBaseVC(IntPtr handle) : base(handle)
        {
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            CallButton.TouchUpInside += delegate {
                var url = new NSUrl("tel:" + GelenMekan.telephone);
                UIApplication.SharedApplication.OpenUrl(url);
            };
         
            RatingButton.SetTitle(Math.Round(Convert.ToDouble(GelenMekan.rating), 1).ToString(),UIControlState.Normal);
            mekanTitle.Text= GelenMekan.name;
            KonumButton.TouchUpInside += KonumButton_TouchUpInside;
            BekletmeButton.TouchUpInside += BekletmeButton_TouchUpInside;
            CheckinButton.TouchUpInside += CheckinButton_TouchUpInside;
            RatingButton.TouchUpInside += RatingButton_TouchUpInside;
            MekandakilerButton.TouchUpInside += MekandakilerButton_TouchUpInside;
            BackButton.TouchUpInside += BackButton_TouchUpInside;
        }

        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.View.BackgroundColor = UIColor.Clear;
            Task.Run(delegate () {
                InvokeOnMainThread(delegate ()
                {
                    this.DismissViewController(true, null);
                });
            });
        }
        bool Actinmi = false;
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (!Actinmi)
            {
                Tasarim();
                BackButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
                MessageButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
                Actinmi = true;
            }
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            GetMap(Mapview);
        }
        private void RatingButton_TouchUpInside(object sender, EventArgs e)
        {
           
            var LokasyonDetayStory = UIStoryboard.FromName("LokasyonDetayBaseVC", NSBundle.MainBundle);
            RatingVC controller = LokasyonDetayStory.InstantiateViewController("RatingVC") as RatingVC;
            controller.gelenMekan = GelenMekan;
            this.PresentViewController(controller, true, null);
        }

        private void MekandakilerButton_TouchUpInside(object sender, EventArgs e)
        {
            var LokasyonKisilerStory = UIStoryboard.FromName("LokasyondakiKisilerBaseVC", NSBundle.MainBundle);
            LokasyondakiKisilerBaseVC controller = LokasyonKisilerStory.InstantiateViewController("LokasyondakiKisilerBaseVC") as LokasyondakiKisilerBaseVC;
            controller.gelenMekan = GelenMekan;
            MesajAtabilmekIcinSecilenSonLokasyon.TiklananMekan = GelenMekan;
            this.PresentViewController(controller, true, null);
        }

        private void CheckinButton_TouchUpInside(object sender, EventArgs e)
        {
            CheckInYap("ONLINE", "Check-in Yapılıyor...", "Check-in Yapıldı...");
        }

        private void BekletmeButton_TouchUpInside(object sender, EventArgs e)
        {
            CheckInYap("WAITING", "Check-in Bekletme Yapılıyor...", "Check-in Bekletme Yapıldı...");
        }

        private void KonumButton_TouchUpInside(object sender, EventArgs e)
        {
            string url = "http://maps.apple.com/?saddr=" + GelenMekan.coordinateX+","+ GelenMekan.coordinateY;
            if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl(url)))
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }
            else
            {
                new UIAlertView("Hata", "Harita Bu Cihazda Desteklenmiyor", null, "Tamam").Show();
            }
        }

        #endregion

        #region Check - in 
        void CheckInYap(string statuss, string startprogresstext, string alert)
        {
            CustomLoading.Show(this, startprogresstext);
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();

                CheckInIslemiIcinDataModel checkInIslemiIcinDataModel = new CheckInIslemiIcinDataModel()
                {
                    locationId = Convert.ToInt32(GelenMekan.id),
                    status = statuss
                };
                var jsonstring = JsonConvert.SerializeObject(checkInIslemiIcinDataModel);
                var Donus = webService.ServisIslem("locations/check-in", jsonstring);
                if (Donus != "Hata")
                {
                    InvokeOnMainThread(delegate ()
                    {
                        CustomAlert.GetCustomAlert(this, alert);
                        CustomLoading.Hide();
                        var LokasyonKisilerStory = UIStoryboard.FromName("LokasyondakiKisilerBaseVC", NSBundle.MainBundle);
                        LokasyondakiKisilerBaseVC controller = LokasyonKisilerStory.InstantiateViewController("LokasyondakiKisilerBaseVC") as LokasyondakiKisilerBaseVC;
                        controller.gelenMekan = GelenMekan;
                        this.PresentViewController(controller, true, null);
                    });
                }
                else
                {
                    InvokeOnMainThread(delegate ()
                    {
                        CustomAlert.GetCustomAlert(this, "Bir sorun oluştu...");
                        CustomLoading.Hide();
                    });
                }
            })).Start();
        }
        #endregion

        #region Map
        public void GetMap(UIView _mapView)
        {
            CameraPosition camera = CameraPosition.FromCamera(new CLLocationCoordinate2D(GelenMekan.coordinateX, GelenMekan.coordinateY),6);

            mapView = MapView.FromCamera(CGRect.Empty, camera);
            mapView.MyLocationEnabled = true;


            mapView.MapStyle = MapStyle.FromJson(ReadFile(), null);
            mapView.Frame = _mapView.Bounds;
            _mapView.AddSubview(mapView);

            var xamMarker = new Marker()
                {
                    Title = "",
                    Snippet = "",
                    Position = new CLLocationCoordinate2D(GelenMekan.coordinateX, GelenMekan.coordinateY),
                    Map = mapView
                };
            mapView.SelectedMarker = xamMarker;
        }
        private string ReadFile()
        {
            using (StreamReader reader = new StreamReader("MapStyle/mapstyle1.txt"))
            {
                return reader.ReadToEnd();
            }

        }
        #endregion

        #region UITasarim
        public void Tasarim()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 126f);
            HeaderView.Layer.InsertSublayer(gradientLayer, 0);
            HeaderView.Layer.CornerRadius = 30;
            HeaderView.ClipsToBounds = true;
            HeaderView.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);

            MekandakilerButton.BackgroundColor = UIColor.FromRGB(225, 0, 105);
            MekandakilerButton.Layer.CornerRadius = 15f;
            MekandakilerButton.ClipsToBounds = true;

            BekletmeButton.Layer.CornerRadius = BekletmeButton.Frame.Height / 2;
            BekletmeButton.Layer.BorderColor = UIColor.White.CGColor;
            BekletmeButton.Layer.BorderWidth = 5f;
            BekletmeButton.ClipsToBounds = true;
            BekletmeButton.ContentEdgeInsets = new UIEdgeInsets(15, 15, 15, 15);

            CheckinButton.Layer.CornerRadius = CheckinButton.Frame.Height / 2;
            CheckinButton.Layer.BorderColor = UIColor.White.CGColor;
            CheckinButton.Layer.BorderWidth = 5f;
            CheckinButton.ClipsToBounds = true;
            CheckinButton.ContentEdgeInsets = new UIEdgeInsets(15, 15, 15, 15);

            Mapview.Layer.CornerRadius = 30f;
            Mapview.ClipsToBounds = true;
        }



        #endregion

        public class CheckInIslemiIcinDataModel
        {
            public int locationId { get; set; }
            public string status { get; set; }
        }
    }
    public static class MesajAtabilmekIcinSecilenSonLokasyon
    {
        public static Mekanlar_Location TiklananMekan { get; set; }
    }
}