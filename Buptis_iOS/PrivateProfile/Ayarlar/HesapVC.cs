using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using UIKit;

namespace Buptis_iOS
{
    public partial class HesapVC : UIViewController
    {
        public HesapVC(IntPtr handle) : base(handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            HeaderTasarim();
            AddButtonGradient();
            GetEmail();
            BackButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            BackButton.TouchUpInside += BackButton_TouchUpInside;
            CikisYapButton.TouchUpInside += CikisYapButton_TouchUpInside;
            SifremiUnuttum.TouchUpInside += SifremiUnuttum_TouchUpInside;
            HesabiSilButton.TouchUpInside += HesabiSilButton_TouchUpInside;


        }

        private void HesabiSilButton_TouchUpInside(object sender, EventArgs e)
        {
            UIAlertView alert = new UIAlertView();
            alert.Title = "Buptis";
            alert.AddButton("Evet");
            alert.AddButton("Hayýr");
            alert.Message = "Hesabınızı silerseniz bu kullanıcı ile Buptis'e bir daha giriş yapamazsınız.\nHesabı silmek istiyor musunuz?";
            alert.AlertViewStyle = UIAlertViewStyle.Default;
            alert.Clicked += (object s, UIButtonEventArgs ev) =>
            {
                if (ev.ButtonIndex == 0)
                {
                    alert.Dispose();
                    if (HesabiSilApi())
                    {
                        string path;
                        path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                        File.Delete(System.IO.Path.Combine(path, "Buptis.db"));
                        InvokeOnMainThread(delegate () {
                            alert.Dispose();
                            var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
                            appDelegate.SetRootSplashViewController();
                        });
                    }

                }
                else
                {
                    alert.Dispose();
                }
            };
            alert.Show();
        }
        bool HesabiSilApi()
        {
            WebService webService = new WebService();
            var Mee = DataBase.MEMBER_DATA_GETIR()[0];
            UpdateUserDto UpdateUserDto1 = new UpdateUserDto()
            {
                activated = false,
                birthDay = Convert.ToDateTime(Mee.birthDayDate).ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                gender = Mee.gender,
                userJob = Mee.userJob
            };
            string jsonString = JsonConvert.SerializeObject(UpdateUserDto1);
            var Donus = webService.ServisIslem("users/update", jsonString);
            if (Donus != "Hata")
            {

                return true;
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Bir sorun oluştu. ");
                return false;
            }
        }
        private void SifremiUnuttum_TouchUpInside(object sender, EventArgs e)
        {

            UIAlertView alert = new UIAlertView()
            {
                Title = "Buptis",
                Message = "Þifreniz : " + GetPassword()
            };
            alert.AddButton("Tamam");
            alert.Show();
        }
        string GetPassword()
        {
            var UserPassword = DataBase.MEMBER_DATA_GETIR()[0].password;
            return UserPassword;
        }
        private void CikisYapButton_TouchUpInside(object sender, EventArgs e)
        {
            var Confirm = new UIAlertView("Buptis", "Çıkmak istediğinize emin misiniz ?", null, "Evet", "Hayır");
            Confirm.Show();
            Confirm.Clicked += (object senders, UIButtonEventArgs es) =>
            {
                if (es.ButtonIndex == 0)
                {
                    //Evetse
                    string path;
                    path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    File.Delete(System.IO.Path.Combine(path, "Buptis.db"));
                    InvokeOnMainThread(delegate () {
                     Confirm.Dispose();
                        //Thread.CurrentThread.Abort();
                        var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
                        appDelegate.SetRootSplashViewController();

                    });
                 

                }
                else
                {
                    // hayırsa
                    Confirm.Dispose();
                }
               
            };
            Confirm.Show();
        }

        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }
        void GetEmail()
        {
            var UserEmail = DataBase.MEMBER_DATA_GETIR()[0].email;
            var Bol = UserEmail.Split('@');
            var IlkHarf = Bol[0].Substring(0, 1);
            var yildizlar = "";
            for (int i = 1; i < Bol[0].Length; i++)
            {
                yildizlar += "*";
            }
            EmailLabel.Text = IlkHarf + yildizlar + "@" + Bol[1];
        }
        void AddButtonGradient()
        {
            var Color1 = UIColor.FromRGB(237, 2, 59).CGColor;
            var Color2 = UIColor.FromRGB(223, 0, 107).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = new CGRect(0, 0, 170f, 42f);
            CikisYapButton.Layer.InsertSublayer(gradientLayer, 0);
            CikisYapButton.Layer.CornerRadius = 42f / 2f;
            CikisYapButton.ClipsToBounds = true;
        }
        void HeaderTasarim()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 126f);
            HeaderView.Layer.InsertSublayer(gradientLayer, 0);
            HeaderView.Layer.CornerRadius = 30;
            HeaderView.ClipsToBounds = true;
            HeaderView.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);
        }
        public class UpdateUserDto
        {
            public bool activated { get; set; }
            public string birthDay { get; set; }
            public string gender { get; set; }
            public string userJob { get; set; }
        }
    }
}
