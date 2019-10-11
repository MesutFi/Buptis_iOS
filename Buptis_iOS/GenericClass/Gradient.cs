using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Buptis_iOS.GenericClass
{
    [Register("UIGradientView")]
    public class UIGradientView : UIView
    {
        [Export("InsideColor")]
        public UIColor InsideColor { get; set; } = UIColor.FromRGB(48, 43, 99);

        [Export("OutsideColor")]
        public UIColor OutsideColor { get; set; } = UIColor.FromRGB(36, 36, 62);

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            var colors = new CGColor[] { InsideColor.CGColor, OutsideColor.CGColor };

            var center = new CGPoint(Bounds.Size.Width / 3, 90);

            var endRadius = Math.Sqrt(Math.Pow(Bounds.Size.Width / 3 * 2, 2) + Math.Pow(Frame.Height - 90, 2));

            var gradient = new CGGradient(null, colors);

            using (var context = UIGraphics.GetCurrentContext())
            {
                context.DrawRadialGradient(
                    gradient: gradient,
                    startCenter: center,
                    startRadius: 0,
                    endCenter: center,
                    endRadius: (nfloat)endRadius,
                    options: CGGradientDrawingOptions.DrawsBeforeStartLocation);
            }
        }

        public UIGradientView(IntPtr handle) : base(handle)
        {
        }
    }
}