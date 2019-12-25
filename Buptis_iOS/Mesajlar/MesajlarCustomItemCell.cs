using System;
using System.Collections.Generic;
using Buptis_iOS.Database;
using Buptis_iOS.Web_Service;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using Newtonsoft.Json;
using UIKit;
using static Buptis_iOS.Mesajlar.MesajlarBaseVC;


namespace Buptis_iOS.Mesajlar
{
    public partial class MesajlarCustomItemCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MesajlarCustomItemCell");
        public static readonly UINib Nib;
        MesajKisileri mesajKisileri;
        List<string> FollowListID;
        UITableViewSource GelenAdapter1;
        NSIndexPath CellIndexPath;
        static MesajlarCustomItemCell()
        {
            Nib = UINib.FromName("MesajlarCustomItemCell", NSBundle.MainBundle);
        }

        protected MesajlarCustomItemCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public static MesajlarCustomItemCell Create(List<string> FollowListt, UITableViewSource GelenAdapter2, NSIndexPath CellIndexPath2)
        {
            var OlusanView = (MesajlarCustomItemCell)Nib.Instantiate(null, null)[0];
            OlusanView.FollowListID = FollowListt;
            OlusanView.GelenAdapter1 = GelenAdapter2;
            OlusanView.CellIndexPath = CellIndexPath2;
            return OlusanView;
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Design();

            FavorilerIcon.TouchUpInside += FavorilerIcon_TouchUpInside;
        }

        private void FavorilerIcon_TouchUpInside(object sender, EventArgs e)
        {
            var MeDTO = DataBase.MEMBER_DATA_GETIR()[0];
            WebService webService = new WebService();
            FavoriDTO favoriDTO = new FavoriDTO()
            {
                userId = MeDTO.id,
                favUserId = mesajKisileri.receiverId
            };
            string jsonString = JsonConvert.SerializeObject(favoriDTO);
            var IsFollow = FollowListID.FindAll(item => item == mesajKisileri.receiverId.ToString());
            if (IsFollow.Count > 0)//Fav varmış Kaldır
            {
               InvokeOnMainThread(delegate ()
                {
                    if (FavEkleKaldir(jsonString))
                    {
                        FavorilerIcon.SetImage(UIImage.FromBundle("Images/fav_pasif.png"), UIControlState.Normal);
                        FavListeGuncelle(false);
                        return;
                    }

                });
            }
            else//Fav yokmus Ekle
            {
                InvokeOnMainThread(delegate ()
                {
                    if (FavEkleKaldir(jsonString))
                    {
                        FavorilerIcon.SetImage(UIImage.FromBundle("Images/fav_aktif.png"), UIControlState.Normal);
                        FavListeGuncelle(true);
                    }

                });
            }
        }
        void FavListeGuncelle(bool eklekaldir)
        {
            if (GelenAdapter1 is MesajlarNormal.MesajlarCustomTableCellSoruce)
            {
                ((MesajlarNormal.MesajlarCustomTableCellSoruce)GelenAdapter1).FavListGuncelle(mesajKisileri.receiverId.ToString(), eklekaldir,CellIndexPath);
            }
            else if (GelenAdapter1 is MesajlarIstekler.MesajlarCustomTableCellSoruce)
            {
                ((MesajlarIstekler.MesajlarCustomTableCellSoruce)GelenAdapter1).FavListGuncelle(mesajKisileri.receiverId.ToString(), eklekaldir, CellIndexPath);

            }
            else if (GelenAdapter1 is MesajlarFavoriler.MesajlarCustomTableCellSoruce)
            {
                ((MesajlarFavoriler.MesajlarCustomTableCellSoruce)GelenAdapter1).FavListGuncelle(mesajKisileri.receiverId.ToString());
            }
        }
        
        bool FavEkleKaldir(string jsonString)
        {
            WebService webService = new WebService();
            var Donus = webService.ServisIslem("users/fav", jsonString);
            if (Donus != "Hata")
            {
                return true;
            }
            else
            {
                //AlertHelper.AlertGoster("Bir Sorun Oluştu.", mContext);
                return false;
            }
        }
        public void UpdateCell(MesajKisileri mesajKisileri2,bool secim)
        {
            mesajKisileri = mesajKisileri2;
            GetUserInfo(secim);
        }
        void GetUserInfo(bool invisibleButton)
        {
            OkunmamisMesajCount.Hidden = true;
            UserName.Text = mesajKisileri.firstName + " " + mesajKisileri.lastName.Substring(0, 1).ToString() + ".";
            GetUserImage(mesajKisileri.receiverId.ToString(), UserImg);
            FavoriFilter();
            var Boll = mesajKisileri.lastChatText.Split('#');
            if (Boll.Length <= 1)
            {
                SonMesajText.Text = mesajKisileri.lastChatText;
            }
            else
            {
                SonMesajText.Text = "Hediye";
            }
            if (mesajKisileri.unreadMessageCount > 0)
            {

                OkunmamisMesajCount.Hidden = false;
                OkunmamisMesajCount.Text = mesajKisileri.unreadMessageCount.ToString();
            }
            if (!invisibleButton)
            {
                
            }
            
        }
        void GetUserImage(string USERID, UIImageView UserImage)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("images/user/" + USERID);
                if (Donus != null)
                {
                    BeginInvokeOnMainThread(delegate () {

                        var Images = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserImageDTO>>(Donus.ToString());
                        if (Images.Count > 0)
                        {
                            ImageService.Instance.LoadUrl(CDN.CDN_Path + Images[Images.Count - 1].imagePath).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Transform(new CircleTransformation(15, "#FFFFFF")).Into(UserImage);
                        }
                    });
                }
            })).Start();
        }
        public void Design()
        {
            this.ContentView.BackgroundColor = UIColor.Clear;
            UserImg.Layer.CornerRadius = UserImg.Frame.Height / 2;
            UserImg.Layer.BorderColor = UIColor.White.CGColor;
            UserImg.Layer.BorderWidth = 3f;
            UserImg.ClipsToBounds = true;

            FavorilerIcon.Layer.CornerRadius = UserName.Frame.Height / 2;
            FavorilerIcon.ClipsToBounds = true;
            FavorilerIcon.ContentEdgeInsets = new UIEdgeInsets(2, 2, 2, 2);

            OkunmamisMesajCount.Layer.CornerRadius = OkunmamisMesajCount.Frame.Height / 2;
            OkunmamisMesajCount.ClipsToBounds = true;
        }
        void FavoriFilter()
        {
            FavorilerIcon.BackgroundColor = UIColor.Clear;
            FavorilerIcon.Layer.BorderWidth = 0;
            FavorilerIcon.Layer.BorderColor = UIColor.Clear.CGColor;
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                var IsFollow = Array.IndexOf(FollowListID.ToArray(),mesajKisileri.receiverId.ToString());
                if (IsFollow  != -1)
                {
                    InvokeOnMainThread(delegate () {
                        FavorilerIcon.SetImage(UIImage.FromBundle("Images/fav_aktif.png"), UIControlState.Normal);
                    });
                }
                else
                {
                    InvokeOnMainThread(delegate () {
                        FavorilerIcon.SetImage(UIImage.FromBundle("Images/fav_pasif.png"), UIControlState.Normal);
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
        public class FavoriDTO
        {
            public int favUserId { get; set; }
            public int userId { get; set; }
        }
    }
}
