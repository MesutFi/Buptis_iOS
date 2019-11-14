using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using Xamarin.RangeSlider;

namespace Buptis_iOS
{
    public partial class FiltreVC : UIViewController
    {
        #region Tanimlamalar
        List<UIButton> Menuler = new List<UIButton>();
        int SonCinsiyetSecim = 1;
        

        #endregion
        public FiltreVC(IntPtr handle) : base(handle)
        {
        }
        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ManButton.Tag = 1;
            WomanButton.Tag = 2;
            BothButton.Tag = 3;
            ManButton.TouchUpInside += ManButton_TouchUpInside;
            WomanButton.TouchUpInside += WomanButton_TouchUpInside;
            BothButton.TouchUpInside += BothButton_TouchUpInside;
            BackButton.TouchUpInside += BackButton_TouchUpInside;
            OkButon.TouchUpInside += OkButon_TouchUpInside;
        }

        private void OkButon_TouchUpInside(object sender, EventArgs e)
        {
            var MinValue = Math.Round(rangeSlider.LowerValue, 0);
            var MaxValue = Math.Round(rangeSlider.UpperValue, 0);

            maxText.Text = MinValue.ToString() + " - " + MaxValue.ToString();
            FILTRELER fILTRELER = new FILTRELER()
            {
                Cinsiyet = SonCinsiyetSecim,
                minAge = (int)Math.Round(Convert.ToDouble(MinValue), 0),
                maxAge = (int)Math.Round(Convert.ToDouble(MaxValue), 0)
            };
            if (DataBase.FILTRELER_TEMIZLE())
            {
                if (DataBase.FILTRELER_EKLE(fILTRELER))
                {
                    //CustomAlert.GetCustomAlert(this, "Filtreler kaydedildi.");
                    this.View.BackgroundColor = UIColor.Clear;
                    Task.Run(delegate ()
                    {
                        InvokeOnMainThread(delegate ()
                        {
                            SlideVerticaly(FilterTabView, false, true);
                            this.DismissViewController(true, null);
                        });
                    });
                }
                else
                {
                    CustomAlert.GetCustomAlert(this, "Bir sorun oluştu.");
                }
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Bir sorun oluştu.");
            }
        }
        private void BothButton_TouchUpInside(object sender, EventArgs e)
        {
            var Tagg = (int)((UIButton)sender).Tag;
            HepsiniSifirla(BothButton);
            SonCinsiyetSecim = Tagg;
            ButtonTasarimlariniDuzenle(2);

        }
        private void WomanButton_TouchUpInside(object sender, EventArgs e)
        {
            var Tagg = (int)((UIButton)sender).Tag;
            HepsiniSifirla(WomanButton);
            SonCinsiyetSecim = Tagg;
            ButtonTasarimlariniDuzenle(1);
        }
        private void ManButton_TouchUpInside(object sender, EventArgs e)
        {
            var Tagg = (int)((UIButton)sender).Tag;
            HepsiniSifirla(ManButton);
            SonCinsiyetSecim = Tagg;
            ButtonTasarimlariniDuzenle(0);
        }

