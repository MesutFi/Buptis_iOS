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
    [Register ("SonucVC")]
    partial class SonucVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ActionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Counter { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DahaSonraButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Hazne1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MentionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ProgressHazne { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActionButton != null) {
                ActionButton.Dispose ();
                ActionButton = null;
            }

            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (Counter != null) {
                Counter.Dispose ();
                Counter = null;
            }

            if (DahaSonraButton != null) {
                DahaSonraButton.Dispose ();
                DahaSonraButton = null;
            }

            if (Hazne1 != null) {
                Hazne1.Dispose ();
                Hazne1 = null;
            }

            if (MentionLabel != null) {
                MentionLabel.Dispose ();
                MentionLabel = null;
            }

            if (ProgressHazne != null) {
                ProgressHazne.Dispose ();
                ProgressHazne = null;
            }
        }
    }
}