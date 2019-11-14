using Buptis_iOS.GenericClass;
using Buptis_iOS.Lokasyonlar;
using Buptis_iOS.Web_Service;
using CoreGraphics;
using Foundation;
using System;
using System.Drawing;
using System.Threading.Tasks;
using UIKit;

namespace Buptis_iOS
{
    public partial class RatingVC : UIViewController
    {
        UIButton[] ratingButtons = new UIButton[10];
        public Mekanlar_Location gelenMekan;
        CoreGraphics.CGPoint viewCenter;
        public RatingVC (IntPtr handle) : base (handle)
        {

        }
        #region Life Cycles 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Tasarim();
            ButtonlariEkle();
            for (int i = 0; i < ratingButtons.Length; i++)
            {

                ratingButtons[i].Title(UIControlState.Normal);
                ratingButtons[i].TouchUpInside += ratingButtons_TouchUpInside;
            }
            BackButton.TouchUpInside += BackButton_TouchUpInside;
            OkButton.TouchUpInside += OkButton_TouchUpInside;
        }

        private void OkButton_TouchUpInside(object sender, EventArgs e)
        {
            LokasyonRate(SonSecilenRate.ToString());
            this.View.BackgroundColor = UIColor.Clear;
            Task.Run(delegate () {
                InvokeOnMainThread(delegate ()
                {
                    this.DismissViewController(true, null);
                });
            });
        }

        public void ButtonlariEkle()
        {
            ratingButtons[0] = button1;
            ratingButtons[1] = button2;
            ratingButtons[2] = button3;
            ratingButtons[3] = button4;
            ratingButtons[4] = button5;
            ratingButtons[5] = button6;
            ratingButtons[6] = button7;
            ratingButtons[7] = button8;
            ratingButtons[8] = button9;
            ratingButtons[9] = button10;
        }
        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.View.BackgroundColor = UIColor.Clear;
            Task.Run(delegate () {
                InvokeOnMainThread(delegate ()
                {
                    this.DismissViewController(true, null);
                });
            });
        }

        private void ratingButtons_TouchUpInside(object sender, EventArgs e)
        {
           int titlee =Convert.ToInt32(((UIButton)sender).TitleLabel.Text);
           
            ArkaPlanSifirla(titlee);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            View.BackgroundColor = UIColor.Clear;
            ViewHazne.Hidden = true;
           
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            viewCenter = ViewHazne.Center;
            ViewAnimation();
            SetBackGround();
        }

        #endregion
        void LokasyonRate(string Ratee)
        {
            WebService webService = new WebService();
            var Donus = webService.ServisIslem("locations/rating/" + gelenMekan.id, Ratee);
            if (Donus != "Hata")
            {
                CustomAlert.GetCustomAlert(this, "Değerlendirme için teşekkürler!");
                return;
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Bir sorun oluştu!");
                return;
            }
        }


        int SonSecilenRate = 0;
        void ArkaPlanSifirla(int index)
        {
            for (int i = 0; i < ratingButtons.Length; i++)
            {
                ratingButtons[i].SetBackgroundImage(UIImage.FromBundle("Images/star2"),UIControlState.Normal);
            }
            ratingButtons[index - 1].SetBackgroundImage(UIImage.FromBundle("Images/stariconmavi"), UIControlState.Normal);
            SonSecilenRate = index;
        }
        #region UITasarim
        public void Tasarim()
        {
            ViewHazne.BackgroundColor = UIColor.White;
            ViewHazne.Layer.CornerRadius = 30f;
            ViewHazne.ClipsToBounds = true;

            OkButton.BackgroundColor = UIColor.FromRGB(225, 0, 105);
            OkButton.Layer.CornerRadius = 15f;
            OkButton.ClipsToBounds = true;
        }
        public void ViewAnimation()
        {
            ViewHazne.Hidden = false;
            SlideVerticaly(ViewHazne, true, false);

            new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
            {
                await Task.Run(async delegate () {

                    await Task.Delay(1000);
                });
                InvokeOnMainThread(delegate
                {

                    UIView.Animate(0.8, 0, UIViewAnimationOptions.CurveEaseInOut,
                        () =>
                        {
                            ViewHazne.Center = viewCenter;
                        },
                        () =>
                        {
                            ViewHazne.Center = viewCenter;
                        }
                    );
                });
            })).Start();
        }
        public static void SlideVerticaly(UIView view, bool isIn, bool fromTop, double duration = 0.3, Action onFinished = null)
        {
            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;
            var minTransform = CGAffineTransform.MakeTranslation(0, (fromTop ? -1 : 1) * view.Bounds.Height);
            var maxTransform = CGAffineTransform.MakeIdentity();

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                    view.Transform = isIn ? maxTransform : minTransform;
                },
                onFinished
            );
        }
        void SetBackGround()
        {
            float sayac = 0;
            Task.Run(async delegate () {
            Atla:
                await Task.Delay(1);
                InvokeOnMainThread(delegate () {
                    sayac += 0.01f;
                    this.View.BackgroundColor = UIColor.FromRGB(0, 0, 245).ColorWithAlpha(sayac);

                });
                if (sayac <= 0.8f)
                {
                    goto Atla;
                }
            });
        }
        

        #endregion
    }
}
