using Buptis_iOS.GenericClass;
using Buptis_iOS.LokasyonDetay;
using Buptis_iOS.Lokasyonlar;
using Buptis_iOS.Web_Service;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;

namespace Buptis_iOS
{
    public partial class LokasyonlarBiryersec : UIView
    {
        LokasyonlarTableCell[] Noktalar = new LokasyonlarTableCell[0];
        List<Mekanlar_Location> mekanlar_list = new List<Mekanlar_Location>();
        LokasyonlarBaseVC GelenBase1;
        MapView mapView;
        List<Marker> OlusanMarkerlar = new List<Marker>();

        public LokasyonlarBiryersec(IntPtr handle) : base(handle)
        {
        }
        public static LokasyonlarBiryersec Create(LokasyonlarBaseVC GelenBase2)
        {
            var arr = NSBundle.MainBundle.LoadNib("LokasyonlarBiryersec", null, null);
            var v = Runtime.GetNSObject<LokasyonlarBiryersec>(arr.ValueAt(0));
            v.BackgroundColor = UIColor.Clear;
            v.GelenBase1 = GelenBase2;
            return v;
            
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CustomLoading.Show(GelenBase1, "Lokasyonlar Yükleniyor...");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                BanaYakinLokasyonlariGetir();

            })).Start();

        }
        void BanaYakinLokasyonlariGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("locations");
            if (Donus != null)
            {
                var aa = Donus.ToString();
                mekanlar_list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Mekanlar_Location>>(Donus.ToString());
                if (mekanlar_list.Count > 0)
                {
                    InvokeOnMainThread(delegate ()
                    {
                        if (mekanlar_list.Count>0)
                        {
                            GetMap(Mapview);
                        }

                        for (int i = 0; i < mekanlar_list.Count; i++)
                        {

                            var xamMarker = new Marker()
                            {
                                Title = "",
                                Snippet = "",
                                Position = new CLLocationCoordinate2D(mekanlar_list[i].coordinateX, mekanlar_list[i].coordinateY),
                                Map = mapView,
                                Icon =  UIImage.FromBundle("Images/mapmarkerrr.png"),
                            };
                            OlusanMarkerlar.Add(xamMarker);
                        }
                        if (mekanlar_list.Count > 0)
                        {
                            mapView.SelectedMarker = OlusanMarkerlar[0];
                            var newCamera = CameraPosition.FromCamera(OlusanMarkerlar[0].Position, 10, mapView.Camera.Bearing + 10, mapView.Camera.ViewingAngle + 10);
                            mapView.Animate(newCamera);
                            this.mapView.TappedMarker = (map, marker) =>
                            {
                                var id = OlusanMarkerlar.FindIndex(item => item == marker);
                                try
                                {
                                    var IndexConvert = Convert.ToInt32(id);
                                    Scrolll.SetContentOffset(new CGPoint(Noktalar[IndexConvert].Frame.X, 0), true);
                                    var PageeIndex = (nint)(Scrolll.ContentOffset.X / Scrolll.Frame.Width);
                                    Console.WriteLine("OK");
                                }
                                catch
                                {
                                }
                                return false;
                            };
                        }
                        CreateScrollViews();
                        Scrolll.PagingEnabled = true;
                        CustomLoading.Hide();
                    });
                }
                else
                {
                    CustomAlert.GetCustomAlert(GelenBase1, "Çevrenizde hiç lokasyon bulunamadý...");
                    CustomLoading.Hide();
                }
            }
            else
            {
                CustomLoading.Hide();
            }
        }
       
        void CreateScrollViews()
        {
            Noktalar = new LokasyonlarTableCell[mekanlar_list.Count];
            for (int i = 0; i < mekanlar_list.Count; i++)
            {
                var NoktaItem = LokasyonlarTableCell.Create();
                NoktaItem.UpdateCell(mekanlar_list[i]);
                if (i == 0)
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, 144f);
                    NoktaItem.UserInteractionEnabled = true;
                    NoktaItem.Tag = i;
                    Action Actionn = () => MekanClick(NoktaItem);
                    UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(Actionn);
                    NoktaItem.AddGestureRecognizer(tapGesture);
                }
                else
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(UIKit.UIScreen.MainScreen.Bounds.Width * i, 0, UIKit.UIScreen.MainScreen.Bounds.Width, 144f);
                    NoktaItem.UserInteractionEnabled = true;
                    NoktaItem.Tag = i;
                    Action Actionn = () => MekanClick(NoktaItem);
                    UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(Actionn);
                    NoktaItem.AddGestureRecognizer(tapGesture);
                }

                Scrolll.AddSubview(NoktaItem);
                Noktalar[i] = NoktaItem;
            }

            Scrolll.ContentSize = new CoreGraphics.CGSize(Noktalar[Noktalar.Length - 1].Frame.Right, 144f);
            Scrolll.Scrolled += Scrolll_Scrolled;
        }
        void MekanClick(UIView GlenView)
        {
            try
            {
                var LokasyonDetayStory = UIStoryboard.FromName("LokasyonDetayBaseVC", NSBundle.MainBundle);
                LokasyonDetayBaseVC controller = LokasyonDetayStory.InstantiateViewController("LokasyonDetayBaseVC") as LokasyonDetayBaseVC;
                controller.GelenMekan = mekanlar_list[(int)GlenView.Tag];
                controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                GelenBase1.PresentViewController(controller, true, null);
            }
            catch 
            {
            }
           
        }
        private void Scrolll_Scrolled(object sender, EventArgs e)
        {
            var PageeIndex = (double)(Scrolll.ContentOffset.X / Scrolll.Frame.Width);
          
            if (PageeIndex%1==0)
            {
                try
                {
                    var newCamera = CameraPosition.FromCamera(OlusanMarkerlar[(int)PageeIndex].Position, 12, mapView.Camera.Bearing + 10, mapView.Camera.ViewingAngle + 10);
                    mapView.Animate(newCamera);
                }
                catch
                {
                }
            }
            else
            {
            }
        }

        #region Map
        public void GetMap(UIView _mapView)
        {
            CameraPosition camera = CameraPosition.FromCamera(37.797865, -122.402526, 6);

            mapView = MapView.FromCamera(CGRect.Empty, camera);
            mapView.MyLocationEnabled = true;

           
            mapView.MapStyle = MapStyle.FromJson(ReadFile(), null);
            mapView.Frame = _mapView.Bounds;
            _mapView.AddSubview(mapView);
        }
        private string ReadFile()
        {
            using (StreamReader reader = new StreamReader("MapStyle/mapstyle1.txt"))
            {
                return reader.ReadToEnd();
            }

        }
        #endregion
    }
}