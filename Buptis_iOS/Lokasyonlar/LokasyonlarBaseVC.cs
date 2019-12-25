
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Buptis_iOS.Database;
using Buptis_iOS.Mesajlar;
using Buptis_iOS.PrivateProfile;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using UIKit;
using static Buptis_iOS.ChatVC;

namespace Buptis_iOS.Lokasyonlar
{
    public partial class LokasyonlarBaseVC : UIViewController
    {

        #region Tanimlamalar
        List<UIButton> Menuler = new List<UIButton>();
        #endregion
        public LokasyonlarBaseVC(IntPtr handle) : base(handle)
        {
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
        }
        bool Actinmi = false;
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            new GetUnReadMessage().GetUnReadMessageCount(MessageCount, this);
            if (!Actinmi)
            {
                MessageCount.Layer.CornerRadius = MessageCount.Frame.Height / 2;
                MessageCount.ClipsToBounds = true;
                MessageCount.Hidden = true;
                Menuler = new List<UIButton>();
                Menuler.Add(BanaYakinButton);
                Menuler.Add(PopulerButton);
                Menuler.Add(BiryersecButton);
                TasarimiDuzenle();
                ButtonTasarimlariniDuzenle(0);
                MesajlarButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
                BanaYakinButton.TouchUpInside += BanaYakinButton_TouchUpInside;
                PopulerButton.TouchUpInside += PopulerButton_TouchUpInside;
                BiryersecButton.TouchUpInside += BiryersecButton_TouchUpInside;
                MyProfileButton.TouchUpInside += MyProfileButton_TouchUpInside;
                MesajlarButton.TouchUpInside += MesajlarButton_TouchUpInside;
                Actinmi = true;
            }
        }
        private void MesajlarButton_TouchUpInside(object sender, EventArgs e)
        {
            var LokasyonKisilerStory = UIStoryboard.FromName("MesajlarBaseVC", NSBundle.MainBundle);
            MesajlarBaseVC controller = LokasyonKisilerStory.InstantiateViewController("MesajlarBaseVC") as MesajlarBaseVC;
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            this.PresentViewController(controller, true, null);
        }

