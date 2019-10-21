using Buptis_iOS.GenericClass;
using Buptis_iOS.Web_Service;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using Plugin.InAppBilling;
using System;
using System.IO;
using UIKit;

namespace Buptis_iOS
{
    public partial class SuperBoostBaseVC : UIViewController
    {
        public PrivateProfileVC PrivateProfileVC1;
        public SuperBoostBaseVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            AciklamaWebView.BackgroundColor = UIColor.Clear;
            AciklamaWebView.Opaque = false;
            string contentDirectoryPath = Path.Combine(NSBundle.MainBundle.BundlePath, "Content/");
            AciklamaWebView.LoadHtmlString(ReadFile(), new NSUrl(contentDirectoryPath, true));
            Desing();
            KapatButton.TouchUpInside += KapatButton_TouchUpInside;
            Paket1Button.TouchUpInside += Paket1Button_TouchUpInside;
            Paket2Button.TouchUpInside += Paket2Button_TouchUpInside;
            Paket3Button.TouchUpInside += Paket3Button_TouchUpInside;
            Paket4Button.TouchUpInside += Paket4Button_TouchUpInside;
            SatinAlButton.TouchUpInside += SatinAlButton_TouchUpInside;
        }

        private void SatinAlButton_TouchUpInside(object sender, EventArgs e)
        {
            PaketSatinAl(SecilenPaket);
        }

        int SecilenPaket = 3;
        private void Paket1Button_TouchUpInside(object sender, EventArgs e)
        {
            PaketViewDuzenle(Paket1View, true);
            PaketViewDuzenle(Paket2View);
            PaketViewDuzenle(Paket3View);
            PaketViewDuzenle(Paket4View);
            SecilenPaket = 1;
        }
        private void Paket2Button_TouchUpInside(object sender, EventArgs e)
        {
            PaketViewDuzenle(Paket1View);
            PaketViewDuzenle(Paket2View, true);
            PaketViewDuzenle(Paket3View);
            PaketViewDuzenle(Paket4View);
            SecilenPaket = 2;
        }
        private void Paket3Button_TouchUpInside(object sender, EventArgs e)
        {
            PaketViewDuzenle(Paket1View);
            PaketViewDuzenle(Paket2View);
            PaketViewDuzenle(Paket3View, true);
            PaketViewDuzenle(Paket4View);
            SecilenPaket = 3;
        }
        private void Paket4Button_TouchUpInside(object sender, EventArgs e)
        {
            PaketViewDuzenle(Paket1View);
            PaketViewDuzenle(Paket2View);
            PaketViewDuzenle(Paket3View);
            PaketViewDuzenle(Paket4View, true);
            SecilenPaket = 4;
        }

        private void KapatButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            //var maskingShapeLayer = new CAShapeLayer()
            //{
            //    Path = UIBezierPath.FromRoundedRect(HazneView.Bounds, UIRectCorner.TopLeft | UIRectCorner.TopRight, new CGSize(30, 30)).CGPath
            //};
            // HazneView.Layer.CornerRadius = 
            HazneView.ClipsToBounds = true;
            HazneView.Layer.CornerRadius = 30f;
            HazneView.Layer.MaskedCorners = (CoreAnimation.CACornerMask)3;
        }

        #region SetUI
        void Desing()
        {
            PaketViewDuzenle(Paket1View);
            PaketViewDuzenle(Paket2View);
            PaketViewDuzenle(Paket3View, true);
            PaketViewDuzenle(Paket4View);

            SatinAlButton.Layer.CornerRadius = 23f;
            SatinAlButton.Layer.ShadowOpacity = 0.8f;
            SatinAlButton.Layer.ShadowOffset = new CGSize(0, 0);
            SatinAlButton.Layer.ShadowColor = UIColor.Black.CGColor;

            //HazneView.Layer.CornerRadius = 30;
            //HazneView.ClipsToBounds = true;
            //HazneView.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);
        }

        void PaketViewDuzenle(UIView GelenView, bool IsSelecet = false)
        {
            if (IsSelecet)
            {
                GelenView.BackgroundColor = UIColor.Clear;
                GelenView.Layer.BorderColor = UIColor.FromRGB(225, 0, 104).CGColor;
                GelenView.Layer.BorderWidth = 1f;
                GelenView.Layer.CornerRadius = 20f;
                GelenView.Layer.ShadowOpacity = 0.8f;
                GelenView.Layer.ShadowOffset = new CGSize(0, 0);
                GelenView.Layer.ShadowColor = UIColor.Black.CGColor;
            }
            else
            {
                GelenView.BackgroundColor = UIColor.Clear;
                GelenView.Layer.BorderColor = UIColor.FromRGB(155, 155, 155).CGColor;
                GelenView.Layer.BorderWidth = 1f;
                GelenView.Layer.CornerRadius = 20f;
                GelenView.Layer.ShadowOpacity = 0f;
                GelenView.Layer.ShadowOffset = new CGSize(0, 0);
                GelenView.Layer.ShadowColor = UIColor.Clear.CGColor;
            }

        }
        #endregion

        private string ReadFile()
        {
            using (StreamReader reader = new StreamReader("Strings/super_boost_info.txt"))
            {
                return reader.ReadToEnd();
            }
        }

        async void PaketSatinAl(int SeilenPaket)
        {
            var countt = 0;
            string pakett = "";
            switch (SeilenPaket)
            {
                case 1:
                    countt = 1;
                    pakett = "com.buptis.ios.birsuperboost";
                    break;
                case 2:
                    countt = 2;
                    pakett = "com.buptis.ios.ikisuperboost";
                    break;
                case 3:
                    countt = 3;
                    pakett = "com.buptis.ios.ucsuperboost";
                    break;
                case 4:
                    countt = 5;
                    pakett = "com.buptis.ios.bessuperboost";
                    break;
                default:
                    break;
            }

            if (countt!= 0)
            {
                try
                {
                    var purchase = await CrossInAppBilling.Current.PurchaseAsync(pakett, Plugin.InAppBilling.Abstractions.ItemType.InAppPurchase, "buptispayload");

                    if (purchase == null)
                    {
                        CustomAlert.GetCustomAlert(this, "Bir Sorun Oluþtu!");
                        //AlertHelper.AlertGoster("Bir Sorun Oluþtu!", this.Activity);
                    }
                    else
                    {
                        PaketSatinAlmaUzakDBAyarla(countt);
                    }
                }
                catch (Exception ex)
                {
                    CustomAlert.GetCustomAlert(this, ex.Message);
                    Console.WriteLine(ex);
                }
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Lütfen bir paket seçin.");
            }
          
        }

        void PaketSatinAlmaUzakDBAyarla(int Miktar)
        {
            LicenceBuyDTO licenceBuyDTO = new LicenceBuyDTO()
            {
                count = Miktar,
                credit = 0,
                licenceType = "SUPER_BOOST"
            };

            WebService webService = new WebService();
            string jsonString = JsonConvert.SerializeObject(licenceBuyDTO);
            var Donus = webService.ServisIslem("licences/buy", jsonString);
            if (Donus != "Hata")
            {
                CustomAlert.GetCustomAlert(this, Miktar + " Super Boost Satýn Alýndý.");
                PrivateProfileVC1.GetUserLicence();
                this.DismissViewController(true, null);
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Bir sorun oluþtu. Lütfen tekrar deneyin.");
            }
        }

        public class LicenceBuyDTO
        {
            public int count { get; set; }
            public int credit { get; set; }
            public string licenceType { get; set; }
        }
    }
}