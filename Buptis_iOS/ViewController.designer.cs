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
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InformationButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (InformationButton != null) {
                InformationButton.Dispose ();
                InformationButton = null;
            }
        }
    }
}