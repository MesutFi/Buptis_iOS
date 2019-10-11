﻿
using System;
using System.Drawing;
using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Lokasyonlar;
using Buptis_iOS.Web_Service;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIKit;
using Xamarin.Auth;
using static Buptis_iOS.KayitVC;

namespace Buptis_iOS.GirisKayit
{
    public partial class GirisVC : UIViewController
    {
        const string TAG = "Buptis";
        const int RC_SIGN_IN = 9001;
       
        public GirisVC(IntPtr handle) : base(handle)
        {
        }

        #region View lifecycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SifreTxt.SecureTextEntry = true;
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.View.BackgroundColor = UIColor.Clear;
            UyeOlButton.TouchUpInside += UyeOlButton_TouchUpInside;
            GirisButton.TouchUpInside += GirisButton_TouchUpInside;
            FacebookButton.TouchUpInside += FacebookButton_TouchUpInside;
        }
        UIViewController FacebookVC;
        private void FacebookButton_TouchUpInside(object sender, EventArgs e)
        {
            var auth = new OAuth2Authenticator(
               clientId: "371724660433864",
               scope: "email",
               authorizeUrl: new System.Uri("https://m.facebook.com/dialog/oauth/"),
               redirectUrl: new System.Uri("https://www.facebook.com/connect/login_success.html"));
            auth.Completed += Auth_Completed;
            FacebookVC = auth.GetUI();
            this.PresentViewController(FacebookVC, true, null);
        }

