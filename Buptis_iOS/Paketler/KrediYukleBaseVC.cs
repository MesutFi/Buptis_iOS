using Buptis_iOS.GenericClass;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using StoreKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UIKit;

namespace Buptis_iOS
{
    public partial class KrediYukleBaseVC : UIViewController
    {
        public PrivateProfileVC PrivateProfileVC1;

      

        public KrediYukleBaseVC (IntPtr handle) : base (handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
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


            UzeriCiziliYap(EskiFiyat1);
            UzeriCiziliYap(EskiFiyat2);
            UzeriCiziliYap(Eskifiyat3);
            UzeriCiziliYap(EakiFiyat4);
        }
        void UzeriCiziliYap(UILabel GelenLabel)
        {
            var attrString = new NSAttributedString(GelenLabel.Text, new UIStringAttributes { StrikethroughStyle = NSUnderlineStyle.Single });
            GelenLabel.AttributedText = attrString;
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
        async void PaketSatinAl(int SeilenPaket)
        {
            var countt = 0;
            string pakett = "";
            switch (SeilenPaket)
            {
                case 1:
                    countt = 200;
                    pakett = "com.buptis.ios.ikiyuzkredi";
                    break;
                case 2:
                    countt = 500;
                    pakett = "com.buptis.ios.besyuzkredi";
                    break;
                case 3:
                    countt = 1000;
                    pakett = "com.buptis.ios.binkredi";
                    break;
                case 4:
                    countt = 2000;
                    pakett = "com.buptis.ios.ikibinkredi";
                    break;
                default:
                    break;
            }
            if (countt != 0)
            {
                try
                {
                    var Durumm = await PurchaseItem(pakett, "buptispayload2");
                    if (Durumm)
                    {
                        PaketSatinAlmaUzakDBAyarla(countt);
                    }
                    else
                    {
                        CustomAlert.GetCustomAlert(this, "Satın Alma Başarısız.");
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
        public async Task<bool> PurchaseItem(string productId, string payload)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync(ItemType.InAppPurchase);
                if (!connected)
                {
                    //we are offline or can't connect, don't try to purchase
                    return true;
                }

                //check purchases
                var purchase = await billing.PurchaseAsync(productId, ItemType.InAppPurchase, payload);

                //possibility that a null came through.
                if (purchase == null)
                {
                    //did not purchase
                    return false;
                }
                else if (purchase.State == PurchaseState.Purchased)
                {
                    //purchased, we can now consume the item or do it later

                    //If we are on iOS we are done, else try to consume the purchase
                    //Device.RuntimePlatform comes from Xamarin.Forms, you can also use a conditional flag or the DeviceInfo plugin
                    //if (Device.RuntimePlatform == Device.iOS)
                    return true;

                    //var consumedItem = await CrossInAppBilling.Current.ConsumePurchaseAsync(purchase.ProductId, purchase.PurchaseToken);

                    //if (consumedItem != null)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
                else
                {
                    return false;
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                Console.WriteLine("Error: " + purchaseEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                //Something else has gone wrong, log it
                Console.WriteLine("Issue connecting: " + ex.Message);
                return false;
            }
            finally
            {

                await billing.DisconnectAsync();
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
                CustomAlert.GetCustomAlert(this, Miktar + " Kredi Satın Aldınız.");
                if (PrivateProfileVC1 != null)
                {
                    PrivateProfileVC1.GetUserLicence();
                }
                this.DismissViewController(true, null);
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Bir sorun oluştu. Lütfen tekrar deneyin.");
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
