using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.PrivateProfile.Ayarlar;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using UIKit;

namespace Buptis_iOS
{
    public partial class BlockedUsersVC : UIViewController
    {
        List<BlockedUserDataModel> blockedList = new List<BlockedUserDataModel>();
        BlockedUsersVC GelenBase1;
        public BlockedUsersVC (IntPtr handle) : base (handle)
        {

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            HeaderTasarim();
            BackButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            BackButton.TouchUpInside += BackButton_TouchUpInside;
            BlockedTableView.BackgroundColor = UIColor.Clear;
            BlockedTableView.TableFooterView = new UIView();
            GetBlockedUserList();
            BlockedTableView.ReloadData();
            
        }

        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }
        void GetBlockedUserList()
        {
            BlockedTableView.Source = null;
            BlockedTableView.ReloadData();
            WebService webservice = new WebService();
            var donus = webservice.OkuGetir("blocked-user/block-list");
            if (donus!=null)
            {
                blockedList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BlockedUserDataModel>>(donus.ToString());
                if (blockedList.Count>0)
                {
                    InvokeOnMainThread(() => {
                        BlockedTableView.Source = new BlockedCustomTableCellSource(blockedList, this);
                        BlockedTableView.ReloadData();
                        BlockedTableView.BackgroundColor = UIColor.Clear;
                        BlockedTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                        CustomLoading.Hide();
                    });

                }
            }

        }
        public void UnBlockedUser(int id)
        {
            UIAlertView alert = new UIAlertView();
            alert.Title = "Buptis";
            alert.AddButton("Evet");
            alert.AddButton("Hayýr");
            alert.Message = "Kullanýcýnýn engelini kaldýrmak istediðinize emin misiniz ?";
            alert.AlertViewStyle = UIAlertViewStyle.Default;
            alert.Clicked += (object s, UIButtonEventArgs ev) =>
            {
                if (ev.ButtonIndex == 0)
                {
                    alert.Dispose();
                    WebService webService = new WebService();
                    var Donus = webService.ServisIslem("blocked-users/" + id, "", Method: "DELETE");
                    if (Donus != "Hata")
                    {
                        GetBlockedUserList();
                        CustomAlert.GetCustomAlert(this, "Kullanýcýnýn engeli kaldýrýldý");
                    }
                    
                }
                else
                {
                    alert.Dispose();
                }
            };
            alert.Show();


           
        }
        class BlockedCustomTableCellSource : UITableViewSource
        {
            List<BlockedUserDataModel> TableItems;
            BlockedUsersVC BlockedUsers1;
            public BlockedCustomTableCellSource(List<BlockedUserDataModel> mekanlist, BlockedUsersVC BlockedUsers2)
            {
                TableItems = mekanlist;
                BlockedUsers1 = BlockedUsers2;
            }
            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Count;
            }
            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 92f;

            }
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
               
                var itemss = TableItems[indexPath.Row];
                var cell = (BlockedUserTableView)tableView.DequeueReusableCell(BlockedUserTableView.Key);
                if (cell == null)
                {
                    cell = BlockedUserTableView.Create(itemss);
                    
                }

                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
               
                tableView.DeselectRow(indexPath, true);
                BlockedUsers1.UnBlockedUser(TableItems[indexPath.Row].id);
            }
        }
       
        void HeaderTasarim()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 126f);
            HeaderView.Layer.InsertSublayer(gradientLayer, 0);
            HeaderView.Layer.CornerRadius = 30;
            HeaderView.ClipsToBounds = true;
            HeaderView.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);
        }
      
        public class BlockedUserDataModel
        {
            public int blockUserId { get; set; }
            public string createdDate { get; set; }
            public int id { get; set; }
            public string lastModifiedDate { get; set; }
            public string reasonType { get; set; }
            public string status { get; set; }
            public int userId { get; set; }
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