using System;
using Buptis_iOS.Web_Service;
using FFImageLoading;
using FFImageLoading.Work;
using Foundation;
using UIKit;
using static Buptis_iOS.HediyeGonderVC;

namespace Buptis_iOS.Mesajlar.ChatDetay
{
    public partial class HediyelerCustomListItem : UITableViewCell
    {
        public static readonly NSString Key = new NSString("HediyelerCustomListItem");
        public static readonly UINib Nib;
        HediyeGonderVC BaseView;
        HediyelerDataModel GelenDTO;
        static HediyelerCustomListItem()
        {
            Nib = UINib.FromName("HediyelerCustomListItem", NSBundle.MainBundle);
        }

        protected HediyelerCustomListItem(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static HediyelerCustomListItem Create(HediyeGonderVC BaseView2, HediyelerDataModel GelenDTO)
        {
            var OlusanView = (HediyelerCustomListItem)Nib.Instantiate(null, null)[0];
            OlusanView.BaseView = BaseView2;
            OlusanView.GelenDTO = GelenDTO;
            OlusanView.HediyeImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            return OlusanView;

        }
        

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            HiddenButton.TouchUpInside += HiddenButton_TouchUpInside; 
        }

        private void HiddenButton_TouchUpInside(object sender, EventArgs e)
        {
            BaseView.SecilenImage(GelenDTO);
        }

        public void UpdateCell()
        {
            Design();
        }

        #region UI Tasarim
        public void Design()
        {

            HediyeImageView.Layer.CornerRadius = 15f;
            HediyeImageView.ClipsToBounds = true;
            GetPhoto();
        }
        void GetPhoto()
        {
            ImageService.Instance.LoadUrl(CDN.CDN_Path + GelenDTO.path).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Into(HediyeImageView);

        }

        #endregion
    }
}
