using Foundation;
using ObjCRuntime;
using System;
using UIKit;

namespace Buptis_iOS
{
    public partial class EngelleCustomRadioItem : UIView
    {
        string Title;
        public bool isSelect;
        Action GelenAksiyon1;
        public EngelleCustomRadioItem (IntPtr handle) : base (handle)
        {
        }
        public static EngelleCustomRadioItem Create(string title, Action GelenAksiyon2, bool isSelect = false)
        {
            var arr = NSBundle.MainBundle.LoadNib("EngelleCustomRadioItem", null, null);
            var v = Runtime.GetNSObject<EngelleCustomRadioItem>(arr.ValueAt(0));
            v.BackgroundColor = UIColor.Clear;
            v.Title = title;
            v.isSelect = isSelect;
            v.GelenAksiyon1 = GelenAksiyon2;
            v.HiddenButton.TouchUpInside += (sender, e) => v.GelenAksiyon1();
            v.HiddenButton.TouchUpInside += v.RadioBttn_TouchUpInside;
            return v;
        }
        public override void LayoutSubviews()
        {
            Titlee.Text = Title;
            RadioButton.Selected = isSelect;
            ButtonKontrol();
        }
        
        private void RadioBttn_TouchUpInside(object sender, EventArgs e)
        {
            RadioButton.Selected = !RadioButton.Selected;
            isSelect = RadioButton.Selected;
            ButtonKontrol();
        }
        public void UzaktanErisim(bool secim)
        {
            RadioButton.Selected = secim;
            ButtonKontrol();
        }
        public void ButtonKontrol()
        {
            if (RadioButton.Selected == true)
            {
                RadioSelect();
            }
            else
            {
                RadioUnselect();
            }
        }
        public void RadioSelect()
        {
            RadioButton.Layer.CornerRadius = RadioButton.Frame.Height / 2;
            RadioButton.BackgroundColor = UIColor.FromRGB(232, 0, 79);
            RadioButton.ClipsToBounds = true;
        }
        public void RadioUnselect()
        {
            RadioButton.Layer.CornerRadius = RadioButton.Frame.Height / 2;
            RadioButton.Layer.BorderColor = UIColor.FromRGB(232, 0, 79).CGColor;
            RadioButton.BackgroundColor = UIColor.Clear;
            RadioButton.Layer.BorderWidth = 3f;
            RadioButton.ClipsToBounds = true;
        }
    }
}