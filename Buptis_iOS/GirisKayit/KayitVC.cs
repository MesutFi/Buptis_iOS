using Buptis_iOS.Database;
using Buptis_iOS.Lokasyonlar;
using Buptis_iOS.Web_Service;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UIKit;
using CoreAnimation;
using Buptis_iOS.GenericClass;
using System.Text.RegularExpressions;

namespace Buptis_iOS
{
    public partial class KayitVC : UIViewController
    {
        public KayitVC (IntPtr handle) : base (handle)
        {

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
        }
        //public void GetAnimated()
        //{
        //    layer = new CALayer();
        //    layer.Bounds = new CGRect(0, 0, 50, 50);
        //    layer.Position = new CGPoint(UIScreen.MainScreen.Bounds.Width / 2, UIScreen.MainScreen.Bounds.Height / 2);
        //    layer.Contents = UIImage.FromFile("Images/loading.png").CGImage;
        //    layer.ContentsGravity = CALayer.GravityResizeAspectFill;
        //    View.Layer.AddSublayer(layer);

        //    layer.Transform = CATransform3D.MakeRotation((float)Math.PI * 2, 0, 0, 1);

        //    CAKeyFrameAnimation animRotate = (CAKeyFrameAnimation)CAKeyFrameAnimation.FromKeyPath("transform");

        //    animRotate.Values = new NSObject[] {
        //    NSNumber.FromFloat (0f),
        //    NSNumber.FromFloat ((float)Math.PI / 2f),
        //    NSNumber.FromFloat ((float)Math.PI),
        //    NSNumber.FromFloat ((float)Math.PI * 2)};

        //    animRotate.ValueFunction = CAValueFunction.FromName(CAValueFunction.RotateX);

        //    animRotate.Duration = 2;

        //    layer.AddAnimation(animRotate, "transform");
        //}
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            TasarimlariOlustur();
            GirisYapButton.TouchUpInside += GirisYapButton_TouchUpInside;
            KayitOlButton.TouchUpInside += KayitOlButton_TouchUpInside;
            AdText.Text = "Mobil13";
            SoyadText.Text = "Fi";
            EmailTxt.Text = "mobil13@intellifi.tech";
            SifreTxt.Text = "1234qwer";
            SifreTekrarTxt.Text = "1234qwer";
            SifreTxt.SecureTextEntry = true;
            SifreTekrarTxt.SecureTextEntry = true;
        }

