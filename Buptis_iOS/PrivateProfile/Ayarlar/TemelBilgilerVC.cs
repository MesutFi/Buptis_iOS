using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.PrivateProfile.Ayarlar;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using DT.iOS.DatePickerDialog;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UIKit;

namespace Buptis_iOS
{
    public partial class TemelBilgilerVC : UIViewController
    {
        List<string> KonuName = new List<string>();
        List<DateTime> dateTimeList = new List<DateTime>();
        string getGender;
        public TemelBilgilerVC (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            KaydetButton.TouchUpInside += KaydetButton_TouchUpInside;
            ErkekTouch.TouchUpInside += ErkekTouch_TouchUpInside;
            KadinTouch.TouchUpInside += KadinTouch_TouchUpInside;
            birthdayButton.TouchUpInside += BirthdayButton_TouchUpInside;
            
            MeslekText.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            MeslekText.ShouldChangeCharacters = (UITextField field, NSRange range, string ReplacementString) =>
            {
                int maxLength = 30; 
                if (field.Text.Length + ReplacementString.Length > maxLength)
                {
                    return false;
                }
                return true;
            };
        }

        

        private void KaydetButton_TouchUpInside(object sender, EventArgs e)
        {
            if (control())
            {
                UpdateUserDto UpdateUserDto1 = new UpdateUserDto()
                {
                    activated = true,
                    birthDay = Convert.ToDateTime(birtdLbl.Text).ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                    gender = getGender,
                    userJob = MeslekText.Text
                };
                WebService webService = new WebService();
                string jsonString = JsonConvert.SerializeObject(UpdateUserDto1);
                var Donus = webService.ServisIslem("users/update", jsonString);
                if (Donus != "Hata")
                {
                    var UserData = DataBase.MEMBER_DATA_GETIR()[0];
                    UserData.userJob = MeslekText.Text;
                    UserData.birthDayDate = Convert.ToDateTime(birtdLbl.Text);
                    UserData.gender = getGender;
                    if (DataBase.MEMBER_DATA_Guncelle(UserData))
                    {
                        CustomAlert.GetCustomAlert(this, "Bilgileriniz güncellendi.");
                        this.DismissViewController(true, null);
                    }
                }
                else
                {
                    CustomAlert.GetCustomAlert(this, "Bir Sorun Oluþtu.");
                }

            }
        }
        bool control()
        {
            if (birtdLbl.Text.Trim() == "" && MeslekText.Text.Trim() == "" && !KadinRadio.Selected == true && !ErkekRadio.Selected == true)
            {
                CustomAlert.GetCustomAlert(this, "Lütfen bilgilerinizi tamamlayýn.");
                return false;
            }
            else if (birtdLbl.Text.Trim() == "")
            {
                CustomAlert.GetCustomAlert(this, "Lütfen doðum tarihinizi girin.");
                return false;
            }
            else if(MeslekText.Text.Trim() == "")
            {
                CustomAlert.GetCustomAlert(this, "Lütfen mesleðinizi girin.");
                return false;
            }
            else if(!KadinRadio.Selected==true && !ErkekRadio.Selected == true)
            {
                CustomAlert.GetCustomAlert(this, "Lütfen cinsiyetinizi belirtiniz.");
                return false;
            }
            else
            {
                return true;
            }
          
        }
        private void BirthdayButton_TouchUpInside(object sender, EventArgs e)
        {
            var startingTime = KayitliTarihVarmi();
            var dialog = new DatePickerDialog();
            dialog.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            dialog.Show("Doðum Tarihi Seçin", "Tamam", "Vazgeç", UIDatePickerMode.Date, (dt) =>
            {
                birtdLbl.Text = dt.ToShortDateString();
            }, startingTime,DateTime.Now.AddYears(-18), DateTime.Now.AddYears(-65));
        }

        DateTime KayitliTarihVarmi()
        {
            var Me = DataBase.MEMBER_DATA_GETIR()[0];
            if (Me.birthDayDate != null)
            {
                return (DateTime)Me.birthDayDate;
            }
            else
            {
                return DateTime.Now.AddYears(-18);
            }
        }




        private void KadinTouch_TouchUpInside(object sender, EventArgs e)
        {
            HepsiniSifirla(KadinRadio);
            getGender = "Kadýn";
        }
        private void ErkekTouch_TouchUpInside(object sender, EventArgs e)
        {
            HepsiniSifirla(ErkekRadio);
            getGender = "Erkek";
        }
        void HepsiniSifirla(UIButton SecilenButton)
        {
            RadioUnselect(KadinRadio);
            RadioUnselect(ErkekRadio);
            KadinRadio.Selected = false;
            ErkekRadio.Selected = false;
            SecilenButton.Selected = true;
            RadioSelect(SecilenButton);
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            HeaderTasarim();
            BackButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            BackButton.TouchUpInside += BackButton_TouchUpInside;
            AdSoyadTextFieldDuzenle();
            AddButtonGradient();
            RadioUnselect(ErkekRadio);
            RadioUnselect(KadinRadio);
            getUsernfo();

        }
        void AdSoyadTextFieldDuzenle()
        {
            AdSoyadTxt.AttributedPlaceholder = new NSAttributedString("Ýsminiz..", null, UIColor.White.ColorWithAlpha(0.5f));
            UIView paddingView = new UIView(new CGRect(0, 0, 15f, 30f));
            paddingView.BackgroundColor = UIColor.Clear;
            AdSoyadTxt.LeftView = paddingView;
        }
        void getUsernfo()
        {
            var User = DataBase.MEMBER_DATA_GETIR()[0];
            AdSoyadTxt.Text = User.firstName + " " + User.lastName;
            if (!string.IsNullOrEmpty(User.birthDayDate.ToString()))
            {
                birtdLbl.Text = Convert.ToDateTime(User.birthDayDate).ToShortDateString();
            }
            MeslekText.Text = User.userJob;
        }
        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public void RadioSelect(UIButton GelenButton)
        {
            GelenButton.Layer.CornerRadius = 15f;
            GelenButton.BackgroundColor = UIColor.FromRGB(232, 0, 79);
            GelenButton.ClipsToBounds = true;
        }
        public void RadioUnselect(UIButton GelenButton)
        {
            GelenButton.Layer.CornerRadius = 15f;
            GelenButton.Layer.BorderColor = UIColor.FromRGB(232, 0, 79).CGColor;
            GelenButton.BackgroundColor = UIColor.Clear;
            GelenButton.Layer.BorderWidth = 3f;
            GelenButton.ClipsToBounds = true;
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
            KaydetButton.Layer.InsertSublayer(gradientLayer, 0);
            KaydetButton.Layer.CornerRadius = 42f/2f;
            KaydetButton.ClipsToBounds = true;
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