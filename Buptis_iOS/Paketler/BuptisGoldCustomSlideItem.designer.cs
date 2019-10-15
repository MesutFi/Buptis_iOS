// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Buptis_iOS.Paketler
{
    [Register ("BuptisGoldCustomSlideItem")]
    partial class BuptisGoldCustomSlideItem
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView SlideImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SlideMetin { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SlideImage != null) {
                SlideImage.Dispose ();
                SlideImage = null;
            }

            if (SlideMetin != null) {
                SlideMetin.Dispose ();
                SlideMetin = null;
            }
        }
    }
}