using System;
using System.Collections.Generic;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using FFImageLoading;
using FFImageLoading.Work;
using Foundation;
using UIKit;
using static Buptis_iOS.GalleryView;

namespace Buptis_iOS.PrivateProfile
{
    
    public partial class CustomImageTableCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("CustomImageTableCell");
        public static readonly UINib Nib;
        UIImagePickerController picker;
        GalleryView BaseView;
        GalleryDataModel GelenDTO;
       
        static CustomImageTableCell()
        {
            Nib = UINib.FromName("CustomImageTableCell", NSBundle.MainBundle);
        }

        protected CustomImageTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public static CustomImageTableCell Create(GalleryView BaseView2, GalleryDataModel GelenDTO)
        {
            var OlusanView = (CustomImageTableCell)Nib.Instantiate(null, null)[0];
            OlusanView.BaseView = BaseView2;
            OlusanView.GelenDTO = GelenDTO;
            return OlusanView;

        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            HiddenButton.TouchUpInside += HiddenButton_TouchUpInside;
            DeleteButton.TouchUpInside += DeleteButton_TouchUpInside;
            //this.Add(HiddenButton);
        }
      
        private void DeleteButton_TouchUpInside(object sender, EventArgs e)
        {
            UIAlertView alert = new UIAlertView();
            alert.Title = "Buptis";
            alert.AddButton("Evet");
            alert.AddButton("Hayır");
            alert.Message = "Fotoğrafı silmek istediğinize emin misiniz?";
            alert.AlertViewStyle = UIAlertViewStyle.Default;
            alert.Clicked += (object s, UIButtonEventArgs ev) =>
            {
                if (ev.ButtonIndex == 0)
                {
                    alert.Dispose();
                   
                    WebService webService = new WebService();
                    var Donus = webService.ServisIslem("images/" + GelenDTO.id, "", Method: "DELETE");
                    if (Donus != "Hata")
                    {
                        BaseView.FotografSildiktenSonraGuncelle();
                    }

                }
                else
                {
                    alert.Dispose();
                }
            };
            alert.Show();
            

        }
       
        private void HiddenButton_TouchUpInside(object sender, EventArgs e)
        {

            picker = new UIImagePickerController();
            picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            picker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
            picker.FinishedPickingMedia += Picker_FinishedPickingMedia;
            picker.Canceled += Picker_Canceled;
            BaseView.PresentModalViewController(picker, true);
        }

        public void UpdateCell()
        {
            Design();
            
        }
        
        private void Picker_Canceled(object sender, EventArgs e)
        {
            picker.DismissModalViewController(true);
        }

        private void Picker_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            bool isImage = false;
            switch (e.Info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image":
                    isImage = true;
                    break;
                case "public.video":
                    break;
            }
            NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
            if (referenceURL != null) Console.WriteLine("Url:" + referenceURL.ToString());
            if (isImage)
            {
                UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                if (originalImage != null)
                {
                    BaseView.SetPhoto(originalImage);
                }
            }
            else
            {
                NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
                if (mediaURL != null)
                {
                    Console.WriteLine(mediaURL.ToString());
                }
            }
            picker.DismissModalViewController(true);
        }

        #region UI Tasarim
        public void Design()
        {
           
            DeleteButton.BackgroundColor = UIColor.FromRGB(226, 0, 93);
            DeleteButton.Layer.CornerRadius = DeleteButton.Frame.Height / 2;
            DeleteButton.ClipsToBounds = true;
            DeleteButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            ImageViewCell.Layer.CornerRadius = 15f;
            ImageViewCell.ClipsToBounds = true;
            GetPhoto();

            if (GelenDTO.isAddedCell)
            {
                DeleteButton.Hidden = true;
            }
            
        }

        void GetPhoto()
        {
         ImageService.Instance.LoadUrl(CDN.CDN_Path+GelenDTO.imagePath).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Into(ImageViewCell);

        }

        #endregion
    }
}
