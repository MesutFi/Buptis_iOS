using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.PublicProfile;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using static Buptis_iOS.LokasyondakiKisiler.LokasyondakiKisilerBaseVC;
using static Buptis_iOS.Mesajlar.MesajlarBaseVC;
using static Buptis_iOS.ProfilSorulariBaseVC;

namespace Buptis_iOS
{
    public partial class PublicProfileVC : UIViewController
    {
        #region Tanimlamalar
        PublicProfilePhotoView[] Noktalar = new PublicProfilePhotoView[0];
        PublicProfileDataModel UserDatas = new PublicProfileDataModel();
        List<UserAnswerDataModel> UserAnswers = new List<UserAnswerDataModel>();
        GetUserLastLocation userlastloc = new GetUserLastLocation();
        List<UserGalleryDataModel> PhotoList = new List<UserGalleryDataModel>();
        List<string> FollowListID = new List<string>();
        public PublicProfileBaseVC GelenBase;
        #endregion
        public PublicProfileVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            GeriButton.TouchUpInside += GeriButton_TouchUpInside;
            MesajAtButton.TouchUpInside += MesajAtButton_TouchUpInside;
            ScrollFotograf.Scrolled += ScrollFotograf_Scrolled;
            EngelleButton.TouchUpInside += EngelleButton_TouchUpInside;
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                GetUserInfo();
                BekletVeGuncelle();
            })).Start();
            FavoriEkleButton.TouchUpInside += FavoriEkleButton_TouchUpInside;
        }

        private void FavoriEkleButton_TouchUpInside(object sender, EventArgs e)
        {
            var IsFollow = FollowListID.FindAll(item => item == SecilenKisi.SecilenKisiDTO.id.ToString());
            if (IsFollow.Count > 0)
            {

                UIAlertView alert = new UIAlertView();
                alert.Title = "Buptis";
                alert.AddButton("Evet");
                alert.AddButton("Hayýr");
                alert.Message = SecilenKisi.SecilenKisiDTO.firstName + " adlý kullanýcýyý favorilerilerinden çýkartmak istediðini emin misiniz?";
                alert.AlertViewStyle = UIAlertViewStyle.Default;
                alert.Clicked += (object s, UIButtonEventArgs ev) =>
                {
                    if (ev.ButtonIndex == 0)
                    {
                        alert.Dispose();
                    }
                    else
                    {
                        alert.Dispose();
                        FavoriIslemleri(SecilenKisi.SecilenKisiDTO.firstName + " Favorilerinde çýkarýldý.");
                    }
                };
                alert.Show();
            }
            else
            {
                FavoriIslemleri(SecilenKisi.SecilenKisiDTO.firstName + " Favorilerine eklendi.");
            }
        }
        void FavoriIslemleri(string Message)
        {
            var MeID = DataBase.MEMBER_DATA_GETIR()[0].id;
            WebService webService = new WebService();
            FavoriDTO favoriDTO = new FavoriDTO()
            {
                userId = MeID,
                favUserId = SecilenKisi.SecilenKisiDTO.id
            };
            string jsonString = JsonConvert.SerializeObject(favoriDTO);
            var Donus = webService.ServisIslem("users/fav", jsonString);
            if (Donus != "Hata")
            {
                CustomAlert.GetCustomAlert(this, Message);
               
                GetFavorite();
                return;
            }
        }
        void GetFavorite()
        {
            InvokeOnMainThread(delegate ()
            {
                WebService webService = new WebService();
                var MeID = DataBase.MEMBER_DATA_GETIR()[0].id;
                var Donus4 = webService.OkuGetir("users/favList/" + MeID.ToString());
                if (Donus4 != null)
                {
                    var JSONStringg = Donus4.ToString().Replace("[", "").Replace("]", "");
                    if (!string.IsNullOrEmpty(JSONStringg))
                    {
                        FollowListID = JSONStringg.Split(',').ToList();
                    }
                }
                else
                {
                }
            });
        }
        private void EngelleButton_TouchUpInside(object sender, EventArgs e)
        {
            var PublicProfileBaseVC1 = UIStoryboard.FromName("PublicProfileBaseVC", NSBundle.MainBundle);
            EngelleVC controller = PublicProfileBaseVC1.InstantiateViewController("EngelleVC") as EngelleVC;
            controller.BaseVC = this;
            this.PresentViewController(controller, true, null);
        }

        private void ScrollFotograf_Scrolled(object sender, EventArgs e)
        {
            var PageeIndex = (nint)(ScrollFotograf.ContentOffset.X / ScrollFotograf.Frame.Width);
            PageControll.CurrentPage = PageeIndex;
        }

        private void MesajAtButton_TouchUpInside(object sender, EventArgs e)
        {
            MesajlarIcinSecilenKullanici.Kullanici = SecilenKisi.SecilenKisiDTO;
            var mesKey = GetMessageKey(MesajlarIcinSecilenKullanici.Kullanici.id);
            MesajlarIcinSecilenKullanici.key = mesKey;

            var LokasyonKisilerStory = UIStoryboard.FromName("MesajlarBaseVC", NSBundle.MainBundle);
            ChatVC controller = LokasyonKisilerStory.InstantiateViewController("ChatVC") as ChatVC;
            this.PresentViewController(controller, true, null);
        }
        string GetMessageKey(int UserId)
        {
            var MessageKey = DataBase.CHAT_KEYS_GETIR();
            if (MessageKey.Count > 0)
            {
                MessageKey = MessageKey.FindAll(item => item.UserID == UserId);
                if (MessageKey.Count > 0)
                {
                    return MessageKey[MessageKey.Count - 1].MessageKey;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        private void GeriButton_TouchUpInside(object sender, EventArgs e)
        {
            GelenBase.Closee();
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            KullaniciAdiYasi.Text = "";
            HakkindaYazisi.Text = "";
            EnSonLokasyonu.Text = "";
              var marginn = 17;
            MesajAtButton.ContentEdgeInsets = new UIEdgeInsets(marginn, marginn, marginn, 20);
           
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        
            
            ButtonUIDuzenle(MesajAtButton, UIColor.White);
            // ButtonUIDuzenle(FavoriEkleButton, UIColor.White);
            FavoriEkleButton.Layer.CornerRadius = FavoriEkleButton.Frame.Height / 2;
            FavoriEkleButton.Layer.BorderColor = UIColor.White.CGColor;
            FavoriEkleButton.Layer.BorderWidth = 2;
            FavoriEkleButton.ClipsToBounds = true;
            FavoriEkleButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            AddHeaderCorner();
            GeriButtonTint();
        }
        void ButtonUIDuzenle(UIButton GelenButon, UIColor BorderColor)
        {
            GelenButon.Layer.CornerRadius = GelenButon.Frame.Height / 2;
            GelenButon.Layer.BorderColor = BorderColor.CGColor;
            GelenButon.Layer.BorderWidth = 3f;
            var Color1 = UIColor.FromRGB(237, 2, 59).CGColor;
            var Color2 = UIColor.FromRGB(223, 0, 107).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = GelenButon.Frame;
            GelenButon.Layer.InsertSublayer(gradientLayer, 0);
        }
        void AddHeaderCorner()
        {
            ScrollFotograf.Layer.CornerRadius = 30;
            ScrollFotograf.ClipsToBounds = true;
            ScrollFotograf.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);
        }
        void GeriButtonTint()
        {
            var IconImage = GeriButton.ImageView.Image.ImageWithAlignmentRectInsets(new UIEdgeInsets(-5, -5, -5, -5));
            var TintImage = IconImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            GeriButton.ImageView.Image = TintImage;
            GeriButton.ImageView.TintColor = UIColor.FromRGB(224, 0, 108);
        }
        void GetUserPhotos()
        {
            var Yukseklik = ScrollFotograf.Frame.Height;

            WebService webService = new WebService();
            InvokeOnMainThread(delegate ()
            {
                var Donus1 = webService.OkuGetir("images/user/"+SecilenKisi.SecilenKisiDTO.id);
                PhotoList.Reverse();
                if (Donus1 != null)
                {
                    PhotoList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserGalleryDataModel>>(Donus1.ToString());
                    if (PhotoList.Count>0)
                    {
                        if (PhotoList.Count<=10)
                        {
                            Noktalar = new PublicProfilePhotoView[PhotoList.Count];
                            for (int i = 0; i < PhotoList.Count; i++)
                            {
                                var NoktaItem = PublicProfilePhotoView.Create(PhotoList[i]);

                                if (i == 0)
                                {
                                    NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, Yukseklik);
                                }
                                else
                                {
                                    NoktaItem.Frame = new CoreGraphics.CGRect(UIKit.UIScreen.MainScreen.Bounds.Width * i, 0, UIKit.UIScreen.MainScreen.Bounds.Width, Yukseklik);
                                }

                                ScrollFotograf.AddSubview(NoktaItem);
                                Noktalar[i] = NoktaItem;
                            }
                            ScrollFotograf.ContentSize = new CoreGraphics.CGSize(Noktalar[Noktalar.Length - 1].Frame.Right, Yukseklik);
                            ScrollFotograf.PagingEnabled = true;
                            PageControll.Pages = Noktalar.Length;
                        }
                        else
                        {
                            Noktalar = new PublicProfilePhotoView[PhotoList.Count];
                            for (int i = 0; i < 10; i++)
                            {
                                var NoktaItem = PublicProfilePhotoView.Create(PhotoList[i]);

                                if (i == 0)
                                {
                                    NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, Yukseklik);
                                }
                                else
                                {
                                    NoktaItem.Frame = new CoreGraphics.CGRect(UIKit.UIScreen.MainScreen.Bounds.Width * i, 0, UIKit.UIScreen.MainScreen.Bounds.Width, Yukseklik);
                                }

                                ScrollFotograf.AddSubview(NoktaItem);
                                Noktalar[i] = NoktaItem;
                            }
                            ScrollFotograf.ContentSize = new CoreGraphics.CGSize(Noktalar[Noktalar.Length - 1].Frame.Right, Yukseklik);
                            ScrollFotograf.PagingEnabled = true;
                            PageControll.Pages = Noktalar.Length;
                        }
                        
                    }
                    else
                    {
                        Noktalar = new PublicProfilePhotoView[1];
                        for (int i = 0; i < 1; i++)
                        {
                            var NoktaItem = PublicProfilePhotoView.Create(new UserGalleryDataModel() { imagePath=""});

                            if (i == 0)
                            {
                                NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, Yukseklik);
                            }
                            else
                            {
                                NoktaItem.Frame = new CoreGraphics.CGRect(UIKit.UIScreen.MainScreen.Bounds.Width * i, 0, UIKit.UIScreen.MainScreen.Bounds.Width, Yukseklik);
                            }

                            ScrollFotograf.AddSubview(NoktaItem);
                            Noktalar[i] = NoktaItem;
                        }
                        ScrollFotograf.ContentSize = new CoreGraphics.CGSize(Noktalar[Noktalar.Length - 1].Frame.Right, Yukseklik);
                        ScrollFotograf.PagingEnabled = true;
                        PageControll.Pages = Noktalar.Length;
                    }

                }
                else
                {
                    Noktalar = new PublicProfilePhotoView[1];
                    for (int i = 0; i < 1; i++)
                    {
                        var NoktaItem = PublicProfilePhotoView.Create(new UserGalleryDataModel() { imagePath = "" });

                        if (i == 0)
                        {
                            NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, Yukseklik);
                        }
                        else
                        {
                            NoktaItem.Frame = new CoreGraphics.CGRect(UIKit.UIScreen.MainScreen.Bounds.Width * i, 0, UIKit.UIScreen.MainScreen.Bounds.Width, Yukseklik);
                        }

                        ScrollFotograf.AddSubview(NoktaItem);
                        Noktalar[i] = NoktaItem;
                    }
                    ScrollFotograf.ContentSize = new CoreGraphics.CGSize(Noktalar[Noktalar.Length - 1].Frame.Right, Yukseklik);
                    ScrollFotograf.PagingEnabled = true;
                    PageControll.Pages = Noktalar.Length;
                    
                }
            });
           
         
        }
        void GetUserInfo()
        {
            WebService webService = new WebService();
            InvokeOnMainThread(delegate ()
            {
                var Donus1 = webService.OkuGetir("users/" + SecilenKisi.SecilenKisiDTO.id);
                if (Donus1 != null)
                {
                    UserDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<PublicProfileDataModel>(Donus1.ToString());
                    if (UserDatas.birthDayDate==null || UserDatas.birthDayDate == String.Empty)
                    {
                        KullaniciAdiYasi.Text = UserDatas.firstName + " " + UserDatas.lastName.Substring(0, 1) + ". ";
                        KullaniciAdiYasi.Text += "";
                       
                    }
                    else
                    {
                        DateTime zeroTime = new DateTime(1, 1, 1);
                        var Fark = (DateTime.Now - Convert.ToDateTime(UserDatas.birthDayDate));

                        KullaniciAdiYasi.Text = UserDatas.firstName + " " + UserDatas.lastName.Substring(0, 1) + ". ";
                        KullaniciAdiYasi.Text += ((zeroTime + Fark).Year - 1).ToString();
                    }

                }
            });
            var abouttxt = GetUserAbout();
            InvokeOnMainThread(delegate ()
            {
                HakkindaYazisi.Text = abouttxt;
            });

            InvokeOnMainThread(delegate ()
            {
                var Donus3 = webService.OkuGetir("locations/user/" + SecilenKisi.SecilenKisiDTO.id);
                if (Donus3 != null)
                {
                    userlastloc = Newtonsoft.Json.JsonConvert.DeserializeObject<GetUserLastLocation>(Donus3.ToString());
                    GetUserTown(userlastloc.townId, EnSonLokasyonu);
                }
                else
                {
                    EnSonLokasyonu.Text = "Henüz Check-in yapýlmadý.";
                }
            });
            GetFavorite();
        }
        string GetUserAbout()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("answers/user/all");
            if (Donus != null)
            {
                string CevaplarBirlesmis = "";
                var Cevaplar = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserAnswersDTO>>(Donus.ToString());
                if (Cevaplar.Count > 0)
                {
                    for (int i = 0; i < Cevaplar.Count; i++)
                    {
                        CevaplarBirlesmis += Cevaplar[i].option + ", ";
                    }

                    return CevaplarBirlesmis;
                }
                else
                {
                    return "Henüz bilgi yok.";
                }

            }
            else
            {
                return "Henüz bilgi yok.";
            }

        }
        void GetUserTown(string townid, UILabel HangiText)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus1 = webService.OkuGetir("towns/" + townid.ToString());
                if (Donus1 != null)
                {
                    JObject js = JObject.Parse(Donus1.ToString());
                    var TownName = (string)js["townName"];
                    var CityID = (string)js["cityId"];
                    var Donus2 = webService.OkuGetir("cities/ " + CityID.ToString());
                    if (Donus2 != null)
                    {
                        JObject js2 = JObject.Parse(Donus2.ToString());
                        var CityName = js2["cityName"];
                        this.InvokeOnMainThread(() => {
                            HangiText.Text = CityName + ", " + TownName;
                        });
                    }
                    else
                    {
                        this.InvokeOnMainThread(() => {
                            HangiText.Text = TownName;
                        });
                    }
                }
                else
                {
                    this.InvokeOnMainThread(() => {
                        HangiText.Text = "";
                    });
                }

            })).Start();
        }
        void BekletVeGuncelle()
        {
            Task.Run(async delegate () {
                await Task.Delay(500);

                InvokeOnMainThread(delegate () {
                    GetUserPhotos();
                });

            });
        }
        public class PublicProfileDataModel
        {
            public bool activated { get; set; }
            public string birthDayDate { get; set; }
            public string createdBy { get; set; }
            public string createdDate { get; set; }
            public string email { get; set; }
            public string firstName { get; set; }
            public int id { get; set; }
            public string imageUrl { get; set; }
            public string langKey { get; set; }
            public string lastModifiedBy { get; set; }
            public string lastModifiedDate { get; set; }
            public string lastName { get; set; }
            public string login { get; set; }
        }
        public class UserAnswerDataModel
        {
            public int id { get; set; }
            public string option { get; set; }
            public int questionId { get; set; }
        }
        public class GetUserLastLocation
        {
            public int allUserCheckIn { get; set; }
            public int capacity { get; set; }
            public double coordinateX { get; set; }
            public double coordinateY { get; set; }
            public string createdDate { get; set; }
            public int environment { get; set; }
            public int id { get; set; }
            public string lastModifiedDate { get; set; }
            public string name { get; set; }
            public string place { get; set; }
            public string rating { get; set; }
            public string telephone { get; set; }
            public string townId { get; set; }
            public string townName { get; set; }
        }
        public class UserGalleryDataModel
        {
            public string createdDate { get; set; }
            public int id { get; set; }
            public string imagePath { get; set; }
            public string lastModifiedDate { get; set; }
            public int userId { get; set; }
        }
        public class FavoriDTO
        {
            public int favUserId { get; set; }
            public int userId { get; set; }
        }

    }
}