        private void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                FacebookVC.DismissViewController(true, null);
                //CustomAlert.GetCustomAlert(this, "Lütfen Bekleyin...");
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    var authenticator = sender as OAuth2Authenticator;
                    if (authenticator.AuthorizeUrl.Host == "m.facebook.com")
                    {
                        FacebookEmail facebookEmail = null;

                        WebService webService = new WebService();
                        var FacebookDonus = webService.OkuGetir($"https://graph.facebook.com/me?fields=id,name,first_name,last_name,email,picture.type(large)&access_token=" + e.Account.Properties["access_token"], true);
                        if (FacebookDonus != null)
                        {
                            var Durum = FacebookDonus.ToString();
                            facebookEmail = JsonConvert.DeserializeObject<FacebookEmail>(FacebookDonus.ToString());

                            #region FaceBook Login With Out API
                            string Ad = "", Soyad = "", email, sifre;
                            var parcala = facebookEmail.name.Split(' ');
                            for (int i = 0; i < parcala.Length; i++)
                            {
                                if (i == 0)
                                {
                                    Ad = parcala[0];
                                }
                                else
                                {
                                    Soyad += parcala[1];
                                }
                            }
                            email = facebookEmail.email;
                            sifre = "Buptis2019@@";
                            SosyalKullaniciKaydet(Ad, Soyad, sifre, email);
                            #endregion
                        }
                    }
                })).Start();
            }
        }
        void SosyalKullaniciKaydet(string AdText, string SoyadText, string Sifre, string email)
        {
            WebService webService = new WebService();
            KayitIcinRoot kayitIcinRoot = new KayitIcinRoot()
            {
                firstName = AdText.Trim(),
                lastName = SoyadText.Trim(),
                password = Sifre,
                login = email,
                email = email
            };
            string jsonString = JsonConvert.SerializeObject(kayitIcinRoot);
            var Responsee = webService.ServisIslem("register", jsonString, true);
            GirisYapMetod(email, Sifre);
            //if (Responsee != "Hata")
            //{
            //}
            //else
            //{
            //    CustomAlert.GetCustomAlert(this,"Bir sorun oluştu lütfen internet bağlantınızı kontrol edin.");
            //    return;
            //}
        }


        void GirisYapMetod(string email, string sifre)
        {
            CustomLoading.Show(this, "Lütfen Bekleyin...");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                LoginRoot loginRoot = null;
                InvokeOnMainThread(delegate () {

                    loginRoot = new LoginRoot()
                    {
                        password = sifre,
                        rememberMe = true,
                        username = email
                    };
                });

                string jsonString = JsonConvert.SerializeObject(loginRoot);
                var Donus = webService.ServisIslem("authenticate", jsonString, true);
                if (Donus == "Hata")
                {
                    CustomLoading.Hide();
                    CustomAlert.GetCustomAlert(this, "Giriş Yapılamadı!");
                    return;
                }
                else
                {
                    JObject obj = JObject.Parse(Donus);
                    string Token = (string)obj["id_token"];
                    if (Token != null && Token != "")
                    {
                        APITOKEN.TOKEN = Token;
                        if (GetMemberData())
                        {
                            CustomLoading.Hide();
                            InvokeOnMainThread(delegate () {
                                var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
                                appDelegate.SetRootLokasyonlarViewController();
                            });
                        }
                    }
                }
            })).Start();
        }


        private void GirisButton_TouchUpInside(object sender, EventArgs e)
        {
            if (BosVarmi())
            {
                GirisYapMetod(EmailTxt.Text.Trim(), SifreTxt.Text);
            }
        }

        private void UyeOlButton_TouchUpInside(object sender, EventArgs e)
        {
            //var story = UIStoryboard.FromName("GenericUI", NSBundle.MainBundle);
            //PopUp controller = story.InstantiateViewController("PopUp") as PopUp;
            //controller.ModalPresentationStyle = UIModalPresentationStyle.BlurOverFullScreen;
            //controller.InitVC("title", "title", "title", "title");
            //this.PresentViewController(controller, true, null);

            KayitVC controller = this.Storyboard.InstantiateViewController("KayitVC") as KayitVC;
            this.PresentViewController(controller, true, null);
        }

        bool Acildimi = false;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!Acildimi)
            {
                TasarimlariOlustur();
                Acildimi = true;
            }
            EmailTxt.Text = "mesut@intellifi.tech";
            SifreTxt.Text = "qwer1234";

        }
        #endregion

        bool GetMemberData()
        {
            WebService webService = new WebService();
            var JSONData = webService.OkuGetir("account");
            if (JSONData != null)
            {
                var JsonSting = JSONData.ToString();

                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(JSONData.ToString());
                Icerik.API_TOKEN = APITOKEN.TOKEN;
                InvokeOnMainThread(delegate () {
                    Icerik.password = SifreTxt.Text;
                });
                DataBase.MEMBER_DATA_TEMIZLE();
                DataBase.MEMBER_DATA_EKLE(Icerik);
                return true;
            }
            else
            {
                return false;
            }
        }
        bool BosVarmi()
        {
            if (EmailTxt.Text.Trim() == "")
            {
                CustomAlert.GetCustomAlert(this, "Lütfen Emailinizi Girin");
                return false;
            }
            else
            {
                if (SifreTxt.Text.Trim() == "")
                {
                    CustomAlert.GetCustomAlert(this, "Lütfen Şifrenizi yazın");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #region UI Tasarim
        void TasarimlariOlustur()
        {
            EmailTxt.BackgroundColor = UIColor.White.ColorWithAlpha(0.33f);
            EmailTxt.Layer.CornerRadius = EmailTxt.Frame.Height / 2;
            EmailTxt.TextColor = UIColor.White;
            EmailTxt.AttributedPlaceholder = new NSAttributedString("Mail / Telefon Numarası", null, UIColor.White);
            UIView paddingView = new UIView(new CGRect(0, 0, 15f, EmailTxt.Frame.Height));
            paddingView.BackgroundColor = UIColor.Clear;
            EmailTxt.LeftView = paddingView;
            EmailTxt.LeftViewMode = UITextFieldViewMode.Always;

            SifreTxt.BackgroundColor = UIColor.White.ColorWithAlpha(0.33f);
            SifreTxt.Layer.CornerRadius = SifreTxt.Frame.Height / 2;
            SifreTxt.TextColor = UIColor.White;
            SifreTxt.AttributedPlaceholder = new NSAttributedString("Şifre", null, UIColor.White);
            UIView paddingView2 = new UIView(new CGRect(0, 0, 15f, SifreTxt.Frame.Height));
            paddingView2.BackgroundColor = UIColor.Clear;
            SifreTxt.LeftView = paddingView2;
            SifreTxt.LeftViewMode = UITextFieldViewMode.Always;
            SifreTxt.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
            EmailTxt.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };


            ButtonBg(GirisButton);
            ButtonBg(GoogleButton);
            ButtonBg(FacebookButton);
        }

        void ButtonBg(UIButton GelenButton)
        {
            GelenButton.BackgroundColor = UIColor.Clear;
            GelenButton.Layer.CornerRadius = GelenButton.Frame.Height / 2;
            GelenButton.Layer.BorderWidth = 2f;
            GelenButton.Layer.BorderColor = UIColor.White.ColorWithAlpha(0.33f).CGColor;
        }

        #endregion

        #region Facebook Login DTO

        public class Data
        {
            public int height { get; set; }
            public bool is_silhouette { get; set; }
            public string url { get; set; }
            public int width { get; set; }
        }

        public class Picture
        {
            public Data data { get; set; }
        }

        public class FacebookEmail
        {
            public string email { get; set; }
            public string first_name { get; set; }
            public string id { get; set; }
            public string last_name { get; set; }
            public string name { get; set; }
            public Picture picture { get; set; }
        }
        #endregion
    }
}