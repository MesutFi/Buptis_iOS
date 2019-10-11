// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Buptis_iOS.GirisKayit
{
    [Register ("GirisVC")]
    partial class GirisVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField EmailTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FacebookButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GirisButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GoogleButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SifrenimiUnuttunButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField SifreTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UyeOlButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (EmailTxt != null) {
                EmailTxt.Dispose ();
                EmailTxt = null;
            }

            if (FacebookButton != null) {
                FacebookButton.Dispose ();
                FacebookButton = null;
            }

            if (GirisButton != null) {
                GirisButton.Dispose ();
                GirisButton = null;
            }

            if (GoogleButton != null) {
                GoogleButton.Dispose ();
                GoogleButton = null;
            }

            if (SifrenimiUnuttunButton != null) {
                SifrenimiUnuttunButton.Dispose ();
                SifrenimiUnuttunButton = null;
            }

            if (SifreTxt != null) {
                SifreTxt.Dispose ();
                SifreTxt = null;
            }

            if (UyeOlButton != null) {
                UyeOlButton.Dispose ();
                UyeOlButton = null;
            }
        }
    }
}