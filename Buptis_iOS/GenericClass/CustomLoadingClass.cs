using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using Foundation;
using UIKit;

namespace Buptis_iOS.GenericClass
{
    public static class CustomLoadingClass
    {
        public static void RotateAnimation(this UIView view, float duration = 1, float rotations = 1, float repeat = int.MaxValue)
        {
            var rotationAnimation = CABasicAnimation.FromKeyPath("transform.rotation.z");
            rotationAnimation.To = new NSNumber(Math.PI * 2.0 /* full rotation*/ * 1 * 1);
            rotationAnimation.Duration = 1;
            rotationAnimation.Cumulative = true;
            rotationAnimation.RepeatCount = int.MaxValue;
            rotationAnimation.RemovedOnCompletion = false;
            view.Layer.AddAnimation(rotationAnimation, "rotationAnimation");
        }
    }
}