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
    [Register ("LokasyonlarBiryersec")]
    partial class LokasyonlarBiryersec
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Mapview { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView Scrolll { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Mapview != null) {
                Mapview.Dispose ();
                Mapview = null;
            }

            if (Scrolll != null) {
                Scrolll.Dispose ();
                Scrolll = null;
            }
        }
    }
}