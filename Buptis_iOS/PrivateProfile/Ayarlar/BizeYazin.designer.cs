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
    [Register ("BizeYazin")]
    partial class BizeYazin
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Backbutton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GonderButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Headerview { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView MesajTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Backbutton != null) {
                Backbutton.Dispose ();
                Backbutton = null;
            }

            if (GonderButton != null) {
                GonderButton.Dispose ();
                GonderButton = null;
            }

            if (Headerview != null) {
                Headerview.Dispose ();
                Headerview = null;
            }

            if (MesajTextView != null) {
                MesajTextView.Dispose ();
                MesajTextView = null;
            }

            if (textField != null) {
                textField.Dispose ();
                textField = null;
            }
        }
    }
}