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
    [Register ("CoktanSecmeliSoruVC")]
    partial class CoktanSecmeliSoruVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SoruLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView VerticalScroll { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SoruLabel != null) {
                SoruLabel.Dispose ();
                SoruLabel = null;
            }

            if (VerticalScroll != null) {
                VerticalScroll.Dispose ();
                VerticalScroll = null;
            }
        }
    }
}