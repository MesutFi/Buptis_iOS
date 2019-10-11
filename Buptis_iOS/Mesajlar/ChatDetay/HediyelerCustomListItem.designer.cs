// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Buptis_iOS.Mesajlar.ChatDetay
{
    [Register ("HediyelerCustomListItem")]
    partial class HediyelerCustomListItem
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView HediyeImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HiddenButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HediyeImageView != null) {
                HediyeImageView.Dispose ();
                HediyeImageView = null;
            }

            if (HiddenButton != null) {
                HiddenButton.Dispose ();
                HiddenButton = null;
            }
        }
    }
}