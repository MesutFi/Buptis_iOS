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
using System.Linq;
using UIKit;

namespace Buptis_iOS
{
    public partial class LokasyonlarPopuler : UIView
    {
        #region Tanimlamalar
        List<Mekanlar_Location> Mekanlar_Locations = new List<Mekanlar_Location>();
        LokasyonlarBaseVC GelenBase1;
        List<Mekanlar_Location> populerDatas = new List<Mekanlar_Location>();
        LokasyonDetayBaseVC lokasyonDetay;
        #endregion
        public LokasyonlarPopuler (IntPtr handle) : base (handle)
        {
        }
        public static LokasyonlarPopuler Create(LokasyonlarBaseVC GelenBase2)
        {
            var arr = NSBundle.MainBundle.LoadNib("LokasyonlarPopuler", null, null);
            var v = Runtime.GetNSObject<LokasyonlarPopuler>(arr.ValueAt(0));
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
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            GelenBase1.PresentViewController(controller, true, null);
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CustomLoading.Show(GelenBase1, "Lokasyonlar Yükleniyor...");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                PopulerLokasyonlariGetir();

            })).Start();
        }
        void PopulerLokasyonlariGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("locations");
            if (Donus != null)
            {
                var aa = Donus.ToString();
                Mekanlar_Locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Mekanlar_Location>>(Donus.ToString());
                Mekanlar_Locations = Mekanlar_Locations.OrderBy(o => o.allUserCheckIn).ToList();//Checkin sayýsýna göre sýralýyor.
                Mekanlar_Locations.Reverse();
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
                    CustomAlert.GetCustomAlert(GelenBase1, "Çevrenizde hiç popüler lokasyon bulunamadı...");
                    InvokeOnMainThread(delegate () {
                        
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
            LokasyonlarPopuler lokasyonlarPopuler1;
            public LokasyonlarCustomTableCellSoruce(List<Mekanlar_Location> mekanlist,LokasyonlarPopuler lokasyonlarPopuler)
            {
                TableItems = mekanlist;
                lokasyonlarPopuler1 = lokasyonlarPopuler;
            }
            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Count;
            }
            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 144f;

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
                //tableView.BeginUpdates();
                //tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                //tableView.EndUpdates();
                lokasyonlarPopuler1.RowSelectt(TableItems[indexPath.Row]);
            }
        }
        
    }
}
