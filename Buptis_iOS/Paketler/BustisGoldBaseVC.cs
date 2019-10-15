using Buptis_iOS.Paketler;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;

namespace Buptis_iOS
{
    public partial class BustisGoldBaseVC : UIViewController
    {
        BuptisGoldCustomSlideItem[] Noktalar = new BuptisGoldCustomSlideItem[0];
        List<SlideContent> slideContents = new List<SlideContent>();
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
            Desing();
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
            PaketViewDuzenle(Paket2View, true);
            PaketViewDuzenle(Paket3View);

            SatinAlButton.Layer.CornerRadius = 23f;
            SatinAlButton.Layer.ShadowOpacity = 0.8f;
            SatinAlButton.Layer.ShadowOffset = new CGSize(0, 0);
            SatinAlButton.Layer.ShadowColor = UIColor.Black.CGColor;
        }

        void PaketViewDuzenle(UIView GelenView, bool IsSelecet = false)
        {
            if (IsSelecet)
            {
                GelenView.BackgroundColor = UIColor.FromRGB(202, 171, 71);
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
    }
}