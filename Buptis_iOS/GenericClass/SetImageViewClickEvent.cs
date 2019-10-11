using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Buptis_iOS.GenericClass
{
    public static class SetImageViewClickEvent
    {
        public static void SetClick(UIViewController HedefClass, UIImageView GelenImage,string MetodAdi)
        {
            GelenImage.UserInteractionEnabled = true;
            var tapGesture = new UITapGestureRecognizer(HedefClass,
                new ObjCRuntime.Selector(MetodAdi + ":"))
            {
                NumberOfTapsRequired = (nuint)new Random().Next(1, 99999)
            };
            GelenImage.AddGestureRecognizer(tapGesture);
        }
    }
}