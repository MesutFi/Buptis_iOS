using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.GirisKayit;
using CoreAnimation;
using CoreLocation;
using Foundation;
using System;
using UIKit;
using Xamarin.Essentials;

namespace Buptis_iOS
{
    public partial class GirisYapBaseVC : UIViewController
    {
        public GirisYapBaseVC (IntPtr handle) : base (handle)
        {

        }
        bool Actimi = false;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!Actimi)
            {
                var MainStoryBoard = UIStoryboard.FromName("GirisKayit", NSBundle.MainBundle);
                var GirisVC1 = MainStoryBoard.InstantiateViewController("GirisVC") as GirisVC;
                var viewController = GirisVC1;
                viewController.View.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 700);
                viewController.WillMoveToParentViewController(this);
                ScrollHazne.AddSubview(viewController.View);
                this.AddChildViewController(viewController);
                viewController.DidMoveToParentViewController(this);
                ScrollHazne.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, 700);
                Actimi = true;
            }
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }
    }
    public class LoginRoot
    {
        public string password { get; set; }
        public bool rememberMe { get; set; }
        public string username { get; set; }
    }
}