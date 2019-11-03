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
    [Register ("KayitVC")]
    partial class KayitVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField AdText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField EmailTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GeriButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GirisYapButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton KayitOlButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField SifreTekrarTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField SifreTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField SoyadText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AdText != null) {
                AdText.Dispose ();
                AdText = null;
            }

            if (EmailTxt != null) {
                EmailTxt.Dispose ();
                EmailTxt = null;
            }

            if (GeriButton != null) {
                GeriButton.Dispose ();
                GeriButton = null;
            }

            if (GirisYapButton != null) {
                GirisYapButton.Dispose ();
                GirisYapButton = null;
            }

            if (KayitOlButton != null) {
                KayitOlButton.Dispose ();
                KayitOlButton = null;
            }

            if (SifreTekrarTxt != null) {
                SifreTekrarTxt.Dispose ();
                SifreTekrarTxt = null;
            }

            if (SifreTxt != null) {
                SifreTxt.Dispose ();
                SifreTxt = null;
            }

            if (SoyadText != null) {
                SoyadText.Dispose ();
                SoyadText = null;
            }
        }
    }
}