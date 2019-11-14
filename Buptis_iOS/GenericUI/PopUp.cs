using CoreAnimation;
using Foundation;
using System;
using System.Drawing;
using UIKit;

namespace Buptis_iOS
{
    public partial class PopUp : UIViewController
    {
        #region Tanimlamalar
        string title, description, btn1title, btn2title;
        #endregion
        public PopUp (IntPtr handle) : base (handle)
        {

        }
        public void InitVC(string title , string description , string btn1title , string btn2title)
        {
            this.title = title;
            this.description = description;
            this.btn1title = btn1title;
            this.btn2title = btn2title;
        }

        #region View LifeCycle
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            UITasarim();
            IconlariAyarla(ImageView2);
            ButtonTintAyarla(CloseButton);
           
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            this.TitleLabel.Text = "PopUp Title";
            this.DescriptionLabel.Text = "Lorem upsum del mundo lel diptca lorem nunpu..";
            this.PopupButton1.SetTitle("Lorem", UIControlState.Normal);
            this.PopupButton2.SetTitle("Upsum", UIControlState.Normal);
            this.View.BackgroundColor = UIColor.Clear;
            this.View.BackgroundColor = UIColor.FromRGB(15, 0, 241).ColorWithAlpha(0.7f);
        }
        #endregion

        #region PopUp UI Tasarim
        public void UITasarim()
        {
            var C1 = UIColor.FromRGB(238, 0, 60).CGColor;
            var C2 = UIColor.FromRGB(225, 0, 105).CGColor;
            var gradientLayer1 = new CAGradientLayer();
            gradientLayer1.Colors = new CoreGraphics.CGColor[] { C1, C2 };
            gradientLayer1.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer1.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer1.Frame = PopupButton1.Bounds;
            PopupButton1.Layer.InsertSublayer(gradientLayer1, 0);
            PopupButton1.Layer.CornerRadius = PopupButton1.Frame.Height / 2;
            PopupButton1.ClipsToBounds = true;

            CloseButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);

            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = ImageView2.Frame;
            ImageView2.Layer.InsertSublayer(gradientLayer, 0);
            ImageView2.Layer.CornerRadius = ImageView2.Frame.Height / 2;
            ImageView2.ClipsToBounds = true;

            PopUpView.Layer.CornerRadius = 30f;
        }
        void IconlariAyarla(UIImageView Iconn)
        {
            Iconn.Image = ResizeImage(Iconn.Image,40,40);
            Iconn.ContentMode = UIViewContentMode.Center;
        }
        void ButtonTintAyarla(UIButton gelenButton)
        {
            //var IconImage = Iconn.Image.ImageWithAlignmentRectInsets(new UIEdgeInsets(top: -5, left: -5, bottom: -5, right: -5));
            var TintImage = gelenButton.ImageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            gelenButton.TintColor = UIColor.FromRGB(15, 0, 241);
            gelenButton.SetImage(TintImage, UIControlState.Normal);
        }
        public UIImage ResizeImage(UIImage sourceImage, float width, float height)
        {
            UIGraphics.BeginImageContext(new SizeF(width, height));
            sourceImage.Draw(new RectangleF(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }
        #endregion
    }
}