        private void KayitOlButton_TouchUpInside(object sender, EventArgs e)
        {
            if (BosVarmi())
            {
                if (ControlUserAction())
                {
                    CustomLoading.Show(this, "Lütfen Bekleyin...");
                    new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                    {
                        WebService webService = new WebService();
                        KayitIcinRoot kayitIcinRoot = null;
                        InvokeOnMainThread(delegate () {

                            kayitIcinRoot = new KayitIcinRoot()
                            {
                                firstName = AdText.Text.Trim(),
                                lastName = SoyadText.Text.Trim(),
                                password = SifreTxt.Text,
                                login = EmailTxt.Text,
                                email = EmailTxt.Text
                            };
                        });

                        string jsonString = JsonConvert.SerializeObject(kayitIcinRoot);
                        var Responsee = webService.ServisIslem("register", jsonString, true);
                        if (Responsee != "Hata")
                        {
                            TokenAlDevamEt();

                        }
                        else
                        {
                            CustomLoading.Hide();
                            CustomAlert.GetCustomAlert(this, " Bir sorun oluþtu lütfen internet baðlantýnýzý kontrol edin.");
                            return;
                        }
                    })).Start();
                }
            }
        }
        bool ControlUserAction()
        {
            if (AdText.Text.Length < 2)
            {
                CustomAlert.GetCustomAlert(this,"Lütfen adýnýzý kontrol edin!");
                return false;
            }
            else if (SoyadText.Text.Length < 2)
            {
                CustomAlert.GetCustomAlert(this, "Lütfen soyadýnýzý kontrol edin!");
                return false;
            }
            else if (isValidEmail(EmailTxt.Text) == false)
            {
                CustomAlert.GetCustomAlert(this, "Lütfen emalinizi kontrol edin!");
                return false;
            }
            else if (SifreTxt.Text.Length < 6 == true)
            {
                CustomAlert.GetCustomAlert(this, "Þifreniz 6 karakterden az olamaz!");
                return false;
            }
            else if (SifreTekrarTxt.Text.Length < 6 == true)
            {
                CustomAlert.GetCustomAlert(this, "Þifreniz 6 karakterden az olamaz!");
                return false;
            }
            else if (SifreTxt.Text != SifreTekrarTxt.Text)
            {
                CustomAlert.GetCustomAlert(this, "Þifreler uyuþmuyor lütfen tekrar kontrol edin.");
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool isValidEmail(string email)
        {
            var emailPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
            if (Regex.IsMatch(email, emailPattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void GirisYapButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }
        void TokenAlDevamEt()
        {
            LoginRoot loginRoot = null;
            InvokeOnMainThread(delegate () {
                loginRoot = new LoginRoot()
                {
                    password = SifreTxt.Text,
                    rememberMe = true,
                    username = EmailTxt.Text,
                };
            });
           
            string jsonString = JsonConvert.SerializeObject(loginRoot);
            WebService webService = new WebService();
            var Donus = webService.ServisIslem("authenticate", jsonString, true);
            if (Donus == "Hata")
            {
                CustomLoading.Hide();
                CustomAlert.GetCustomAlert(this,"Giriþ Yapýlamadý!");
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
                    else
                    {
                        CustomLoading.Hide();
                        CustomAlert.GetCustomAlert(this,"Bir sorun oluþtu lütfen daha sonra tekrar deneyin.");
                        return;
                    }
                }
            }
        }
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
                CustomLoading.Hide();
                return false;
            }

        }
        bool BosVarmi()
        {
            if (AdText.Text.Trim() == "")
            {
                CustomAlert.GetCustomAlert(this,"Lütfen Adýnýzý Girin");
                return false;
            }
            else if (SoyadText.Text.Trim() == "")
            {
                CustomAlert.GetCustomAlert(this,"Lütfen Soyadýnýzý Girin");
                return false;
            }
            else if (EmailTxt.Text.Trim() == "")
            {
                CustomAlert.GetCustomAlert(this,"Lütfen Email Girin");
                return false;
            }
            else if (SifreTxt.Text.Trim() == "")
            {
                CustomAlert.GetCustomAlert(this,"Lütfen bir þifre belirtin");
                return false;
            }
            else if (SifreTekrarTxt.Text.Trim() == "")
            {
                CustomAlert.GetCustomAlert(this,"Lütfen þifre tekrarýný yazýnýz");
                return false;
            }
            else if (SifreTxt.Text != SifreTekrarTxt.Text)
            {
                CustomAlert.GetCustomAlert(this,"Þifreler uyuþmuyor lütfen tekrar kontrol edin.");
                return false;
            }
            else
            {
                return true;
            }
        }

        void TasarimlariOlustur()
        {
            EditTextAyarla(AdText, "Ad");
            EditTextAyarla(SoyadText, "Soyad");
            EditTextAyarla(EmailTxt,"Mail");
            EditTextAyarla(SifreTekrarTxt, "Þifre");
            EditTextAyarla(SifreTxt, "Þifre Tekrar");
        #region UI Tasarim
            ButtonBg(KayitOlButton);
        }
        void EditTextAyarla(UITextField GelenInput,string PlaceHolderr)
        {
            GelenInput.BackgroundColor = UIColor.White.ColorWithAlpha(0.33f);
            GelenInput.Layer.CornerRadius = GelenInput.Frame.Height / 2;
            GelenInput.TextColor = UIColor.White;
            GelenInput.AttributedPlaceholder = new NSAttributedString(PlaceHolderr, null, UIColor.White);
            UIView paddingView = new UIView(new CGRect(0, 0, 15f, EmailTxt.Frame.Height));
            paddingView.BackgroundColor = UIColor.Clear;
            GelenInput.LeftView = paddingView;
            GelenInput.LeftViewMode = UITextFieldViewMode.Always;
            GelenInput.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
        }
        void ButtonBg(UIButton GelenButton)
        {
            GelenButton.BackgroundColor = UIColor.Clear;
            GelenButton.Layer.CornerRadius = GelenButton.Frame.Height / 2;
            GelenButton.Layer.BorderWidth = 2f;
            GelenButton.Layer.BorderColor = UIColor.White.ColorWithAlpha(0.33f).CGColor;
        }

        #endregion
        public class KayitIcinRoot
        {
            public string email { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string login { get; set; }
            public string password { get; set; }
        }
    }
}