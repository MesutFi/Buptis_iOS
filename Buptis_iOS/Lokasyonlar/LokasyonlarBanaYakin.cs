using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.LokasyondakiKisiler;
using Buptis_iOS.LokasyonDetay;
using Buptis_iOS.Lokasyonlar;
using Buptis_iOS.Web_Service;
using Foundation;
using ObjCRuntime;
using SQLite;
using System;
using System.Collections.Generic;
using UIKit;

namespace Buptis_iOS
{
    public partial class LokasyonlarBanaYakin : UIView
    {
        #region Tanimlamalar
        List<Mekanlar_Location> Mekanlar_Locations = new List<Mekanlar_Location>();
        LokasyonlarBaseVC GelenBase1;
        #endregion
        public LokasyonlarBanaYakin (IntPtr handle) : base (handle)
        {
        }
        public static LokasyonlarBanaYakin Create(LokasyonlarBaseVC GelenBase2)
        {
            var arr = NSBundle.MainBundle.LoadNib("LokasyonlarBanaYakin", null, null);
            var v = Runtime.GetNSObject<LokasyonlarBanaYakin>(arr.ValueAt(0));
            v.BackgroundColor = UIColor.Clear;
            v.GelenBase1 = GelenBase2;
            v.Tablo.BackgroundColor = UIColor.Clear;
            v.Tablo.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            v.Tablo.TableFooterView = new UIView();
            return v;
        }
        public void RowSelectt(Mekanlar_Location GelenMekan)
        {
            var LokasyonDetayStory = UIStoryboard.FromName("LokasyonDetayBaseVC", NSBundle.MainBundle);
            LokasyonDetayBaseVC controller = LokasyonDetayStory.InstantiateViewController("LokasyonDetayBaseVC") as LokasyonDetayBaseVC;
            controller.GelenMekan = GelenMekan;
            GelenBase1.PresentViewController(controller, true, null);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CustomLoading.Show(GelenBase1, "Lokasyonlar Y�kleniyor...");
            
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                BanaYakinLokasyonlariGetir();

            })).Start();

        }
        void BanaYakinLokasyonlariGetir()
        {
            WebService webService = new WebService();
            var x = UserLocationn.UserLoc.Coordinate.Latitude.ToString().Replace(",", ".");
            var y = UserLocationn.UserLoc.Coordinate.Longitude.ToString().Replace(",", ".");
            var Donus = webService.OkuGetir("locations/near?x=" + x + "&y=" + y);
            if (Donus != null)
            {
                var aa = Donus.ToString();
                Mekanlar_Locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Mekanlar_Location>>(Donus.ToString());
                if (Mekanlar_Locations.Count > 0)
                {
                    InvokeOnMainThread(delegate () {
                        Tablo.Source = new LokasyonlarCustomTableCellSoruce(Mekanlar_Locations, this);
                        Tablo.ReloadData();
                        Tablo.BackgroundColor = UIColor.Clear;
                        Tablo.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                        CustomLoading.Hide();
                    });
                }
                else
                {
                    CustomAlert.GetCustomAlert(GelenBase1, "�evrenizde hi� lokasyon bulunamad�...");
                    InvokeOnMainThread(delegate () {
                        Tablo.BackgroundColor = UIColor.Clear;
                        Tablo.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                        Tablo.TableFooterView = new UIView();
                        CustomLoading.Hide();
                    });
                }
            }
            else
            {
                CustomLoading.Hide();
            }
        }
        class LokasyonlarCustomTableCellSoruce : UITableViewSource
        {
            List<Mekanlar_Location> TableItems;
            LokasyonlarBanaYakin LokasyonlarBanaYakin1;
            public LokasyonlarCustomTableCellSoruce(List<Mekanlar_Location> mekanlist, LokasyonlarBanaYakin LokasyonlarBanaYakin2)
            {
                TableItems = mekanlist;
                LokasyonlarBanaYakin1 = LokasyonlarBanaYakin2;
            }
            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Count;
            }
            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 144;

            }
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {

                var itemss = TableItems[indexPath.Row];
                var cell = (LokasyonlarTableCell)tableView.DequeueReusableCell(LokasyonlarTableCell.Key);
                if (cell == null)
                {
                    cell = LokasyonlarTableCell.Create();
                }
                cell.UpdateCell(itemss);
               return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                tableView.DeselectRow(indexPath, true);
                LokasyonlarBanaYakin1.RowSelectt(TableItems[indexPath.Row]);
            }
        }
        
    }
}