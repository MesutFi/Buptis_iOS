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
    [Register ("PublicProfilePhotoView")]
    partial class PublicProfilePhotoView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView PhotoView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (PhotoView != null) {
                PhotoView.Dispose ();
                PhotoView = null;
            }
        }
    }
}