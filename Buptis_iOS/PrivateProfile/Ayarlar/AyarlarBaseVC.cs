
using System;
using System.Collections.Generic;
using System.Drawing;
using Buptis_iOS.Database;
using CoreAnimation;
using Foundation;
using UIKit;

namespace Buptis_iOS.PrivateProfile.Ayarlar
{
    public partial class AyarlarBaseVC : UIViewController
    {
        List<AyarlarDataModel> AyarlarDataModel1 = new List<AyarlarDataModel>();
        public AyarlarBaseVC(IntPtr handle) : base(handle)
        {
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            Tablo.BackgroundColor = UIColor.Clear;
            Tablo.TableFooterView = new UIView();
            BackButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            BackButton.TouchUpInside += BackButton_TouchUpInside;
            HeaderTasarim();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FillDataModel();
        }

        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true,null);
        }

        void HeaderTasarim()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = new CoreGraphics.CGRect(0,0,UIScreen.MainScreen.Bounds.Width,126f);
            HeaderView.Layer.InsertSublayer(gradientLayer, 0);
            HeaderView.Layer.CornerRadius = 30;
            HeaderView.ClipsToBounds = true;
            HeaderView.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);
        }

        #endregion

        void FillDataModel()
        {
            AyarlarDataModel1 = new List<AyarlarDataModel> {
                new AyarlarDataModel(){Titlee="Temel Bilgi",Desc = ""},
                new AyarlarDataModel(){Titlee="Hesap",Desc = GetEmail()},
                new AyarlarDataModel(){Titlee="Bize Yazın",Desc = ""},
                new AyarlarDataModel(){Titlee="Hakkımızda",Desc = ""},
                new AyarlarDataModel(){Titlee="Engelli Kullanıcılar",Desc = ""},
            };

            Tablo.Source = new TableSource(AyarlarDataModel1,this);
            Tablo.ReloadData();
        }
        public string GetEmail()
        {
            string email;
            var UserEmail = DataBase.MEMBER_DATA_GETIR()[0].email;
            var Bol = UserEmail.Split('@');
            var IlkHarf = Bol[0].Substring(0, 1);
            var yildizlar = "";
            for (int i = 1; i < Bol[0].Length; i++)
            {
                yildizlar += "*";
            }
            email = IlkHarf + yildizlar + "@" + Bol[1];
            return email;
        }
        public void RowClick(int Index)
        {
            switch (Index)
            {
                case 0:
                    var AyarlarBaseVC1 = UIStoryboard.FromName("AyarlarBaseVC", NSBundle.MainBundle);
                    TemelBilgilerVC controller = AyarlarBaseVC1.InstantiateViewController("TemelBilgilerVC") as TemelBilgilerVC;
                    this.PresentViewController(controller, true, null);
                    break;
                case 1:
                    var AyarlarBaseVC2 = UIStoryboard.FromName("AyarlarBaseVC", NSBundle.MainBundle);
                    HesapVC controller2 = AyarlarBaseVC2.InstantiateViewController("HesapVC") as HesapVC;
                    this.PresentViewController(controller2, true, null);
                    break;
                case 2:
                    var AyarlarBaseVC3 = UIStoryboard.FromName("AyarlarBaseVC", NSBundle.MainBundle);
                    BizeYazin controller3 = AyarlarBaseVC3.InstantiateViewController("BizeYazin") as BizeYazin;
                    this.PresentViewController(controller3, true, null);
                    break;
                case 3:
                    var AyarlarBaseVC4 = UIStoryboard.FromName("AyarlarBaseVC", NSBundle.MainBundle);
                    HakkimizdaVC controller4 = AyarlarBaseVC4.InstantiateViewController("HakkimizdaVC") as HakkimizdaVC;
                    this.PresentViewController(controller4, true, null);
                    break;
                case 4:
                    var AyarlarBaseVC5 = UIStoryboard.FromName("AyarlarBaseVC", NSBundle.MainBundle);
                    BlockedUsersVC controller5 = AyarlarBaseVC5.InstantiateViewController("BlockedUsersVC") as BlockedUsersVC;
                    this.PresentViewController(controller5, true, null);
                    break;
                default:
                    break;
            }
        }

        public class AyarlarDataModel
        {
            public string Titlee { get; set; }
            public string Desc { get; set; }
        }
        public class TableSource : UITableViewSource
        {

            List<AyarlarDataModel> TableItems;
            AyarlarBaseVC GelenBase;
            string CellIdentifier = "TableCell";

            public TableSource(List<AyarlarDataModel> items, AyarlarBaseVC GelenBase2)
            {
                TableItems = items;
                GelenBase = GelenBase2;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
                var item = TableItems[indexPath.Row];

                //---- if there are no cells to reuse, create a new one
                if (cell == null)
                { cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier); }

                cell.TextLabel.TextColor = UIColor.White;
                cell.DetailTextLabel.TextColor = UIColor.White;

                cell.TextLabel.Text = item.Titlee;
                cell.DetailTextLabel.Text = item.Desc;

                if (item.Titlee == "Engelli Kullanıcılar")
                {
                    cell.TextLabel.TextColor = UIColor.White.ColorWithAlpha(0.5f);
                }

                cell.BackgroundColor = UIColor.Clear;

                return cell;
            }
            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                tableView.DeselectRow(indexPath, true);
                GelenBase.RowClick(indexPath.Row);
            }
        }
    }
}