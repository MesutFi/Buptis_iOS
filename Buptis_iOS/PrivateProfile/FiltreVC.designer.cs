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
    [Register ("FiltreVC")]
    partial class FiltreVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BothButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView FilterTabView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ManButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel maxText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OkButon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Xamarin.RangeSlider.RangeSliderControl rangeSlider { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TabViewMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton WomanButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (BothButton != null) {
                BothButton.Dispose ();
                BothButton = null;
            }

            if (FilterTabView != null) {
                FilterTabView.Dispose ();
                FilterTabView = null;
            }

            if (ManButton != null) {
                ManButton.Dispose ();
                ManButton = null;
            }

            if (maxText != null) {
                maxText.Dispose ();
                maxText = null;
            }

            if (OkButon != null) {
                OkButon.Dispose ();
                OkButon = null;
            }

            if (rangeSlider != null) {
                rangeSlider.Dispose ();
                rangeSlider = null;
            }

            if (TabViewMenu != null) {
                TabViewMenu.Dispose ();
                TabViewMenu = null;
            }

            if (WomanButton != null) {
                WomanButton.Dispose ();
                WomanButton = null;
            }
        }
    }
}