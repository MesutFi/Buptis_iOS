using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.PrivateProfile.Ayarlar;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using static Buptis_iOS.ProfilSorulariBaseVC;

namespace Buptis_iOS
{
    public partial class PrivateProfileVC : UIViewController
    {
     
        public PrivateProfileVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            AyarlarButton.TouchUpInside += AyarlarButton_TouchUpInside;
            GeriButton.TouchUpInside += GeriButton_TouchUpInside;
            ProfilDuzenleButton.TouchUpInside += ProfilDuzenleButton_TouchUpInside;
            FotografEkleButton.TouchUpInside += FotografEkleButton_TouchUpInside;
            FiltreButton.TouchUpInside += FiltreButton_TouchUpInside;
            PhotoEditButton.TouchUpInside += PhotoEditButton_TouchUpInside;
        }

        private void PhotoEditButton_TouchUpInside(object sender, EventArgs e)
        {
            var FotografEkle1 = UIStoryboard.FromName("PrivateProfileBaseVC", NSBundle.MainBundle);
            GalleryView controller = FotografEkle1.InstantiateViewController("GalleryView") as GalleryView;
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
            controller.GenelBase = this;
            this.PresentViewController(controller, true, null);
        }

        private void FiltreButton_TouchUpInside(object sender, EventArgs e)
        {
            var FiltreBaseVC = UIStoryboard.FromName("PrivateProfileBaseVC", NSBundle.MainBundle);
            FiltreVC controller = FiltreBaseVC.InstantiateViewController("FiltreVC") as FiltreVC;
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            this.PresentViewController(controller, true, null);
        }

