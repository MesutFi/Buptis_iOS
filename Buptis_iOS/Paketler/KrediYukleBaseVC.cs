using Buptis_iOS.GenericClass;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using StoreKit;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;

namespace Buptis_iOS
{
    public partial class KrediYukleBaseVC : UIViewController
    {
        public PrivateProfileVC PrivateProfileVC1;

        #region iTunes Properties

        public static string Kredi_200_ID = "com.buptis.ios.ikiyuzkredi";
        public static string Kredi_500_ID = "com.buptis.ios.besyuzkredi";
        public static string Kredi_1000_ID = "com.buptis.ios.binkredi"; 
        public static string Kredi_2000_ID = "com.buptis.ios.ikibinkredi";
        List<string> products;
        bool pricesLoaded = false;
        NSObject priceObserver, succeededObserver, failedObserver, requestObserver;

        CustomPaymentObserver theObserver;
        InAppPurchaseManager iap;
        #endregion

        public KrediYukleBaseVC (IntPtr handle) : base (handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            #region iTunes Satin Alma
            products = new List<string>() { Kredi_200_ID, Kredi_500_ID, Kredi_1000_ID, Kredi_2000_ID };
            iap = new InAppPurchaseManager();
            theObserver = new CustomPaymentObserver(iap);
            // Call this once upon startup of in-app-purchase activities
            // This also kicks off the TransactionObserver which handles the various communications
            SKPaymentQueue.DefaultQueue.AddTransactionObserver(theObserver);

            #endregion
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

            #region iTunes SatinAlmaIslemleri
            // setup the observer to wait for prices to come back from StoreKit <- AppStore
            priceObserver = NSNotificationCenter.DefaultCenter.AddObserver(InAppPurchaseManager.InAppPurchaseManagerProductsFetchedNotification,
                (notification) => {
                    var info = notification.UserInfo;
                    if (info == null)
                        return;

                    var NSKredi_200_IDProductId = new NSString(Kredi_200_ID);

                    if (info.ContainsKey(NSKredi_200_IDProductId))
                    {
                        pricesLoaded = true;
                        var product = (SKProduct)info[NSKredi_200_IDProductId];
                        Print(product);
                    }

                    var NSKredi_500_IDProductId = new NSString(Kredi_500_ID);
                    if (info.ContainsKey(NSKredi_500_IDProductId))
                    {
                        pricesLoaded = true;
                        var product = (SKProduct)info[NSKredi_500_IDProductId];
                        Print(product);
                    }


                    var NSKredi_1000_IDProductId = new NSString(Kredi_1000_ID);
                    if (info.ContainsKey(NSKredi_1000_IDProductId))
                    {
                        pricesLoaded = true;
                        var product = (SKProduct)info[NSKredi_1000_IDProductId];
                        Print(product);
                    }

                    var NSKredi_2000_IDProductId = new NSString(Kredi_2000_ID);
                    if (info.ContainsKey(NSKredi_2000_IDProductId))
                    {
                        pricesLoaded = true;
                        var product = (SKProduct)info[NSKredi_2000_IDProductId];
                        Print(product);
                    }
                });

            // only if we can make payments, request the prices
            if (iap.CanMakePayments())
            {
                // now go get prices, if we don't have them already
                if (!pricesLoaded)
                    iap.RequestProductData(products); // async request via StoreKit -> App Store
            }
            else
            {
                // can't make payments (purchases turned off in Settings?)
                SatinAlButton.SetTitle("AppStore Kullaným Dýþý", UIControlState.Disabled);
                SatinAlButton.SetTitle("AppStore Kullaným Dýþý", UIControlState.Disabled);
            }

            //balanceLabel.Text = String.Format(Balance, CreditManager.Balance());// + " monkey credits";

            succeededObserver = NSNotificationCenter.DefaultCenter.AddObserver(InAppPurchaseManager.InAppPurchaseManagerTransactionSucceededNotification,
            (notification) => {
                Console.WriteLine("Satýn Alma Baþarýlý");
                //balanceLabel.Text = String.Format(Balance, CreditManager.Balance());// + " monkey credits";
            });
            failedObserver = NSNotificationCenter.DefaultCenter.AddObserver(InAppPurchaseManager.InAppPurchaseManagerTransactionFailedNotification,
            (notification) => {
                // TODO:
                Console.WriteLine("Transaction Failed");
            });

            requestObserver = NSNotificationCenter.DefaultCenter.AddObserver(InAppPurchaseManager.InAppPurchaseManagerRequestFailedNotification,
                                                                             (notification) => {
                                                                                 // TODO:
                                                                                 Console.WriteLine("Request Failed");
                                                                                 //buy5Button.SetTitle("Network down?", UIControlState.Disabled);
                                                                                 //buy10Button.SetTitle("Network down?", UIControlState.Disabled);
                                                                             });
            #endregion

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
            this.DismissViewController(true,null);
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
            PaketViewDuzenle(Paket3View,true);
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
            using (StreamReader reader = new StreamReader("Strings/kredi_info_string.txt"))
            {
                return reader.ReadToEnd();
            }
        }
        void PaketSatinAl(int SeilenPaket)
        {
            var countt = 0;
            string pakett = "";
            switch (SeilenPaket)
            {
                case 1:
                    countt = 200;
                    pakett = Kredi_200_ID;
                    break;
                case 2:
                    countt = 500;
                    pakett = Kredi_500_ID;
                    break;
                case 3:
                    countt = 1000;
                    pakett = Kredi_1000_ID;
                    break;
                case 4:
                    countt = 2000;
                    pakett = Kredi_2000_ID;
                    break;
                default:
                    break;
            }
            if (countt != 0)
            {
                iap.PurchaseProduct(pakett);
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
                count = 0,
                credit = Miktar,
                licenceType = "ONLY_CREDÝT"
            };

            WebService webService = new WebService();
            string jsonString = JsonConvert.SerializeObject(licenceBuyDTO);
            var Donus = webService.ServisIslem("licences/buy", jsonString);
            if (Donus != "Hata")
            {
                CustomAlert.GetCustomAlert(this, Miktar + " Kredi Satýn Alýndý.");
                if (PrivateProfileVC1 != null)
                {
                    PrivateProfileVC1.GetUserLicence();
                }
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


        #region Itunes Satin Alma Islemleri
        void Print(SKProduct product)
		{
			Console.WriteLine("Product id: {0}", product.ProductIdentifier);
			Console.WriteLine("Product title: {0}", product.LocalizedTitle);
			Console.WriteLine("Product description: {0}", product.LocalizedDescription);
			Console.WriteLine("Product price: {0}", product.Price);
			Console.WriteLine("Product l10n price: {0}", product.LocalizedPrice());
		}
      
        #endregion
    }
}