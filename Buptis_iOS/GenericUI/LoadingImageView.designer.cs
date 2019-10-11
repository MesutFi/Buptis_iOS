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
    [Register ("LoadingImageView")]
    partial class LoadingImageView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AciklamaLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ArkaHazne { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView LoadingimView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AciklamaLabel != null) {
                AciklamaLabel.Dispose ();
                AciklamaLabel = null;
            }

            if (ArkaHazne != null) {
                ArkaHazne.Dispose ();
                ArkaHazne = null;
            }

            if (LoadingimView != null) {
                LoadingimView.Dispose ();
                LoadingimView = null;
            }
        }
    }
}