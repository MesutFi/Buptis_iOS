using System;
using System.Collections.Generic;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Newtonsoft.Json.Linq;
using UIKit;
using static Buptis_iOS.LokasyonlarBanaYakin;

namespace Buptis_iOS.Lokasyonlar
{
    public partial class LokasyonlarTableCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("LokasyonlarTableCell");
        public static readonly UINib Nib;
        Mekanlar_Location mekanlarlocation;

        static LokasyonlarTableCell()
        {
            Nib = UINib.FromName("LokasyonlarTableCell", NSBundle.MainBundle);
        }

        protected LokasyonlarTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public static LokasyonlarTableCell Create()
        {
            var OlusanView = (LokasyonlarTableCell)Nib.Instantiate(null, null)[0];
            return OlusanView;

        }
        public void UpdateCell(Mekanlar_Location mekanlar1)
        {
            mekanlarlocation = mekanlar1;
            Design();
            GetData();
        }




        public void GetData()
        {
            MekanTur.Text = "";
            MekanAdiLabel.Text ="   "+ mekanlarlocation.name;

            MekanAdiLabel.LayoutIfNeeded();
            var genislik = (MekanAdiLabel.IntrinsicContentSize.Width);
            var manewframee = MekanAdiLabel.Frame;
            manewframee.Width = genislik;
            MekanAdiLabel.Frame = manewframee;


           var ratdurum = mekanlarlocation.rating;
            if (ratdurum >= 10.0)
            {
                MekanRating.SetTitle("10", UIControlState.Normal);

            }
            else
            {
                MekanRating.SetTitle(Math.Round(Convert.ToDouble(mekanlarlocation.rating), 1).ToString(), UIControlState.Normal);
            }
            DolulukProgress.Progress = (float)Math.Round( (float)(mekanlarlocation.allUserCheckIn / mekanlarlocation.capacity), 2);
            //MekanYer.Text = mekanlarlocation.townId.ToString() +" " + mekanlarlocation.place +"km";
            GetLocationOtherInfo(mekanlarlocation.id, mekanlarlocation.catIds, mekanlarlocation.townId, mekanlarlocation.coordinateX,mekanlarlocation.coordinateY);
        }


        nfloat ButonGenislikHesapla(string Content)
        {
            var SanalLabel = new UILabel();
            SanalLabel.Text = Content;
            SanalLabel.LayoutIfNeeded();
            return (SanalLabel.IntrinsicContentSize.Width + 20f);
        }


        void GetLocationOtherInfo(int locid, List<string> catid, string townid, double FromLat, double FromLon)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();

                #region Uzaklik Ve Sempt
                if (!string.IsNullOrEmpty(townid))
                {
                    var Donus1 = webService.OkuGetir("towns/" + townid.ToString());
                    if (Donus1 != null)
                    {
                        JObject obj = JObject.Parse(Donus1.ToString());
                        string TownName = (string)obj["townName"];
                        InvokeOnMainThread(() => {
                            if (UserLastloc != null)
                            {
                                var km = GetUserCityCountryAndDistance(UserLastloc.Coordinate.Latitude, UserLastloc.Coordinate.Longitude, FromLat, FromLon);

                                if (km > 0)
                                {
                                    var yuvarla = Math.Round(km, 1);
                                    MekanYer.Text = TownName + " / " + yuvarla.ToString() + " km";
                                }
                                
                            }
                            else
                            {
                                MekanYer.Text = TownName;
                            }
                           
                        });
                    }
                    else
                    {
                        InvokeOnMainThread(() => {
                            MekanYer.Text = "";
                        });
                    }
                }
                #endregion

                #region LokasyonTuru
                if (catid != null)
                {
                    if (catid.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(catid[0]))
                        {
                            var Donus2 = webService.OkuGetir("categories/ " + catid[0].ToString());
                            if (Donus2 != null)
                            {
                                JObject obj = JObject.Parse(Donus2.ToString());
                                string KategoriAdi = (string)obj["name"];
                                InvokeOnMainThread(() => {
                                    MekanTur.Text = KategoriAdi;
                                });
                            }
                            else
                            {
                                InvokeOnMainThread(() => {
                                    MekanTur.Text = "";
                                });
                            }
                        }
                    }
                }
                #endregion

            })).Start();
        }


        #region Custom Design
        public void Design()
        {
            this.ContentView.BackgroundColor = UIColor.Clear;
            BeyazArkaHazne.BackgroundColor = UIColor.White;
            BeyazArkaHazne.Layer.CornerRadius = 15f;
            BeyazArkaHazne.ClipsToBounds = true;

            //var C1 = UIColor.FromRGB(238, 0, 60).CGColor;
            //var C2 = UIColor.FromRGB(225, 0, 105).CGColor;
            //var labelgradient = new CAGradientLayer();
            //labelgradient.Colors = new CoreGraphics.CGColor[] { C1, C2 };
            //labelgradient.Locations = new NSNumber[] { 0.0, 1.0 };
            //labelgradient.Frame = MekanAdiLabel.Frame;
            //MekanAdiLabel.Layer.InsertSublayer(labelgradient, 0);
            MekanAdiLabel.Layer.CornerRadius = 15f;
            MekanAdiLabel.ClipsToBounds = true;

            //CATransform3D transform = new CATransform3D().Scale(1.0f, 3.0f, 1.0f);
            //DolulukProgress.Layer.Transform = transform;

            DolulukProgress.Transform = CGAffineTransform.MakeScale(1f, 10f);

            DolulukBgView.Layer.BorderWidth = 1f;
            DolulukBgView.Layer.BorderColor = UIColor.FromRGB(237, 0, 64).CGColor;
            DolulukBgView.Layer.CornerRadius = 5f;
            DolulukBgView.ClipsToBounds = true;
            //scaledBy(x: 1, y: 9)
        }
        #endregion



        public double GetUserCityCountryAndDistance(double ToLat,double ToLon, double FromLat, double FromLon)
        {
            try
            {
                float pk = (float)(180f / Math.PI);

                float a1 = (float)FromLat / pk;
                float a2 = (float)FromLon / pk;
                float b1 = (float)ToLat / pk;
                float b2 = (float)ToLon / pk;

                double t1 = Math.Cos(a1) * Math.Cos(a2) * Math.Cos(b1) * Math.Cos(b2);
                double t2 = Math.Cos(a1) * Math.Sin(a2) * Math.Cos(b1) * Math.Sin(b2);
                double t3 = Math.Sin(a1) * Math.Sin(b1);
                double tt = Math.Acos(t1 + t2 + t3);

                var aaaaa = Math.Round(((6366000 * tt) / 1000),1);
                return aaaaa;
            }
            catch (Exception ex)
            {
                var aa = ex.Message;
                return 0;
            }
        }


        public class CatDTO
        {
            public string createdDate { get; set; }
            public int id { get; set; }
            public string lastModifiedDate { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }

        public static CLLocation UserLastloc { get; set; }
    }
}
