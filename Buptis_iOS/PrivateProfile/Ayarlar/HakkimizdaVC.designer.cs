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
    [Register ("HakkimizdaVC")]
    partial class HakkimizdaVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GizlilikButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HakkindaLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView HeaderView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SartlarButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (GizlilikButton != null) {
                GizlilikButton.Dispose ();
                GizlilikButton = null;
            }

            if (HakkindaLabel != null) {
                HakkindaLabel.Dispose ();
                HakkindaLabel = null;
            }

            if (HeaderView != null) {
                HeaderView.Dispose ();
                HeaderView = null;
            }

            if (SartlarButton != null) {
                SartlarButton.Dispose ();
                SartlarButton = null;
            }
        }
    }
}