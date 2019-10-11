using CoreGraphics;
using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;

namespace Buptis_iOS
{
    public partial class LoadingImageView : UIViewController
    {
        public LoadingImageView(IntPtr handle) : base(handle)
        {
        }

        public string Aciklama { get; set; }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            AciklamaLabel.Text = Aciklama;

            ArkaHazne.BackgroundColor = UIColor.White;
            ArkaHazne.Layer.CornerRadius = 10f;
            ArkaHazne.ClipsToBounds = true;
            StartAnim();
        }

        public NSTimer Timerr;

        void StartAnim()
        {
            int derece = 0;
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                InvokeOnMainThread(delegate ()
                {
                    Timerr = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.004), delegate
                    {
                        derece += 1;
                        LoadingimView.Transform = CGAffineTransform.MakeRotation(3.14159f * derece / 180f);
                    });
                });

            })).Start();
        }

    }

    public static class CustomLoading
    {
        static LoadingImageView controller;
        static UIViewController GelenBase;
        public static void Show(UIViewController GelenBase2, string Aciklama1)
        {
            GelenBase2.InvokeOnMainThread(delegate ()
            {
                var story = UIStoryboard.FromName("GenericUI", NSBundle.MainBundle);
                controller = story.InstantiateViewController("LoadingImageView") as LoadingImageView;
                controller.Aciklama = Aciklama1;
                controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                GelenBase = GelenBase2;
                GelenBase.PresentViewController(controller, true, null);
            });
            
        }
        public static void Hide()
        {
            GelenBase.InvokeOnMainThread(delegate ()
            {
                if (controller != null)
                {
                    if (controller.Timerr != null)
                    {
                        controller.Timerr.Invalidate();
                        controller.Timerr = null;
                    }
                    controller.DismissViewController(true,null);
                    controller = null;
                }
            });
        }
    }

}
