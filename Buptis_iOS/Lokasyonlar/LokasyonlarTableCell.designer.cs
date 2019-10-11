// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Buptis_iOS.Lokasyonlar
{
    [Register ("LokasyonlarTableCell")]
    partial class LokasyonlarTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BeyazArkaHazne { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DolulukBgView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView DolulukProgress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MekanAdiLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MekanRating { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MekanTur { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MekanYer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BeyazArkaHazne != null) {
                BeyazArkaHazne.Dispose ();
                BeyazArkaHazne = null;
            }

            if (DolulukBgView != null) {
                DolulukBgView.Dispose ();
                DolulukBgView = null;
            }

            if (DolulukProgress != null) {
                DolulukProgress.Dispose ();
                DolulukProgress = null;
            }

            if (MekanAdiLabel != null) {
                MekanAdiLabel.Dispose ();
                MekanAdiLabel = null;
            }

            if (MekanRating != null) {
                MekanRating.Dispose ();
                MekanRating = null;
            }

            if (MekanTur != null) {
                MekanTur.Dispose ();
                MekanTur = null;
            }

            if (MekanYer != null) {
                MekanYer.Dispose ();
                MekanYer = null;
            }
        }
    }
}