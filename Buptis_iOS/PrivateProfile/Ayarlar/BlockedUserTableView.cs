using System;
using System.Collections.Generic;
using Buptis_iOS.Database;
using Buptis_iOS.Web_Service;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using UIKit;
using static Buptis_iOS.BlockedUsersVC;

namespace Buptis_iOS.PrivateProfile.Ayarlar
{
    public partial class BlockedUserTableView : UITableViewCell
    {
        public static readonly NSString Key = new NSString("BlockedUserTableView");
        public static readonly UINib Nib;
        BlockedUserDataModel User;
        static BlockedUserTableView()
        {
            Nib = UINib.FromName("BlockedUserTableView", NSBundle.MainBundle);
        }

        protected BlockedUserTableView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public static BlockedUserTableView Create(BlockedUserDataModel User2)
        {
            var returnView = (BlockedUserTableView)Nib.Instantiate(null, null)[0];
            returnView.User = User2;
          
            return returnView;

        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Tasarim();
            GetUserDTO();

        }
       
        void Tasarim()
        {
            BlockedPhoto.Layer.CornerRadius = BlockedPhoto.Frame.Height / 2;
            BlockedPhoto.Layer.BorderColor = UIColor.White.CGColor;
            BlockedPhoto.Layer.BorderWidth = 3f;
            BlockedPhoto.ClipsToBounds = true;
            ContentView.BackgroundColor = UIColor.Clear;
        }

        void GetUserDTO()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("users/" + User.blockUserId);
                if (Donus != null)
                {
                    InvokeOnMainThread(delegate () {

                        var Userr = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(Donus.ToString());
                        BlockedName.Text = Userr.firstName + " " + Userr.lastName.Substring(0, 1) + ". ";
                        GetUserImage();
                    });
                }
            })).Start();
        }
        void GetUserImage()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("images/user/" + User.blockUserId);
                if (Donus != null)
                {
                    InvokeOnMainThread(delegate () {

                        var Images = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserImageDTO>>(Donus.ToString());
                        if (Images.Count > 0)
                        {
                            ImageService.Instance.LoadUrl(Images[Images.Count - 1].imagePath).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Transform(new CircleTransformation(15, "#FFFFFF")).Into(BlockedPhoto);
                        }
                    });
                }
            })).Start();
        }

        public class UserImageDTO
        {
            public string createdDate { get; set; }
            public int id { get; set; }
            public string imagePath { get; set; }
            public string lastModifiedDate { get; set; }
            public int userId { get; set; }
        }


    }
}