        private void MyProfileButton_TouchUpInside(object sender, EventArgs e)
        {
            var LokasyonKisilerStory = UIStoryboard.FromName("PrivateProfileBaseVC", NSBundle.MainBundle);
            PrivateProfileBaseVC controller = LokasyonKisilerStory.InstantiateViewController("PrivateProfileBaseVC") as PrivateProfileBaseVC;
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            this.PresentViewController(controller, true, null);
        }
        bool Actinmi2 = false;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            
            if (!Actinmi2)
            {
                ClearViewContents();
                var Populer1 = LokasyonlarBanaYakin.Create(this);
                Populer1.Frame = new CGRect(0, 0, ListeHazne.Frame.Width, ListeHazne.Frame.Height);
                ListeHazne.AddSubview(Populer1);
                var User = DataBase.MEMBER_DATA_GETIR()[0];
                GetUserImage(User.id);
                Actinmi2 = true;
            }
        }
        #endregion

        void GetUserImage(int USERID)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("images/user/" + USERID);
                if (Donus != null)
                {
                    InvokeOnMainThread(delegate () {
                        var Images = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserImageDTO>>(Donus.ToString());
                        if (Images.Count > 0)
                        {
                            ImageService.Instance.LoadUrl(CDN.CDN_Path + Images[Images.Count - 1].imagePath).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Transform(new CircleTransformation(15, "#FFFFFF")).Into(MyProfileButton);
                        }
                    });
                }
            })).Start();
        }


        #region Process
        private void BiryersecButton_TouchUpInside(object sender, EventArgs e)
        {
            ButtonTasarimlariniDuzenle(2);
            ClearViewContents();
            var LokasyonlarBiryersec1 = LokasyonlarBiryersec.Create(this);
            LokasyonlarBiryersec1.Frame = new CGRect(0, 0, ListeHazne.Frame.Width, ListeHazne.Frame.Height);
            ListeHazne.AddSubview(LokasyonlarBiryersec1);
        }

        private void PopulerButton_TouchUpInside(object sender, EventArgs e)
        {
            ButtonTasarimlariniDuzenle(1);
            ClearViewContents();
            var Populer1 = LokasyonlarPopuler.Create(this);
            Populer1.Frame = new CGRect(0, 0, ListeHazne.Frame.Width, ListeHazne.Frame.Height);
            ListeHazne.AddSubview(Populer1);
        }

        private void BanaYakinButton_TouchUpInside(object sender, EventArgs e)
        {
            ButtonTasarimlariniDuzenle(0);
            ClearViewContents();
            var LokasyonlarBanaYakin1 = LokasyonlarBanaYakin.Create(this);
            LokasyonlarBanaYakin1.Frame = new CGRect(0, 0, ListeHazne.Frame.Width, ListeHazne.Frame.Height);
            ListeHazne.AddSubview(LokasyonlarBanaYakin1);
        }

        void ClearViewContents()
        {
            foreach (var item in ListeHazne.Subviews)
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
            gradientLayer.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 263f);
            HeaderView.Layer.InsertSublayer(gradientLayer, 0);
            HeaderView.Layer.CornerRadius = 30;
            HeaderView.ClipsToBounds = true;
            HeaderView.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);

            TabMenuHazne.BackgroundColor = UIColor.White;
            TabMenuHazne.Layer.CornerRadius = 10f;
            TabMenuHazne.ClipsToBounds = true;

        }

        void ButtonTasarimlariniDuzenle(int Index)
        {
            for (int i = 0; i < Menuler.Count; i++)
            {
                Menuler[i].BackgroundColor = UIColor.Clear;
                Menuler[i].SetTitleColor(UIColor.Black, UIControlState.Normal);
            }

            //var C1 = UIColor.FromRGB(238, 0, 60).CGColor;
            //var C2 = UIColor.FromRGB(225, 0, 105).CGColor;
            //var viewgradient = new CAGradientLayer();
            //viewgradient.Colors = new CoreGraphics.CGColor[] { C1, C2 };
            //viewgradient.Locations = new NSNumber[] { 0.0, 1.0 };
            //viewgradient.Frame = Menuler[Index].Frame;
            //Menuler[Index].Layer.InsertSublayer(viewgradient, 0);
            Menuler[Index].BackgroundColor = UIColor.FromRGB(226, 0, 93);
            Menuler[Index].Layer.CornerRadius = 10f;
            Menuler[Index].ClipsToBounds = true;
            Menuler[Index].SetTitleColor(UIColor.White, UIControlState.Normal);
        }




        #endregion

        public static class SecilenLokasyonn
        {
            public static string LokID { get; set; }
            public static string LokName { get; set; }
            public static double lat { get; set; }
            public static double lon { get; set; }
            public static string telephone { get; set; }
            public static string Rate { get; set; }

        }
        public class UserImageDTO
        {
            public string createdDate { get; set; }
            public int id { get; set; }
            public string imagePath { get; set; }
            public string lastModifiedDate { get; set; }
            public int userId { get; set; }
        }
    }

    #region GetUnreadcount
    public class GetUnReadMessage
    {
        List<SonMesajlarListViewDataModel> mFriends = new List<SonMesajlarListViewDataModel>();
        public void GetUnReadMessageCount(UILabel CounterText, UIViewController GelenBase)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                #region Message Count
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("chats/user");
                if (Donus != null)
                {
                    mFriends = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SonMesajlarListViewDataModel>>(Donus.ToString());
                    SonMesajKiminKontrolunuYap();
                    var toplam = OkunmaamisSayi(0, mFriends) + OkunmaamisSayi(1, mFriends) + OkunmaamisSayi(2, mFriends);
                    if (toplam > 0)
                    {
                        GelenBase.InvokeOnMainThread(delegate ()
                        {
                            try
                            {
                                if (toplam > 9)
                                {
                                    CounterText.Text = "9+";
                                }
                                else
                                {
                                    CounterText.Text = toplam.ToString();
                                }
                                CounterText.Hidden = false;
                            }
                            catch
                            {

                                CounterText.Hidden = true;
                            }

                        });
                    }
                    else
                    {

                        GelenBase.BeginInvokeOnMainThread(delegate ()
                        {
                            try
                            {
                                CounterText.Hidden = true;
                            }
                            catch
                            {
                                CounterText.Hidden = true;
                            }

                        });
                    }
                }
                #endregion
            })).Start();
        }
        MEMBER_DATA MeData;
        void SonMesajKiminKontrolunuYap()
        {
            MeData = DataBase.MEMBER_DATA_GETIR()[0];
            for (int i = 0; i < mFriends.Count; i++)
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("chats/user/" + mFriends[i].receiverId);
                if (Donus != null)
                {
                    var AA = Donus.ToString(); ;
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
        int OkunmaamisSayi(int ButtonIndex, List<SonMesajlarListViewDataModel> Liste)
        {
            List<SonMesajlarListViewDataModel> Liste2 = new List<SonMesajlarListViewDataModel>();
            int OkunmamisMesajSayisi = 0;
            switch (ButtonIndex)
            {
                case 0:
                    Liste = mFriends.FindAll(item => item.request == false);
                    break;
                case 1:
                    Liste = mFriends.FindAll(item => item.request == true); //Bana Gelen İstekler;
                    break;
                case 2:
                    Liste = mFriends.FindAll(item => item.request == false);
                    Liste = FavorileriAyir(Liste);
                    break;
                default:
                    break;
            }
            Liste.ForEach(item =>
            {
                OkunmamisMesajSayisi += item.unreadMessageCount;
            });

            if (Liste.Count > 0 && OkunmamisMesajSayisi > 0)
            {
                return OkunmamisMesajSayisi;
            }
            else
            {
                return 0;
            }
        }
        List<SonMesajlarListViewDataModel> FavorileriAyir(List<SonMesajlarListViewDataModel> GelenListe)
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

        class SonMesajlarListViewDataModel
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
    #endregion



}