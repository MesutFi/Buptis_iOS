using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using Xamarin.CircularProgress.iOS;
using static Buptis_iOS.ProfilSorulariBaseVC;

namespace Buptis_iOS
{
    public partial class SonucVC : UIViewController
    {
        private CircularProgress starProgress;
        int progress = 0;
        List<UserAnswersDTO> userAnswers = new List<UserAnswersDTO>();
        
        public ProfilSorulariBaseVC ProfilSorulariBaseVC1;
        public SonucVC (IntPtr handle) : base (handle)
        {
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewBackground();
            Hazne1.BackgroundColor = UIColor.Clear;
            ButtonlariDuzenle(CloseButton);
            ActionButton.TouchUpInside += ActionButton_TouchUpInside;
            CloseButton.TouchUpInside += CloseButton_TouchUpInside;
            DahaSonraButton.TouchUpInside += DahaSonraButton_TouchUpInside;
        }
        private void DahaSonraButton_TouchUpInside(object sender, EventArgs e)
        {
            Kayit();
        }
        private void CloseButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(false, null);
        }
        private void ActionButton_TouchUpInside(object sender, EventArgs e)
        {
            if (CounttDolu < userAnswers.Count)
            {
                this.DismissViewController(false,null);
            }
            else
            {
               
                Kayit();
                this.DismissViewController(false, null);
            }
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ActionButtonBg();
            SonDurumuYansit();
            CreateProgress();
        }
        void SonDurumuYansit()
        {
            for (int i = 0; i < ProfilSorulariBaseVC1.Noktalar.Length; i++)
            {
                if (ProfilSorulariBaseVC1.Noktalar[i].GetType() == typeof(CoktanSecmeliSoruVC))
                {
                    var Cevap = ((CoktanSecmeliSoruVC)ProfilSorulariBaseVC1.Noktalar[i]).GetSelectedAnswer();
                    userAnswers.Add(Cevap);
                }
                else if (ProfilSorulariBaseVC1.Noktalar[i].GetType() == typeof(RangeSoruVC))
                {
                    var Cevap = ((RangeSoruVC)ProfilSorulariBaseVC1.Noktalar[i]).GetSelectedAnswer();
                    userAnswers.Add(Cevap);
                }
            }
        }
        int CounttDolu = 0;
        void CreateProgress()
        {
            for (int i = 0; i < userAnswers.Count; i++)
            {
                if (userAnswers[i] != null)
                {
                    CounttDolu += 1;
                }
            }
            StartProgress();
            Counter.Text = CounttDolu.ToString() + "/" + ProfilSorulariBaseVC1.Noktalar.Length;
            
            if (CounttDolu < ProfilSorulariBaseVC1.Noktalar.Length)
            {
                MentionLabel.Text = "Eksik kalan profil bilgilerini tamamla...";
                DahaSonraButton.Hidden = false;
                ActionButton.SetTitle("Profilini Tamamla", UIControlState.Normal);
            }
            else
            {
                MentionLabel.Text = "Tebrikler, tüm profil bilgilerini tamamladýn...";
                DahaSonraButton.Hidden = true;
                ActionButton.SetTitle("Profiline dön", UIControlState.Normal);
            }
        }
         void Kayit()
        {
            List<UserAnswersDTO> NullOlmayanlar = new List<UserAnswersDTO>();
            for (int i = 0; i < userAnswers.Count; i++)
            {
                if (userAnswers[i] != null)
                {
                    NullOlmayanlar.Add(userAnswers[i]);
                }
            }
            string jsonString = JsonConvert.SerializeObject(NullOlmayanlar);
            WebService webService = new WebService();
            var Donus = webService.ServisIslem("answers/user", jsonString);
            if (Donus != "Hata")
            {
                CustomAlert.GetCustomAlert(this, "Cevaplarýnýz için teþekkürler.");
                ProfilSorulariBaseVC1.DismissViewController(false, null);
                DismissViewController(false, null);
                return;
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Bir sorun oluþtu");
                return;
            }
        }


        #region ProgressBar
        private void ConfigureStarCircularProgress()
        {
            CGRect frame = new CGRect(0, 0, 128, 128);
            starProgress = new CircularProgress(frame);
            //starProgress.Center = ProgressHazne.Center;
            starProgress.Colors = new[]
            {
                UIColor.FromRGB(225, 0, 105).CGColor
            };

            starProgress.LineWidth = 10.0;

            var path = new UIBezierPath();
            path.MoveTo(new CGPoint(50.0, 2.0));
            path.AddLineTo(new CGPoint(84.0, 86.0));
            path.AddLineTo(new CGPoint(6.0, 33.0));
            path.AddLineTo(new CGPoint(96.0, 33.0));
            path.AddLineTo(new CGPoint(17.0, 86.0));
            path.ClosePath();
            starProgress.Path = path;
            ProgressHazne.AddSubview(starProgress);
            starProgress.Transform = CGAffineTransform.MakeRotation(3.14159f * 270 / 180f);
        }

        NSTimer Timerr;
        void StartProgress()
        {
            float orantila = Convert.ToSingle(CounttDolu) / Convert.ToSingle(ProfilSorulariBaseVC1.Noktalar.Length);

            Task.Run(async delegate () {
               await Task.Delay(500);
                InvokeOnMainThread(delegate () {
                    ConfigureStarCircularProgress();
                    Timerr = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.01), delegate
                    {
                        UpdateProgress(orantila);
                    });
                });
            });
        }
        private void UpdateProgress(float EndProgress)
        {
            progress = progress + 1;
            var normalizedProgress = (double)(progress / 255.0);
            starProgress.Progress = normalizedProgress;

            if (normalizedProgress >= EndProgress)
            { 
                Timerr.Invalidate();
                Timerr = null;
            }
        }
        #endregion


        #region Tasarim Duzenlemelemeleri
        void ButtonlariDuzenle(UIButton GelenButton)
        {
            GelenButton.Layer.CornerRadius = GelenButton.Frame.Height / 2;
            GelenButton.Layer.BorderColor = UIColor.White.CGColor;
            GelenButton.Layer.BorderWidth = 3f;
            GelenButton.ClipsToBounds = true;
            GelenButton.ContentEdgeInsets = new UIEdgeInsets(10, 10, 10, 10);
        }

        void ActionButtonBg()
        {
            var Color1 = UIColor.FromRGB(237, 2, 59).CGColor;
            var Color2 = UIColor.FromRGB(223, 0, 107).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = ActionButton.Frame;
            ActionButton.Layer.InsertSublayer(gradientLayer, 0);
            ActionButton.Layer.CornerRadius = ActionButton.Frame.Height/2;
            ActionButton.ClipsToBounds = true;
        }

        void ViewBackground()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            this.View.Layer.InsertSublayer(gradientLayer, 0);
    
            this.View.ClipsToBounds = true;
        }
        #endregion
    }
}