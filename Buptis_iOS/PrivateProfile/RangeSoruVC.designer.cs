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
    [Register ("RangeSoruVC")]
    partial class RangeSoruVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Hazne2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider SliderControll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SoruLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CountLabel != null) {
                CountLabel.Dispose ();
                CountLabel = null;
            }

            if (Hazne2 != null) {
                Hazne2.Dispose ();
                Hazne2 = null;
            }

            if (SliderControll != null) {
                SliderControll.Dispose ();
                SliderControll = null;
            }

            if (SoruLabel != null) {
                SoruLabel.Dispose ();
                SoruLabel = null;
            }
        }
    }
}