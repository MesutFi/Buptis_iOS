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
    [Register ("GirisYapBaseVC")]
    partial class GirisYapBaseVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InformationButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollHazne { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (InformationButton != null) {
                InformationButton.Dispose ();
                InformationButton = null;
            }

            if (ScrollHazne != null) {
                ScrollHazne.Dispose ();
                ScrollHazne = null;
            }
        }
    }
}