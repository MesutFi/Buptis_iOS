using Buptis_iOS.GenericClass;
using Buptis_iOS.Paketler;
using Buptis_iOS.Web_Service;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UIKit;

namespace Buptis_iOS
{
    public partial class BustisGoldBaseVC : UIViewController
    {
        BuptisGoldCustomSlideItem[] Noktalar = new BuptisGoldCustomSlideItem[0];
        List<SlideContent> slideContents = new List<SlideContent>();
        public PrivateProfileVC PrivateProfileVC1;
        public BustisGoldBaseVC (IntPtr handle) : base (handle)
        {
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            AciklamaWebView.BackgroundColor = UIColor.Clear;
            AciklamaWebView.Opaque = false;
            string contentDirectoryPath = Path.Combine(NSBundle.MainBundle.BundlePath, "Content/");
            AciklamaWebView.LoadHtmlString(ReadFile(), new NSUrl(contentDirectoryPath, true));
            SatinAlButton.TouchUpInside += SatinAlButton_TouchUpInside;
            Desing();
            KapatButtonn.TouchUpInside += KapatButtonn_TouchUpInside;
            Paket1Button.TouchUpInside += Paket1Button_TouchUpInside;
            Paket2Button.TouchUpInside += Paket2Button_TouchUpInside;
            Paket3Button.TouchUpInside += Paket3Button_TouchUpInside;
            
            
        }

        private void SatinAlButton_TouchUpInside(object sender, EventArgs e)
        {
            PaketSatinAl(SecilenPaket);
        }


