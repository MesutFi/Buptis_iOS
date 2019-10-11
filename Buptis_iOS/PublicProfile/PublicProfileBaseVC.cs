
using System;
using System.Drawing;
using System.Threading.Tasks;
using Buptis_iOS.Database;
using Foundation;
using UIKit;

namespace Buptis_iOS.PublicProfile
{
    public partial class PublicProfileBaseVC : UIViewController
    {
      
        public PublicProfileBaseVC(IntPtr handle) : base(handle)
        {
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        bool Actimi = false;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!Actimi)
            {
                var MainStoryBoard = UIStoryboard.FromName("PublicProfileBaseVC", NSBundle.MainBundle);
                var PublicProfileVC1 = MainStoryBoard.InstantiateViewController("PublicProfileVC") as PublicProfileVC;
                PublicProfileVC1.GelenBase = this;
                var viewController = PublicProfileVC1;
                viewController.View.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 820);
                viewController.WillMoveToParentViewController(this);
                ScrollVieww.AddSubview(viewController.View);
                this.AddChildViewController(viewController);
                viewController.DidMoveToParentViewController(this);
                ScrollVieww.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, 820);
                ScrollTopp();
                Actimi = true;
            }
        }
        public void Closee()
        {
            this.DismissViewController(true, null);
        }
        void ScrollTopp()
        {
            Task.Run(async delegate () {
                await Task.Delay(1000);

                InvokeOnMainThread(delegate () {
                    ScrollVieww.SetContentOffset(new CoreGraphics.CGPoint(0, ScrollVieww.ContentInset.Top), true);
                });

            });
        }
        #endregion
    }

  
}