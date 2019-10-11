// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Buptis_iOS
{
    [Register ("ProfilSorulariBaseVC")]
    partial class ProfilSorulariBaseVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AtlaButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ContentScroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GeriButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton IleriButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AtlaButton != null) {
                AtlaButton.Dispose ();
                AtlaButton = null;
            }

            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (ContentScroll != null) {
                ContentScroll.Dispose ();
                ContentScroll = null;
            }

            if (CountLabel != null) {
                CountLabel.Dispose ();
                CountLabel = null;
            }

            if (GeriButton != null) {
                GeriButton.Dispose ();
                GeriButton = null;
            }

            if (IleriButton != null) {
                IleriButton.Dispose ();
                IleriButton = null;
            }
        }
    }
}