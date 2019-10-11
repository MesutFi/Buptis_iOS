using System;

using Foundation;
using UIKit;

namespace Buptis_iOS.PrivateProfile.Ayarlar
{
    public partial class BlockedUserTableCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("BlockedUserTableCell");
        public static readonly UINib Nib;

        static BlockedUserTableCell()
        {
            Nib = UINib.FromName("BlockedUserTableCell", NSBundle.MainBundle);
        }

        protected BlockedUserTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
