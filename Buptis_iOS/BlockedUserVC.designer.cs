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
    [Register ("BlockedUserVC")]
    partial class BlockedUserVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView BlockedScrollView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BlockedScrollView != null) {
                BlockedScrollView.Dispose ();
                BlockedScrollView = null;
            }
        }
    }
}