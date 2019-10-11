
using System;
using System.Collections.Generic;
using System.Drawing;
using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Lokasyonlar;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using static Buptis_iOS.EngelleVC;

namespace Buptis_iOS.LokasyondakiKisiler
{
    public partial class LokasyondakiKisilerBaseVC : UIViewController
    {
        #region Tanimlamalar
        List<UIButton> Menuler = new List<UIButton>();
        public Mekanlar_Location gelenMekan;
        List<BlockedUser> blockedUsers = new List<BlockedUser>() { };
        
        
        #endregion
        public LokasyondakiKisilerBaseVC(IntPtr handle) : base(handle)
        {
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TumuButton.TouchUpInside += TumuButton_TouchUpInside;
            CevrimiciButton.TouchUpInside += CevrimiciButton_TouchUpInside;
            BeklenenlerButton.TouchUpInside += BeklenenlerButton_TouchUpInside;
            BackButton.TouchUpInside += BackButton_TouchUpInside;
            BackButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            MesajlarButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
        }

        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }
        bool Actinmi2 = false;
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (!Actinmi2)
            {
                this.View.Hidden = true;
                Actinmi2 = true;
            }
        }
        bool Actinmi = false;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!Actinmi)
            {
                Menuler = new List<UIButton>();
                Menuler.Add(TumuButton);
                Menuler.Add(CevrimiciButton);
                Menuler.Add(BeklenenlerButton);
                TasarimiDuzenle();
                ButtonTasarimlariniDuzenle(0);
                GetBlockedUser();
                Actinmi = true;
            }
           
        }
        #endregion

        #region Process
        void GetBlockedUser()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("blocked-user/block-list");
            if (Donus != null)
            {
                var aa = Donus.ToString();
                blockedUsers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BlockedUser>>(Donus.ToString());
            }
            ClearViewContents();
            this.View.Hidden = false;
            var Populer1 = LokasyondakiKisilerTumu.Create(gelenMekan, this, blockedUsers);
            Populer1.Frame = new CGRect(0, 0, ContentHazne.Frame.Width, ContentHazne.Frame.Height);
            ContentHazne.AddSubview(Populer1);
            LokasyondakiKisilerBaseVC_Kopya.LokasyondakiKisilerBaseVC1 = this;
        }

        private void BeklenenlerButton_TouchUpInside(object sender, EventArgs e)
        {
            ClearViewContents();
            var LokasyondakiKisilerBeklenenler1 = LokasyondakiKisilerBeklenenler.Create(gelenMekan,this);
            LokasyondakiKisilerBeklenenler1.GelenMekan = gelenMekan;
            LokasyondakiKisilerBeklenenler1.Frame = new CGRect(0, 0, ContentHazne.Frame.Width, ContentHazne.Frame.Height);
            ContentHazne.AddSubview(LokasyondakiKisilerBeklenenler1);
            ButtonTasarimlariniDuzenle(2);
        }

        private void CevrimiciButton_TouchUpInside(object sender, EventArgs e)
        {
            ClearViewContents();
            var LokasyondakiKisilerCevrimici1 = LokasyondakiKisilerCevrimici.Create(gelenMekan,this);
            LokasyondakiKisilerCevrimici1.GelenMekan = gelenMekan;
            LokasyondakiKisilerCevrimici1.Frame = new CGRect(0, 0, ContentHazne.Frame.Width, ContentHazne.Frame.Height);
            ContentHazne.AddSubview(LokasyondakiKisilerCevrimici1);
            ButtonTasarimlariniDuzenle(1);
        }

        private void TumuButton_TouchUpInside(object sender, EventArgs e)
        {
            ClearViewContents();
            var LokasyondakiKisilerTumu1 = LokasyondakiKisilerTumu.Create(gelenMekan, this, blockedUsers);
            LokasyondakiKisilerTumu1.GelenMekan = gelenMekan;
            LokasyondakiKisilerTumu1.Frame = new CGRect(0, 0, ContentHazne.Frame.Width, ContentHazne.Frame.Height);
            ContentHazne.AddSubview(LokasyondakiKisilerTumu1);
            ButtonTasarimlariniDuzenle(0);
        }

        void ClearViewContents()
        {
            foreach (var item in ContentHazne.Subviews)
            {
                item.RemoveFromSuperview();
            }

        }
        #endregion

        #region  UI Tasarimlar
        void TasarimiDuzenle()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = HeaderHazne.Frame;
            HeaderHazne.Layer.InsertSublayer(gradientLayer, 0);
            HeaderHazne.Layer.CornerRadius = 30;
            HeaderHazne.ClipsToBounds = true;
            HeaderHazne.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);

            MenuHazne.BackgroundColor = UIColor.White;
            MenuHazne.Layer.CornerRadius = 10f;
            MenuHazne.ClipsToBounds = true;


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


        #region Custom Class
        public static class LokasyondakiKisilerBaseVC_Kopya
        {
            public static LokasyondakiKisilerBaseVC LokasyondakiKisilerBaseVC1 { get; set; }
        }
        public static class SecilenKisi
        {
            public static MEMBER_DATA SecilenKisiDTO { get; set; }
        }
        public class GetBlockedUserDataModel
        {
            public int blockUserId { get; set; }
            public string createdDate { get; set; }
            public int id { get; set; }
            public string lastModifiedDate { get; set; }
            public string reasonType { get; set; }
            public string status { get; set; }
            public int userId { get; set; }
        }
        #endregion
    }
}