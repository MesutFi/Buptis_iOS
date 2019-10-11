using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Web_Service;

using CoreAnimation;
using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UIKit;
using static Buptis_iOS.LokasyondakiKisiler.LokasyondakiKisilerBaseVC;

namespace Buptis_iOS
{
    public partial class EngelleVC : UIViewController
    {
        EngelleCustomRadioItem[] Noktalar = new EngelleCustomRadioItem[0];
        List<EngelleCustomRadioItem> engelleList = new List<EngelleCustomRadioItem> { };
        public PublicProfileVC BaseVC;
        bool selectButton = true;
        
        string[] Titlesss = new string[]
        {
            "Sahte Profil",
            "Uygunsuz Ýçerik",
            "Kabalýk ve saygýsýzlýk",
            "Spam",
            "Dolandýrýcýlýk",
            "Diðer"
        };

        string[] reasonTypes = new string[]
        {
            "FAKE_ACCOUNT",
            "INAPPROPRIATE_CONTENT",
            "DISRESPECT",
            "SPAM",
            "FRAUD",
            "OTHER"
        };

        int secim;

        public EngelleVC (IntPtr handle) : base (handle)
        {
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            HeaderView.Hidden = true;
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            HeaderTasarim();
            GeriButton.TouchUpInside += GeriButton_TouchUpInside;
            GeriButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);

            HeaderView.Hidden = false;
            GetRadioButtons();
        }

        private void GeriButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }

        void GetRadioButtons()
        {
            Noktalar = new EngelleCustomRadioItem[6];
            for (int i = 0; i < Noktalar.Length; i++)
            {
                var NoktaItem = EngelleCustomRadioItem.Create(Titlesss[i],RadioBUtonlarinTasarimlariniDuzenle, false);

                if (i == 0)
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(0, HeaderView.Frame.Bottom + 20f, UIKit.UIScreen.MainScreen.Bounds.Width, 47f);
                }
                else
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(0, Noktalar[i-1].Frame.Bottom, UIKit.UIScreen.MainScreen.Bounds.Width, 47f);
                }

                this.View.AddSubview(NoktaItem);
                Noktalar[i] = NoktaItem;
               
            }
            AddApplyButton();
        }
        //Custom Button Touch Action
        void RadioBUtonlarinTasarimlariniDuzenle()
        {
            for (int i = 0; i < Noktalar.Length; i++)
            {
                Noktalar[i].UzaktanErisim(false);
            }
        }
        void HeaderTasarim()
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
        }

        void AddApplyButton()
        {
            var OkButton = new UIButton(UIButtonType.Custom)
            {
                Frame = new CoreGraphics.CGRect(0, 0, 180f, 45f),
            };
            OkButton.SetTitle("Engelle", UIControlState.Normal);
            OkButton.SetTitleColor(UIColor.White, UIControlState.Normal);

            var newFramee = OkButton.Frame;
            newFramee.X = (UIScreen.MainScreen.Bounds.Width / 2) - (newFramee.Width / 2);
            newFramee.Y = Noktalar[Noktalar.Length - 1].Frame.Bottom + 40f;
            OkButton.Frame = newFramee;
            this.View.AddSubview(OkButton);

            OkButton.BackgroundColor = UIColor.FromRGB(225, 0, 105);
            OkButton.Layer.CornerRadius = OkButton.Frame.Height / 2;
            OkButton.ClipsToBounds = true;
            OkButton.TouchUpInside += OkButton_TouchUpInside;
        }

        private void OkButton_TouchUpInside(object sender, EventArgs e)
        {

            var ArrayToList = new List<EngelleCustomRadioItem>(Noktalar);
            var SecilenIndex = ArrayToList.FindIndex(item => item.isSelect == true);
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    WebService webService = new WebService();
                    BlockedUser blockedUser = null;
                    InvokeOnMainThread(delegate ()
                    {
                        string reasonTypee = "OTHER";
                        if (SecilenIndex != -1)
                        {
                            reasonTypee = reasonTypes[SecilenIndex];
                        }

                        blockedUser = new BlockedUser()
                        {
                            reasonType = reasonTypee,
                            blockUserId = SecilenKisi.SecilenKisiDTO.id,
                            userId = DataBase.MEMBER_DATA_GETIR()[0].id,
                            status="BLOCKED"
                        };
                    });

                    string jsonString = JsonConvert.SerializeObject(blockedUser);
                    var Responsee = webService.ServisIslem("blocked-users", jsonString);
                    if (Responsee != "Hata")
                    {
                        InvokeOnMainThread(delegate ()
                        {
                           // CustomAlert.GetCustomAlert(this, SecilenKisi.SecilenKisiDTO.firstName + " Engellendi.");
                            this.DismissViewController(true, null);
                            BaseVC.DismissViewController(true, null);
                        });

                    }
                   
                })).Start();
            
        }

        public class BlockedUser
        {
            public int blockUserId { get; set; }
            public string createdDate { get; set; }
            public string id { get; set; }
            public string lastModifiedDate { get; set; }
            public string reasonType { get; set; }
            public string status { get; set; }
            public int userId { get; set; }
        }
    }
}