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
    [Register ("GalleryView")]
    partial class GalleryView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ImageBackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ImageOkButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ImageScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ViewHazne { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ImageBackButton != null) {
                ImageBackButton.Dispose ();
                ImageBackButton = null;
            }

            if (ImageOkButton != null) {
                ImageOkButton.Dispose ();
                ImageOkButton = null;
            }

            if (ImageScrollView != null) {
                ImageScrollView.Dispose ();
                ImageScrollView = null;
            }

            if (ViewHazne != null) {
                ViewHazne.Dispose ();
                ViewHazne = null;
            }
        }
    }
}