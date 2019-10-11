// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Buptis_iOS.PrivateProfile.Ayarlar
{
    [Register ("BlockedUserTableCell")]
    partial class BlockedUserTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BlockedUserName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView BlockedUserPhoto { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BlockedUserName != null) {
                BlockedUserName.Dispose ();
                BlockedUserName = null;
            }

            if (BlockedUserPhoto != null) {
                BlockedUserPhoto.Dispose ();
                BlockedUserPhoto = null;
            }
        }
    }
}