        async void PaketSatinAl(int SeilenPaket)
        {
            var countt = 0;
            string pakett = "";
            switch (SeilenPaket)
            {
                case 1:
                    countt = 1;
                    pakett = "com.buptis.ios.birgold";
                    break;
                case 2:
                    countt = 6;
                    pakett = "com.buptis.ios.altigold";
                    break;
                case 3:
                    countt = 12;
                    pakett = "com.buptis.ios.onikigold";
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
                        CustomAlert.GetCustomAlert(this, "Satýn Alma Baþarýsýz");
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
                count = Miktar,
                credit = 0,
                licenceType = "GOLD"
            };

            WebService webService = new WebService();
            string jsonString = JsonConvert.SerializeObject(licenceBuyDTO);
            var Donus = webService.ServisIslem("licences/buy", jsonString);
            if (Donus != "Hata")
            {
                CustomAlert.GetCustomAlert(this, Miktar + " Aylýk Buptis Gold Paketi Satýn Alýndý.");
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


        int SecilenPaket = 2;
        private void Paket1Button_TouchUpInside(object sender, EventArgs e)
        {
            PaketViewDuzenle(Paket2View);
            PaketViewDuzenle(Paket3View);
            PaketViewDuzenle(Paket1View, true);

            Paket1AyLabel.TextColor = UIColor.FromRGB(34, 30, 32);
            Paket1FiyatLabel.TextColor = UIColor.White;
            Paket1IndirimLabel.TextColor = UIColor.FromRGB(34, 30, 32);

            SecilenPaket = 1;
        }
        private void Paket2Button_TouchUpInside(object sender, EventArgs e)
        {
            PaketViewDuzenle(Paket1View);
            PaketViewDuzenle(Paket3View);
            PaketViewDuzenle(Paket2View, true);
            Paket2AyLabel.TextColor = UIColor.FromRGB(34, 30, 32);
            Paket2FiyatLabel.TextColor = UIColor.White;
            Paket2IndirimLabel.TextColor = UIColor.FromRGB(34, 30, 32);
            SecilenPaket = 2;
        }
        private void Paket3Button_TouchUpInside(object sender, EventArgs e)
        {
            PaketViewDuzenle(Paket1View);
            PaketViewDuzenle(Paket2View);
            PaketViewDuzenle(Paket3View, true);
            Paket3AyLabel.TextColor = UIColor.FromRGB(34, 30, 32);
            Paket3FiyatLabel.TextColor = UIColor.White;
            Paket3IndirimLabel.TextColor = UIColor.FromRGB(34, 30, 32);
            SecilenPaket = 3;
        }

        private void KapatButtonn_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            HazneView.ClipsToBounds = true;
            HazneView.Layer.CornerRadius = 30f;
            HazneView.Layer.MaskedCorners = (CoreAnimation.CACornerMask)3;
            CreateScrollViews();
        }


        #region SetUI
        void Desing()
        {
            PaketViewDuzenle(Paket1View);
            PaketViewDuzenle(Paket2View,true);
            PaketViewDuzenle(Paket3View);

            SatinAlButton.Layer.CornerRadius = 23f;
            SatinAlButton.Layer.ShadowOpacity = 0.8f;
            SatinAlButton.Layer.ShadowOffset = new CGSize(0, 0);
            SatinAlButton.Layer.ShadowColor = UIColor.Black.CGColor;


            Paket2AyLabel.TextColor = UIColor.FromRGB(34, 30, 32);
            Paket2FiyatLabel.TextColor = UIColor.White;
            Paket2IndirimLabel.TextColor = UIColor.FromRGB(34, 30, 32);
        }

        void PaketViewDuzenle(UIView GelenView, bool IsSelecet = false)
        {
            if (IsSelecet)
            {
                GelenView.BackgroundColor = UIColor.FromRGB(198, 169, 70);
                GelenView.Layer.BorderColor = UIColor.FromRGB(225, 0, 104).CGColor;
                GelenView.Layer.BorderWidth = 0f;
                GelenView.Layer.CornerRadius = 30f;
                GelenView.Layer.ShadowOpacity = 0.8f;
                GelenView.Layer.ShadowOffset = new CGSize(0, 0);
                GelenView.Layer.ShadowColor = UIColor.Black.CGColor;
            }
            else
            {
                GelenView.BackgroundColor = UIColor.Clear;
                GelenView.Layer.BorderColor = UIColor.FromRGB(155, 155, 155).CGColor;
                GelenView.Layer.BorderWidth = 1f;
                GelenView.Layer.CornerRadius = 30f;
                GelenView.Layer.ShadowOpacity = 0f;
                GelenView.Layer.ShadowOffset = new CGSize(0, 0);
                GelenView.Layer.ShadowColor = UIColor.Clear.CGColor;
                Paket1AyLabel.TextColor = UIColor.FromRGB(188, 159, 67);
                Paket2AyLabel.TextColor = UIColor.FromRGB(188, 159, 67);
                Paket3AyLabel.TextColor = UIColor.FromRGB(188, 159, 67);

                Paket1FiyatLabel.TextColor = UIColor.White;
                Paket2FiyatLabel.TextColor = UIColor.White;
                Paket3FiyatLabel.TextColor = UIColor.White;

                Paket1IndirimLabel.TextColor = UIColor.FromRGB(155, 155, 155);
                Paket2IndirimLabel.TextColor = UIColor.FromRGB(155, 155, 155);
                Paket3IndirimLabel.TextColor = UIColor.FromRGB(155, 155, 155);
            }
        }
        #endregion

        private string ReadFile()
        {
            using (StreamReader reader = new StreamReader("Strings/bustis_gold_info_string.txt"))
            {
                return reader.ReadToEnd();
            }
        }

        void CreateScrollViews()
        {
            InfoScroll.ShowsVerticalScrollIndicator = false;
            InfoScroll.ShowsHorizontalScrollIndicator = false;
            slideContents.Add(new SlideContent() { 
            AciklamaText = "Daha çok kiþiyle sohbet edin!",
            ImageName = "gold_icon1.png"
            });
            slideContents.Add(new SlideContent()
            {
                AciklamaText = "Ýsterseniz kimliðinizi gizleyin!",
                ImageName = "gold_icon2.png"
            });
            slideContents.Add(new SlideContent()
            {
                AciklamaText = "Her ay 3 Boost kazanýn!",
                ImageName = "gold_icon3.png"
            });
            slideContents.Add(new SlideContent()
            {
                AciklamaText = "Her ay 3 Super Boost kazanýn!",
                ImageName = "gold_icon5.png"
            });
            slideContents.Add(new SlideContent()
            {
                AciklamaText = "Anýnda 1000 Kredi kazanýn!",
                ImageName = "gold_icon4.png"
            });
            slideContents.Add(new SlideContent()
            {
                AciklamaText = "Reklam izlemeyin!",
                ImageName = "gold_icon6.png"
            });

            PageControll.Pages = slideContents.Count;
            Noktalar = new BuptisGoldCustomSlideItem[slideContents.Count];
            for (int i = 0; i < slideContents.Count; i++)
            {
                var NoktaItem = BuptisGoldCustomSlideItem.Create(slideContents[i].ImageName, slideContents[i].AciklamaText);
                if (i == 0)
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, 113f);
                }
                else
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(UIKit.UIScreen.MainScreen.Bounds.Width * i, 0, UIKit.UIScreen.MainScreen.Bounds.Width, 113f);
                }

                InfoScroll.AddSubview(NoktaItem);
                Noktalar[i] = NoktaItem;
            }

            InfoScroll.ContentSize = new CoreGraphics.CGSize(Noktalar[Noktalar.Length - 1].Frame.Right, 113f);
            InfoScroll.Scrolled += InfoScroll_Scrolled;
            InfoScroll.PagingEnabled = true;
        }

        private void InfoScroll_Scrolled(object sender, EventArgs e)
        {
            var PageeIndex = (nint)(InfoScroll.ContentOffset.X / InfoScroll.Frame.Width);
            PageControll.CurrentPage = PageeIndex;
        }

        public class SlideContent
        {
            public string ImageName { get; set; }
            public string AciklamaText { get; set; }
        }
        public class LicenceBuyDTO
        {
            public int count { get; set; }
            public int credit { get; set; }
            public string licenceType { get; set; }
        }
    }
}