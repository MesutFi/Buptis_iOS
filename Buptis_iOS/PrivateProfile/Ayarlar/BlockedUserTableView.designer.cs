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
    [Register ("BlockedUserTableView")]
    partial class BlockedUserTableView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BlockedName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView BlockedPhoto { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BlockedName != null) {
                BlockedName.Dispose ();
                BlockedName = null;
            }

            if (BlockedPhoto != null) {
                BlockedPhoto.Dispose ();
                BlockedPhoto = null;
            }
        }
    }
}