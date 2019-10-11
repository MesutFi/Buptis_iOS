using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Buptis_iOS.PrivateProfile.Ayarlar
{
    public class PickerModel : UIPickerViewModel
    {
        private readonly IList<string> values;
        
        UITextField _textField;

        public event EventHandler<PickerChangedEventArgs> PickerChanged;
        public PickerModel(IList<string> values)
        {
            this.values = values;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }
        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return values.Count;
        }
        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return values[(int)row];
        }

        public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
        {
            return 40f;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            if (this.PickerChanged != null)
            {
                this.PickerChanged(this, new PickerChangedEventArgs { SelectedValue = values[(int)row].ToString() });
            }
        }

        public class CustomPickerView: UIView
        {
            public string Soru { set; get; }
            public CustomPickerView(RectangleF frame, string Question)
            {
                Frame = frame;
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                BackgroundColor = UIColor.Clear;
                Question = Soru;
            }
            public override void Draw(CGRect rect)
            {
                base.Draw(rect);
                UITextView txtView = new UITextView(new RectangleF(0, 0, (float)this.Frame.Width, (float)Frame.Height));
                txtView.TextColor = UIColor.White;
                txtView.BackgroundColor = UIColor.Clear;
                txtView.Font = UIFont.SystemFontOfSize(17f);
                txtView.TextAlignment = UITextAlignment.Center;
                txtView.Text = Soru;

                var gradientLayer = new CAGradientLayer();
                gradientLayer.Colors = new[] { UIColor.FromRGB(106, 17, 201).CGColor, UIColor.FromRGB(37, 117, 252).CGColor };
                gradientLayer.Locations = new NSNumber[] { 0, 1 };
                gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 1);
                gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 0);
                gradientLayer.Frame = txtView.Bounds;
                txtView.Layer.CornerRadius = txtView.Frame.Height / 2;
                txtView.Layer.MasksToBounds = true;
                txtView.Layer.InsertSublayer(gradientLayer, 0);

                AddSubview(txtView);
            }
        }

        public class PickerChangedEventArgs : EventArgs
        {
            public string SelectedValue { get; set; }
        }
    }
}