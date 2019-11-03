using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.PrivateProfile.Ayarlar;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UIKit;

namespace Buptis_iOS
{
    public partial class BizeYazin : UIViewController
    {
        List<ContactDTO> KonuList = new List<ContactDTO>();
        List<string> KonuName = new List<string>();
        public BizeYazin(IntPtr handle) : base(handle)
        {

        }

        #region Life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            textField.ShouldChangeCharacters = (UITextField field, NSRange range, string ReplacementString) =>
            {
                int maxLength = 300;
                if (field.Text.Length + ReplacementString.Length > maxLength)
                {
                    return false;
                }
                return true;
            };
            List<ContactDTO> KonuList = new List<ContactDTO>
            {
                new ContactDTO{ topic = "Teknik Sorun"},
                new ContactDTO{ topic = "Þikayet"},
                new ContactDTO{ topic = "Öneri"},
                new ContactDTO{ topic = "Diðer"}
            };
            for (int i = 0; i < KonuList.Count; i++)
            {
                KonuName.Add(KonuList[i].topic);
            }

            SetupPicker(textField, KonuName);
            GonderButton.TouchUpInside += GonderButton_TouchUpInside;

            #region DONE BUtton
            UIToolbar toolbar = new UIToolbar();
            toolbar.BarStyle = UIBarStyle.Default;
            toolbar.Translucent = true;
            toolbar.SizeToFit();

            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new[] { UIColor.FromRGB(15, 0, 241).CGColor, UIColor.FromRGB(2, 0, 100).CGColor };
            gradientLayer.Locations = new NSNumber[] { 0, 1 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 1);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 0);
            gradientLayer.Frame = toolbar.Bounds;
            toolbar.Layer.MasksToBounds = true;
            toolbar.Layer.InsertSublayer(gradientLayer, 0);

            UIBarButtonItem doneButton = new UIBarButtonItem("Tamam", UIBarButtonItemStyle.Done,
                                                             (s, e) =>
                                                             {
                                                                 MesajTextView.ResignFirstResponder();
                                                             });

            doneButton.TintColor = UIColor.White;
            toolbar.SetItems(new[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton }, true);
            MesajTextView.InputAccessoryView = toolbar;
            #endregion
        }

        private void GonderButton_TouchUpInside(object sender, EventArgs e)
        {
            BizeYazinGonder();
          
        }

        void BizeYazinGonder()
        {
            if (BosVarmi())
            {
                var Me = DataBase.MEMBER_DATA_GETIR()[0];
                WebService webService = new WebService();
                ContactDTO contactDTO = new ContactDTO()
                {
                    text = MesajTextView.Text,
                    topic = textField.Text,
                    userId = Me.id
                };
                string jsonString = JsonConvert.SerializeObject(contactDTO);
                var Donus = webService.ServisIslem("contacts", jsonString);
                if (Donus != "Hata")
                {
                   var alert = new UIAlertView();
                    alert.Title = "Buptis";
                    alert.AddButton("Tamam");
                    alert.Message = "Destek talebiniz iletildi. Teþekkürler...";
                    alert.AlertViewStyle = UIAlertViewStyle.Default;
                    alert.Clicked += (object s, UIButtonEventArgs ev) =>
                    {
                        alert.Dispose();
                        this.DismissViewController(true, null);

                    };
                    alert.Show();
                    textField.Text ="";
                    MesajTextView.Text = "";
                }
                else
                {
                    CustomAlert.GetCustomAlert(this, "Bir sorun oluþtu...");
                    return;
                }
           
            }
        }
        bool BosVarmi()
        {
            //textField.Text.Trim())
            if (string.IsNullOrEmpty(textField.Text.Trim()))
            {
                CustomAlert.GetCustomAlert(this, "Lütfen konuyu belirtin..");
                return false;
            }
            else if (string.IsNullOrEmpty(MesajTextView.Text.Trim()))
            {
                CustomAlert.GetCustomAlert(this, "Lütfen mesajýnýzý belirtin..");
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            HeaderTasarim();
            AddButtonGradient();
            Backbutton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            Backbutton.TouchUpInside += Backbutton_TouchUpInside;

        }
        private void Backbutton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }
        #endregion
        #region UI Tasarim 
        void AddButtonGradient()
        {
            var Color1 = UIColor.FromRGB(237, 2, 59).CGColor;
            var Color2 = UIColor.FromRGB(223, 0, 107).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = new CGRect(0, 0, 170f, 42f);
            GonderButton.Layer.InsertSublayer(gradientLayer, 0);
            GonderButton.Layer.CornerRadius = 42f / 2f;
            GonderButton.ClipsToBounds = true;

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
            Headerview.Layer.InsertSublayer(gradientLayer, 0);
            Headerview.Layer.CornerRadius = 30;
            Headerview.ClipsToBounds = true;
            Headerview.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);
        }
        private void SetupPicker(UITextField GelenText, List<string> Gelenitems)
        {
            var Secim = "";
            PickerModel model = new PickerModel(Gelenitems);
            model.PickerChanged += (sender, e) =>
            {
                Secim = e.SelectedValue;
            };

            UIPickerView picker = new UIPickerView();
            picker.ShowSelectionIndicator = true;
            picker.Model = model;

            UIToolbar toolbar = new UIToolbar();
            toolbar.BarStyle = UIBarStyle.Default;
            toolbar.Translucent = true;
            toolbar.SizeToFit();

            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new[] { UIColor.FromRGB(15, 0, 241).CGColor, UIColor.FromRGB(2, 0, 100).CGColor };
            gradientLayer.Locations = new NSNumber[] { 0, 1 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 1);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 0);
            gradientLayer.Frame = toolbar.Bounds;
            toolbar.Layer.MasksToBounds = true;
            toolbar.Layer.InsertSublayer(gradientLayer, 0);

            UIBarButtonItem doneButton = new UIBarButtonItem("Tamam", UIBarButtonItemStyle.Done,
                                                             (s, e) =>
                                                             {
                                                                 GelenText.Text = Secim;
                                                                 GelenText.ResignFirstResponder();
                                                             });

            doneButton.TintColor = UIColor.White;
            toolbar.SetItems(new[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton }, true);

            GelenText.InputView = picker;

            GelenText.InputAccessoryView = toolbar;
        }
        #endregion

        public class ContactDTO
        {
            public string createdDate { get; set; }
            public string id { get; set; }
            public string lastModifiedDate { get; set; }
            public string text { get; set; }
            public string topic { get; set; }
            public int userId { get; set; }
        }

    }
}