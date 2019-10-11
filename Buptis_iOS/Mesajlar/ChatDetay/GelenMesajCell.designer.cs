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
    [Register ("GelenMesajCell")]
    partial class GelenMesajCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView MesajTextVieww { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MesajTextVieww != null) {
                MesajTextVieww.Dispose ();
                MesajTextVieww = null;
            }
        }
    }
}