
using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace Buptis_iOS.PrivateProfile
{
    public partial class PrivateProfileBaseVC : UIViewController
    {
        public PrivateProfileBaseVC(IntPtr handle) : base(handle)
        {
        }


        #region View lifecycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
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
                var MainStoryBoard = UIStoryboard.FromName("PrivateProfileBaseVC", NSBundle.MainBundle);
                var PrivateProfileVC1 = MainStoryBoard.InstantiateViewController("PrivateProfileVC") as PrivateProfileVC;

                var viewController = PrivateProfileVC1;
                viewController.View.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width , 1000);
                viewController.WillMoveToParentViewController(this);
                ScrollView.AddSubview(viewController.View);
                this.AddChildViewController(viewController);
                viewController.DidMoveToParentViewController(this);
                ScrollView.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, 1000);
                Actimi = true;
            }

        }
        #endregion


    }
}