        void HepsiniSifirla(UIButton GelenButon)
        {
            GelenButon.BackgroundColor = UIColor.Clear;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
           
            Design();
            FilterTabView.Hidden = true;
            this.View.BackgroundColor = UIColor.Clear;
            rangeSlider.ShowTextAboveThumbs = false;
            rangeSlider.DragCompleted += RangeSlider_DragCompleted;
            Menuler = new List<UIButton>();
            Menuler.Add(ManButton);
            Menuler.Add(WomanButton);
            Menuler.Add(BothButton);

            GetFilter();
        }
        private void RangeSlider_DragCompleted(object sender, EventArgs e)
        {
            var MinValue = Math.Round(rangeSlider.LowerValue, 0);
            var MaxValue = Math.Round(rangeSlider.UpperValue, 0);
            if (MaxValue>=65)
            {
                maxText.Text = MinValue.ToString() + " - " + "65+";
            }
            else
            {
                maxText.Text = MinValue.ToString() + " - " + MaxValue.ToString();
            }
            
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            viewCenter = FilterTabView.Center;
            ViewAnimation();
            SetBackGround();
            ButtonTasarimlariniDuzenle(0);
        }
        void GetFilter()
        {
            var filtre2 = DataBase.FILTRELER_GETIR();
            if (filtre2.Count > 0)
            {
                var filtre = filtre2[0];

                rangeSlider.LowerValue = filtre.minAge;
                rangeSlider.UpperValue = filtre.maxAge;

                maxText.Text = filtre.minAge.ToString() + " - " + filtre.maxAge.ToString();


                if (filtre.Cinsiyet == 1)
                {
                    HepsiniSifirla(ManButton);
                    SonCinsiyetSecim = 1;
                    ButtonTasarimlariniDuzenle(0);
                }
                else if (filtre.Cinsiyet == 2)
                {
                    HepsiniSifirla(WomanButton);
                    SonCinsiyetSecim = 2;
                    ButtonTasarimlariniDuzenle(1);
                }
                else
                {
                    HepsiniSifirla(BothButton);
                    SonCinsiyetSecim = 3;
                    ButtonTasarimlariniDuzenle(2);
                }
            }
        }

        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {

            Task.Run(delegate ()
            {
                InvokeOnMainThread(delegate ()
                {
                    SlideVerticaly(FilterTabView, false, true);
                    this.DismissViewController(true, null);
                });

            });
        }
        CoreGraphics.CGPoint viewCenter;
        public void ViewAnimation()
        {
            FilterTabView.Hidden = false;
            SlideVerticaly(FilterTabView, true, true);

            new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
            {
                await Task.Run(async delegate ()
                {

                    await Task.Delay(1000);
                });
                InvokeOnMainThread(delegate
                {

                    UIView.Animate(0.8, 0, UIViewAnimationOptions.CurveEaseInOut,
                        () =>
                        {
                            FilterTabView.Center = viewCenter;
                        },
                        () =>
                        {
                            FilterTabView.Center = viewCenter;
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
                () =>
                {
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                    view.Transform = isIn ? maxTransform : minTransform;
                },
                onFinished
            );
        }

        void SetBackGround()
        {
            float sayac = 0;
            Task.Run(async delegate ()
            {
            Atla:
                await Task.Delay(1);
                InvokeOnMainThread(delegate ()
                {
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

        #region UI Tasarim
        public void Design()
        {
            TabViewMenu.BackgroundColor = UIColor.FromRGB(34, 30, 32);
            TabViewMenu.Layer.CornerRadius = 10f;
            TabViewMenu.ClipsToBounds = true;

            FilterTabView.BackgroundColor = UIColor.White;
            FilterTabView.Layer.MaskedCorners = (CoreAnimation.CACornerMask)12;
            FilterTabView.Layer.CornerRadius = 30f;
            FilterTabView.ClipsToBounds = true;

            OkButon.BackgroundColor = UIColor.FromRGB(226, 0, 93);
            OkButon.Layer.CornerRadius = 40 / 2;
            OkButon.ClipsToBounds = true;

        }
        void ButtonTasarimlariniDuzenle(int Index)
        {

            for (int i = 0; i < Menuler.Count; i++)
            {
                Menuler[i].BackgroundColor = UIColor.FromRGB(34, 30, 32);
                Menuler[i].SetTitleColor(UIColor.White, UIControlState.Normal);
            }
            Menuler[Index].BackgroundColor = UIColor.FromRGB(226, 0, 93);
            Menuler[Index].Layer.CornerRadius = 10f;
            Menuler[Index].ClipsToBounds = true;
            Menuler[Index].SetTitleColor(UIColor.White, UIControlState.Normal);
        }
        #endregion
    }


}
