using CoreAnimation;
using Foundation;
using System;
using UIKit;
using static CoreFoundation.CFBundle;

namespace Buptis_iOS
{
    public partial class HakkimizdaVC : UIViewController
    {
        public HakkimizdaVC (IntPtr handle) : base (handle)
        {
          
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            HeaderTasarim();
            BackButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            BackButton.TouchUpInside += BackButton_TouchUpInside;
            SartlarButton.TouchUpInside += SartlarButton_TouchUpInside;
            GizlilikButton.TouchUpInside += GizlilikButton_TouchUpInside;
            GetVersion();
        }

        private void GizlilikButton_TouchUpInside(object sender, EventArgs e)
        {

            string UrlString = "https://www.buptis.com/gizlilik.html";
            this.InvokeOnMainThread(() =>
            {
                var uri = NSUrl.FromString(UrlString);
                UIApplication.SharedApplication.OpenUrl(uri);
            });
        }

        private void SartlarButton_TouchUpInside(object sender, EventArgs e)
        {
            string UrlString = "https://www.buptis.com/kullanim-kosullari.html";
            this.InvokeOnMainThread(() =>
            {
                var uri = NSUrl.FromString(UrlString);
                UIApplication.SharedApplication.OpenUrl(uri);
            });
        }
        void GetVersion()
        {
            NSObject ver = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
            HakkindaLabel.Text = ver.ToString();
        }
        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }

        #region UI Tasarim 
        void HeaderTasarim()
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
        }
        #endregion
    }
}
