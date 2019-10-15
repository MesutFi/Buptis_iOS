using System;

using Foundation;
using UIKit;

namespace Buptis_iOS.Paketler
{
    public partial class BuptisGoldCustomSlideItem : UITableViewCell
    {
        public static readonly NSString Key = new NSString("BuptisGoldCustomSlideItem");
        public static readonly UINib Nib;

        static BuptisGoldCustomSlideItem()
        {
            Nib = UINib.FromName("BuptisGoldCustomSlideItem", NSBundle.MainBundle);
        }

        protected BuptisGoldCustomSlideItem(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public static BuptisGoldCustomSlideItem Create(string imagee,string aciklama)
        {
            var OlusanView = (BuptisGoldCustomSlideItem)Nib.Instantiate(null, null)[0];
            OlusanView.SlideImage.Image = UIImage.FromBundle("Images/" + imagee);
            OlusanView.SlideMetin.Text = aciklama;
            return OlusanView;

        }
    }
}
