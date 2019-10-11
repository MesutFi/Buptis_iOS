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
    [Register ("HediyeGonderVC")]
    partial class HediyeGonderVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GeriButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView HediyelerScroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ViewHazne { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (GeriButton != null) {
                GeriButton.Dispose ();
                GeriButton = null;
            }

            if (HediyelerScroll != null) {
                HediyelerScroll.Dispose ();
                HediyelerScroll = null;
            }

            if (ViewHazne != null) {
                ViewHazne.Dispose ();
                ViewHazne = null;
            }
        }
    }
}