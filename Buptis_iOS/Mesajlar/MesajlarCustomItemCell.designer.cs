// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Buptis_iOS.Mesajlar
{
    [Register ("MesajlarCustomItemCell")]
    partial class MesajlarCustomItemCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FavorilerIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel OkunmamisMesajCount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SonMesajText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UserImg { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UserName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FavorilerIcon != null) {
                FavorilerIcon.Dispose ();
                FavorilerIcon = null;
            }

            if (OkunmamisMesajCount != null) {
                OkunmamisMesajCount.Dispose ();
                OkunmamisMesajCount = null;
            }

            if (SonMesajText != null) {
                SonMesajText.Dispose ();
                SonMesajText = null;
            }

            if (UserImg != null) {
                UserImg.Dispose ();
                UserImg = null;
            }

            if (UserName != null) {
                UserName.Dispose ();
                UserName = null;
            }
        }
    }
}