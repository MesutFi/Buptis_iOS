using Buptis_iOS.Web_Service;
using CoreGraphics;
using FFImageLoading;
using FFImageLoading.Work;
using Foundation;
using ObjCRuntime;
using System;
using UIKit;
using static Buptis_iOS.PublicProfileVC;

namespace Buptis_iOS
{
    public partial class PublicProfilePhotoView : UIView
    {
        public PublicProfilePhotoView (IntPtr handle) : base (handle)
        {
        }
        UserGalleryDataModel userPhoto;
        public static PublicProfilePhotoView Create(UserGalleryDataModel userPhoto2)
        {
            var arr = NSBundle.MainBundle.LoadNib("PublicProfilePhotoView", null, null);
            var v = Runtime.GetNSObject<PublicProfilePhotoView>(arr.ValueAt(0));
            v.BackgroundColor = UIColor.Clear;
            v.PhotoView.ClipsToBounds = true;
            v.userPhoto = userPhoto2;
            return v;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            PhotoView.ClipsToBounds = true;
            setPhoto();
        }

        void setPhoto()
        {
            ImageService.Instance.LoadUrl(CDN.CDN_Path+userPhoto.imagePath).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Into(PhotoView);

        }


        public void UpdateCell(string ImagesPath)
        {

            PhotoView.Image = UIImage.FromBundle(ImagesPath);
            PhotoView.ClipsToBounds = true;
            PhotoView.ContentMode = UIViewContentMode.ScaleAspectFill;
        }

    }
}