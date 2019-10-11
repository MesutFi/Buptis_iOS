
using System;
using System.Collections.Generic;
using System.Drawing;
using Buptis_iOS.Database;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Buptis_iOS.Mesajlar
{
    public partial class MesajlarBaseVC : UIViewController
    {
        #region Tanimlamalar
        List<UIButton> Menuler = new List<UIButton>();
        #endregion
        public MesajlarBaseVC(IntPtr handle) : base(handle)
        {
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            GeriButton.TouchUpInside += GeriButton_TouchUpInside;
            MesajlarButton.TouchUpInside += MesajlarButton_TouchUpInside;
            IsteklerButton.TouchUpInside += IsteklerButton_TouchUpInside;
            FavorilerButton.TouchUpInside += FavorilerButton_TouchUpInside;
            AraButton.TouchUpInside += AraButton_TouchUpInside;
            KapatButton.TouchUpInside += KapatButton_TouchUpInside;
            AraText.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
        }

        private void KapatButton_TouchUpInside(object sender, EventArgs e)
        {
            AraText.Text = "";
            AraBgView.Hidden = true;
        }

        private void AraButton_TouchUpInside(object sender, EventArgs e)
        {
            AraBgView.Hidden = false;
        }

        private void FavorilerButton_TouchUpInside(object sender, EventArgs e)
        {
            ButtonTasarimlariniDuzenle(2);
            ClearViewContents();
            var MesajlarFavoriler1 = MesajlarFavoriler.Create(this,AraText);
            MesajlarFavoriler1.Frame = new CGRect(0, 0, ContentView.Frame.Width, ContentView.Frame.Height);
            ContentView.AddSubview(MesajlarFavoriler1);
        }

        private void IsteklerButton_TouchUpInside(object sender, EventArgs e)
        {
            ButtonTasarimlariniDuzenle(1);
            ClearViewContents();
            var MesajlarIstekler1 = MesajlarIstekler.Create(this,AraText);
            MesajlarIstekler1.Frame = new CGRect(0, 0, ContentView.Frame.Width, ContentView.Frame.Height);
            ContentView.AddSubview(MesajlarIstekler1);
        }

        private void MesajlarButton_TouchUpInside(object sender, EventArgs e)
        {
            ButtonTasarimlariniDuzenle(0);
            ClearViewContents();
            var MesajlarNormal1 = MesajlarNormal.Create(this, AraText);
            MesajlarNormal1.Frame = new CGRect(0, 0, ContentView.Frame.Width, ContentView.Frame.Height);
            ContentView.AddSubview(MesajlarNormal1);
        }

        private void GeriButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }
        bool Actinmi1, Actinmi2;
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (!Actinmi1)
            {
                HeaderView.Hidden = true;
                AraBgView.Hidden = true;
                Actinmi1 = true;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!Actinmi2)
            {
                Menuler = new List<UIButton>();
                Menuler.Add(MesajlarButton);
                Menuler.Add(IsteklerButton);
                Menuler.Add(FavorilerButton);
                TasarimiDuzenle();
                ButtonTasarimlariniDuzenle(0);
                GeriButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
                AraButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
                HeaderView.Hidden = false;
                var MesajlarNormal1 = MesajlarNormal.Create(this, AraText);
                MesajlarNormal1.Frame = new CGRect(0, 0, ContentView.Frame.Width, ContentView.Frame.Height);
                ContentView.AddSubview(MesajlarNormal1);
                Actinmi2 = true;
            }
        }
        #endregion


        void ClearViewContents()
        {
            foreach (var item in ContentView.Subviews)
            {
                item.RemoveFromSuperview();
            }

        }

        #region  UI Tasarimlar
        void TasarimiDuzenle()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = HeaderView.Frame;
            HeaderView.Layer.InsertSublayer(gradientLayer, 0);
            HeaderView.Layer.CornerRadius = 30;
            HeaderView.ClipsToBounds = true;
            HeaderView.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);

            TabMenu.BackgroundColor = UIColor.White;
            TabMenu.Layer.CornerRadius = 10f;
            TabMenu.ClipsToBounds = true;

            AraBgView.Layer.CornerRadius = 10;
            AraBgView.ClipsToBounds = true;
            KapatButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
        }

        void ButtonTasarimlariniDuzenle(int Index)
        {
            for (int i = 0; i < Menuler.Count; i++)
            {
                Menuler[i].BackgroundColor = UIColor.Clear;
                Menuler[i].SetTitleColor(UIColor.Black, UIControlState.Normal);
            }
            Menuler[Index].BackgroundColor = UIColor.FromRGB(226, 0, 93);
            Menuler[Index].Layer.CornerRadius = 10f;
            Menuler[Index].ClipsToBounds = true;
            Menuler[Index].SetTitleColor(UIColor.White, UIControlState.Normal);
        }

        #endregion

        public  static class MesajlarIcinSecilenKullanici
        {
            public static MEMBER_DATA Kullanici { get; set; }
            public static string key { get; set; }
        }
        public class MesajKisileri
        {
            public string firstName { get; set; }
            public string key { get; set; }
            public string lastChatText { get; set; }
            public string lastModifiedDate { get; set; }
            public string lastName { get; set; }
            public int receiverId { get; set; }
            public bool request { get; set; }
            public int unreadMessageCount { get; set; }
        }
    }
}