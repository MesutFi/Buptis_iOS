// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Buptis_iOS.PrivateProfile
{
    [Register ("CustomImageTableCell")]
    partial class CustomImageTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DeleteButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HiddenButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageViewCell { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DeleteButton != null) {
                DeleteButton.Dispose ();
                DeleteButton = null;
            }

            if (HiddenButton != null) {
                HiddenButton.Dispose ();
                HiddenButton = null;
            }

            if (ImageViewCell != null) {
                ImageViewCell.Dispose ();
                ImageViewCell = null;
            }
        }
    }
}