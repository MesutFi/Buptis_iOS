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
    [Register ("EngelleCustomRadioItem")]
    partial class EngelleCustomRadioItem
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HiddenButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RadioButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Titlee { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HiddenButton != null) {
                HiddenButton.Dispose ();
                HiddenButton = null;
            }

            if (RadioButton != null) {
                RadioButton.Dispose ();
                RadioButton = null;
            }

            if (Titlee != null) {
                Titlee.Dispose ();
                Titlee = null;
            }
        }
    }
}