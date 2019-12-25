
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Buptis_iOS.Database;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using static Buptis_iOS.ChatVC;

namespace Buptis_iOS.Mesajlar
{
    public partial class MesajlarBaseVC : UIViewController
    {
        #region Tanimlamalar
        List<UIButton> Menuler = new List<UIButton>();
        List<MesajKisileri> mFriends = new List<MesajKisileri>();
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
            MeData = DataBase.MEMBER_DATA_GETIR()[0];
            AraText.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
            GetUnReadMessage();
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


        #region Okunmamis Mesaj Sayisi
        void GetUnReadMessage()
        {
            #region Message Count
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("chats/user");
            if (Donus != null)
            {
                mFriends = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MesajKisileri>>(Donus.ToString());
                SonMesajKiminKontrolunuYap();
                TitleGuncelle(MesajlarButton, 0, mFriends);
                TitleGuncelle(IsteklerButton, 1, mFriends);
                TitleGuncelle(FavorilerButton, 2, mFriends);
            }
            #endregion
        }

        void TitleGuncelle(UIButton GelenButton, int ButtonIndex, List<MesajKisileri> Liste)
        {
            List<MesajKisileri> Liste2 = new List<MesajKisileri>();
            int OkunmamisMesajSayisi = 0;
            string Baslik = "";
            switch (ButtonIndex)
            {
                case 0:
                    Liste = mFriends.FindAll(item => item.request == false);
                    Baslik = "Mesajlar";
                    break;
                case 1:
                    Liste = mFriends.FindAll(item => item.request == true); //Bana Gelen İstekler;
                    Baslik = "İstekler";
                    break;
                case 2:
                    Liste = mFriends.FindAll(item => item.request == false);
                    Liste = FavorileriAyir(Liste);
                    Baslik = "Favoriler";
                    break;
                default:
                    break;
            }
            Liste.ForEach(item =>
            {
                OkunmamisMesajSayisi += item.unreadMessageCount;
            });

            if (Liste.Count > 0)
            {
                GelenButton.SetTitle(Baslik + " (" + OkunmamisMesajSayisi + ")", UIControlState.Normal);
            }
            else
            {
                GelenButton.SetTitle(Baslik, UIControlState.Normal);
            }
        }

        MEMBER_DATA MeData;
        void SonMesajKiminKontrolunuYap()
        {
            for (int i = 0; i < mFriends.Count; i++)
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("chats/user/" + mFriends[i].receiverId);
                if (Donus != null)
                {
                    var AA = Donus.ToString();
                    var NewChatList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChatDetayDTO>>(Donus.ToString());
                    if (NewChatList.Count > 0)//chatList
                    {

                        if (NewChatList[0].userId == MeData.id)
                        {
                            mFriends[i].unreadMessageCount = 0;
                        }
                    }
                }
            }
        }

        List<MesajKisileri> FavorileriAyir(List<MesajKisileri> GelenListe)
        {
            var FavList = FavorileriCagir();
            List<FavListDTO> newList = new List<FavListDTO>();
            for (int i = 0; i < FavList.Count; i++)
            {
                newList.Add(new FavListDTO()
                {
                    FavUserID = Convert.ToInt32(FavList[i])
                });
            }
            var Ayiklanmis = (from list1 in GelenListe
                              join list2 in newList
                              on list1.receiverId equals list2.FavUserID
                              select list1).ToList();
            return Ayiklanmis;
        }
        List<string> FavorileriCagir()
        {
            List<string> FollowListID = new List<string>();
            WebService webService = new WebService();
            var MeDTO = DataBase.MEMBER_DATA_GETIR()[0];
            var Donus4 = webService.OkuGetir("users/favList/" + MeDTO.id.ToString());
            if (Donus4 != null)
            {
                var JSONStringg = Donus4.ToString().Replace("[", "").Replace("]", "");
                if (!string.IsNullOrEmpty(JSONStringg))
                {
                    FollowListID = JSONStringg.Split(',').ToList();
                }
            }
            return FollowListID;
        }
        public class FavListDTO
        {
            public int FavUserID { get; set; }
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
            gradientLayer.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 180f);
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
            public DateTime lastModifiedDate { get; set; }
            public string lastName { get; set; }
            public int receiverId { get; set; }
            public bool request { get; set; }
            public int unreadMessageCount { get; set; }
            //Custom Property
            public bool BoostOrSuperBoost { get; set; }

        }
    }
}