        private void FotografEkleButton_TouchUpInside(object sender, EventArgs e)
        {
            var FotografEkle1 = UIStoryboard.FromName("PrivateProfileBaseVC", NSBundle.MainBundle);
            GalleryView controller = FotografEkle1.InstantiateViewController("GalleryView") as GalleryView;
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
            controller.GenelBase = this;
            this.PresentViewController(controller, true, null);
        }
        private void ProfilDuzenleButton_TouchUpInside(object sender, EventArgs e)
        {
            var PrivateProfileBaseVC1 = UIStoryboard.FromName("PrivateProfileBaseVC", NSBundle.MainBundle);
            ProfilSorulariBaseVC controller = PrivateProfileBaseVC1.InstantiateViewController("ProfilSorulariBaseVC") as ProfilSorulariBaseVC;
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            this.PresentViewController(controller, true, null);
        }
        private void GeriButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }
        private void AyarlarButton_TouchUpInside(object sender, EventArgs e)
        {
            var AyarlarBaseVC1 = UIStoryboard.FromName("AyarlarBaseVC", NSBundle.MainBundle);
            AyarlarBaseVC controller = AyarlarBaseVC1.InstantiateViewController("AyarlarBaseVC") as AyarlarBaseVC;
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            this.PresentViewController(controller, true, null);
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ButtonUIDuzenle(AyarlarButton,UIColor.Clear);
            ButtonUIDuzenle(ProfilDuzenleButton, UIColor.Clear);
            ButtonUIDuzenle(FotografEkleButton, UIColor.White);
            AddHeaderGradient();
            AddButtonGradient();

            UserPhoto.Layer.CornerRadius = UserPhoto.Frame.Height / 2;
            UserPhoto.Layer.BorderColor = UIColor.White.CGColor;
            UserPhoto.Layer.BorderWidth = 5f;
            UserPhoto.ClipsToBounds = true;

            FiltreButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            GeriButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
             
            PaketUIDuzenle();
           
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UserTitle.Text = "";
            UserJob.Text = "";
            UserAbout.Text = "";
            UserLocation.Text = "";
            UserLastLocation.Text = "";
            var marginn = 15;
            AyarlarButton.ContentEdgeInsets = new UIEdgeInsets(marginn, marginn, marginn, marginn);
            FotografEkleButton.ContentEdgeInsets = new UIEdgeInsets(marginn, marginn, marginn, marginn);
            ProfilDuzenleButton.ContentEdgeInsets = new UIEdgeInsets(marginn, marginn, marginn, marginn);
            GetUserInfo();
        }
        void ButtonUIDuzenle(UIButton GelenButon, UIColor BorderColor)
        {
            GelenButon.Layer.CornerRadius = GelenButon.Frame.Height / 2;
            GelenButon.Layer.BorderColor = BorderColor.CGColor;
            GelenButon.Layer.BorderWidth = 3f;
        }
        void AddHeaderGradient()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = new CoreGraphics.CGRect(0,0,UIScreen.MainScreen.Bounds.Width,HeaderHazne.Frame.Height);
            HeaderHazne.Layer.InsertSublayer(gradientLayer, 0);
            HeaderHazne.Layer.CornerRadius = 30;
            HeaderHazne.ClipsToBounds = true;
            HeaderHazne.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);
        }
        void AddButtonGradient()
        {
            var Color1 = UIColor.FromRGB(237, 2, 59).CGColor;
            var Color2 = UIColor.FromRGB(223, 0, 107).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = FotografEkleButton.Frame;
            FotografEkleButton.Layer.InsertSublayer(gradientLayer, 0);
            FotografEkleButton.Layer.CornerRadius = 30;
            FotografEkleButton.ClipsToBounds = true;
        }
        void GetUserAbout()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var MeId = DataBase.MEMBER_DATA_GETIR()[0];
                var Donus = webService.OkuGetir("answers/user/" + MeId.login);
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
                        InvokeOnMainThread(delegate ()
                        {
                            UserAbout.Text = CevaplarBirlesmis;
                        });
                    }
                    else
                    {
                        InvokeOnMainThread(delegate ()
                        {
                            UserAbout.Text = "Diðer kullanýcýlarýn sizi tanýyabilmesi için lütfen profil sorularýný yanýtlayýn.";
                        });
                        
                    }
                }
                else
                {
                    InvokeOnMainThread(delegate ()
                    {
                        UserAbout.Text = "Diðer kullanýcýlarýn sizi tanýyabilmesi için lütfen profil sorularýný yanýtlayýn.";
                    });
                    
                }
            })).Start();
        }
        void GetUserInfo()
        {
            var UserInfo = DataBase.MEMBER_DATA_GETIR();
            if (UserInfo.Count > 0)
            {
                UserTitle.Text = UserInfo[0].firstName + " " + UserInfo[0].lastName.Substring(0, 1) + ". ";
                if (!string.IsNullOrEmpty(UserInfo[0].birthDayDate.ToString()))
                {
                    DateTime zeroTime = new DateTime(1, 1, 1);
                    var Fark = (DateTime.Now - Convert.ToDateTime(UserInfo[0].birthDayDate));
                    UserTitle.Text += ((zeroTime + Fark).Year - 1).ToString();
                }
                UserJob.Text = UserInfo[0].userJob;
                GetUserAbout();
                UserInfo[0].townId = "-1";
                GetUserTown(UserInfo[0].townId.ToString(), UserLocation);
                GetLastCheckin(UserInfo[0].id);
                GetUserImage(UserInfo[0].id);
            }
        }
        public void GetUserImage(int USERID)
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
                            ImageService.Instance.LoadUrl(CDN.CDN_Path + Images[Images.Count - 1].imagePath).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Transform(new CircleTransformation(15, "#FFFFFF")).Into(UserPhoto);
                        }
                    });
                }
            })).Start();
        }
        void GetLastCheckin(int USERID)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {

                WebService webService = new WebService();
                var Donus = webService.OkuGetir("locations/user/" + USERID);
                if (Donus != null)
                {
                    var LasLoc = Newtonsoft.Json.JsonConvert.DeserializeObject<LastLocationDTO>(Donus.ToString());
                    if (!string.IsNullOrEmpty(LasLoc.townId))
                    {
                        var TownID = LasLoc.townId;
                        GetUserTown(TownID, UserLastLocation);
                    }
                    else
                    {
                        InvokeOnMainThread(delegate ()
                        {
                            UserLastLocation.Text = "Henüz check-in yok.";
                        });
                        
                    }

                }
                else
                {
                    InvokeOnMainThread(delegate ()
                    {
                        UserLastLocation.Text = "Henüz check-in yok.";
                    });
                }
            })).Start();

           
        }
        void GetUserTown(string townid, UILabel HangiText)
        {
            if (townid=="-1")
            {
                HangiText.Text = "";
                return;
            }
            else
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    LastLocationDTO lastlocDTO = new LastLocationDTO();
                    var Me = DataBase.MEMBER_DATA_GETIR()[0];
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
                            var CityName = (string)js2["cityName"];

                            InvokeOnMainThread(() =>
                            {
                                HangiText.Text = CityName + ", " + TownName;
                            });
                        }
                        else
                        {
                            InvokeOnMainThread(() =>
                            {
                                HangiText.Text = TownName;
                            });
                        }
                    }
                    else
                    {
                        InvokeOnMainThread(() =>
                        {
                            HangiText.Text = "";
                        });
                    }

                })).Start();
            }
          
        }
        #region PaketIslemler
        void PaketUIDuzenle()
        {
            SetShadow(BoostButton);
            SetShadow(SuperBoostButton);
            SetShadow(KrediButton);

            BoostCountLabel.Layer.CornerRadius = 10f;
            SuperBoostCountLabel.Layer.CornerRadius = 10f;
            KrediCountLabel.Layer.CornerRadius = 10f;
            BoostCountLabel.ClipsToBounds = true;
            SuperBoostCountLabel.ClipsToBounds = true;
            KrediCountLabel.ClipsToBounds = true;


            BoostButton.TouchUpInside += BoostButton_TouchUpInside;
            SuperBoostButton.TouchUpInside += SuperBoostButton_TouchUpInside;
            KrediButton.TouchUpInside += KrediButton_TouchUpInside;
            BuptisGoldButton.TouchUpInside += BuptisGoldButton_TouchUpInside;
            BuptisGoldToggle.TouchUpInside += BuptisGoldToggle_TouchUpInside;
            GetUserLicence();
        }

        private void BuptisGoldToggle_TouchUpInside(object sender, EventArgs e)
        {
            var GoldModal = UIStoryboard.FromName("PaketlerBase", NSBundle.MainBundle);
            BustisGoldBaseVC controller = GoldModal.InstantiateViewController("BustisGoldBaseVC") as BustisGoldBaseVC;
            controller.PrivateProfileVC1 = this;
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
            this.PresentViewController(controller, true, null);
        }

        private void BuptisGoldButton_TouchUpInside(object sender, EventArgs e)
        {
           
            var GoldModal = UIStoryboard.FromName("PaketlerBase", NSBundle.MainBundle);
            BustisGoldBaseVC controller = GoldModal.InstantiateViewController("BustisGoldBaseVC") as BustisGoldBaseVC;
            controller.PrivateProfileVC1 = this;
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
            this.PresentViewController(controller, true, null);
        }

        private void KrediButton_TouchUpInside(object sender, EventArgs e)
        {
            var KrediModal = UIStoryboard.FromName("PaketlerBase", NSBundle.MainBundle);
            KrediYukleBaseVC controller = KrediModal.InstantiateViewController("KrediYukleBaseVC") as KrediYukleBaseVC;
            controller.PrivateProfileVC1 = this;
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
            this.PresentViewController(controller, true, null);
        }

        private void SuperBoostButton_TouchUpInside(object sender, EventArgs e)
        {
            if (SuperBoostCountLabel.Text == "+")
            {
                var SuperBoostModal = UIStoryboard.FromName("PaketlerBase", NSBundle.MainBundle);
                SuperBoostBaseVC controller = SuperBoostModal.InstantiateViewController("SuperBoostBaseVC") as SuperBoostBaseVC;
                controller.PrivateProfileVC1 = this;
                controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                this.PresentViewController(controller, true, null);
            }
            else
            {
                UseBoostOrSuperBoost("SUPER_BOOST");
            }
            
        }

        private void BoostButton_TouchUpInside(object sender, EventArgs e)
        {

            if (BoostCountLabel.Text == "+")
            {
                var BoostModal = UIStoryboard.FromName("PaketlerBase", NSBundle.MainBundle);
                BoostBaseVC controller = BoostModal.InstantiateViewController("BoostBaseVC") as BoostBaseVC;
                controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                this.PresentViewController(controller, true, null);
            }
            else
            {
                UseBoostOrSuperBoost("BOOST");
            }
            
        }
        void UseBoostOrSuperBoost(string LicenceType)
        {
            WebService webService = new WebService();
            var Donus = webService.ServisIslem("licences/use", LicenceType,ContentType: "text/plain");
            if (Donus != "Hata")
            {
                switch (LicenceType)
                {
                    case "SUPER_BOOST":
                        CustomAlert.GetCustomAlert(this, "1 Super Boost Aktifleþtirildi.");
                        break;
                    case "BOOST":
                        CustomAlert.GetCustomAlert(this, "1 Boost Aktifleþtirildi.");
                        break;
                    default:
                        break;
                }

                GetUserLicence();
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Bir sorun oluþtu. Lütfen daha sonra tekrar deneyin.");
            }
        }
        void SetShadow(UIButton GelenBut)
        {
            GelenBut.Layer.CornerRadius = 20f;
            GelenBut.Layer.ShadowOpacity = 0.8f;
            GelenBut.Layer.ShadowOffset = new CGSize(0, 0);
            GelenBut.Layer.ShadowColor = UIColor.Black.CGColor;
            GelenBut.ContentEdgeInsets = new UIEdgeInsets(20, 20, 20, 20);
            GelenBut.ClipsToBounds = true;
        }
        public void GetUserLicence()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                var MeID = DataBase.MEMBER_DATA_GETIR()[0].id;
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("users/" + MeID);
                if (Donus != null)
                {
                    var aa = Donus.ToString();
                    var Icerikk = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(Donus.ToString());
                    if (Icerikk != null)
                    {
                        InvokeOnMainThread(delegate ()
                        {
                            if (Icerikk.boost <= 0)
                            {
                                BoostCountLabel.Text = "+";
                            }
                            else
                            {
                                BoostCountLabel.Text = Icerikk.boost.ToString();
                            }

                            if (Icerikk.superBoost <= 0)
                            {
                                SuperBoostCountLabel.Text = "+";
                            }
                            else
                            {
                                SuperBoostCountLabel.Text = Icerikk.superBoost.ToString();
                            }

                            if (Icerikk.messageCount <= 0 || Icerikk.messageCount == null)
                            {
                                KrediCountLabel.Text = "+";
                            }
                            else
                            {
                                KrediCountLabel.Text = " " + Icerikk.messageCount.ToString() + " ";
                                KrediCountLabel.TranslatesAutoresizingMaskIntoConstraints = false;
                                var Kisitlamalari = KrediCountLabel.Constraints;
                                //KrediCountLabel.RemoveConstraints(KrediCountLabel.Constraints);
                                // KrediCountLabel.Text = " " + "50000" + " ";
                                KrediCountLabel.TextAlignment = UITextAlignment.Center;
                                KrediCountLabel.LayoutIfNeeded();
                                var withh = KrediCountLabel.IntrinsicContentSize.Width;
                                var frmaee = KrediCountLabel.Frame;
                                frmaee.Width = withh;
                                frmaee.X = KrediCountLabel.Frame.Right - frmaee.Width;
                                KrediCountLabel.Frame = frmaee;
                                KrediCountLabel.Layer.CornerRadius = 0f;
                                KrediCountLabel.Layer.CornerRadius = frmaee.Height / 2f;
                                //KrediCountLabel.AddConstraints(Kisitlamalari);
                            }
                            if (Icerikk.gold != null)
                            {
                                BuptisGoldToggle.SetImage(UIImage.FromBundle("Images/gold_acik.png"), UIControlState.Normal);
                            }
                        });
                    }
                }
            })).Start();
        }
        #endregion
        public class UserImageDTO
        {
            public string createdDate { get; set; }
            public int id { get; set; }
            public string imagePath { get; set; }
            public string lastModifiedDate { get; set; }
            public int userId { get; set; }
        }
        public class LastLocationDTO
        {
            public int capacity { get; set; }
            public double coordinateX { get; set; }
            public double coordinateY { get; set; }
            public string createdDate { get; set; }
            public int environment { get; set; }
            public int id { get; set; }
            public string lastModifiedDate { get; set; }
            public string name { get; set; }
            public string place { get; set; }
            public double rating { get; set; }
            public string townId { get; set; }
            public string townName { get; set; }
            public int checkincount { get; set; }
            public string catid { get; set; }
        }

       
    }
}