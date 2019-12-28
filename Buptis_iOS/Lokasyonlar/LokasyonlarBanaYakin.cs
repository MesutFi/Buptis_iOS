using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.LokasyondakiKisiler;
using Buptis_iOS.LokasyonDetay;
using Buptis_iOS.Lokasyonlar;
using Buptis_iOS.Web_Service;
using CoreLocation;
using Foundation;
using MapKit;
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


        MKMapView CustomMapView;
        CLLocationManager locationManager;
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
            v.locationManager = new CLLocationManager();
            v.locationManager.RequestWhenInUseAuthorization();
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
                BanaYakinLokasyonlariGetir();

            })).Start();

        }
        void BanaYakinLokasyonlariGetir()
        {
            if (locationManager.Location != null)
            {
                WebService webService = new WebService();
                var x = locationManager.Location.Coordinate.Latitude.ToString().Replace(",", ".");
                var y = locationManager.Location.Coordinate.Longitude.ToString().Replace(",", ".");
                InvokeOnMainThread(delegate { LokasyonlarTableCell.UserLastloc = locationManager.Location; });
                
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
                        CustomAlert.GetCustomAlert(GelenBase1, "Çevrenizde hiç lokasyon bulunamadı...");
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
            else
            {
                CustomLoading.Hide();
                CustomAlert.GetCustomAlert(GelenBase1, "Konumunuza Ulaşılamıyor..");
                return;

                var durum = CheckLocationPermission();
                if (!(bool)durum)
                {
                    InvokeOnMainThread(delegate ()
                    {
                        var alert = new UIAlertView();
                        alert.Title = "Buptis";
                        alert.AddButton("Evet");
                        alert.AddButton("Hayır");
                        alert.Message = "Buptis konumunuzu kullanarak çevrenizde size yakın mekanları listelemektedir.\nKonum ayarlarını açmak istiyor musunuz?";
                        alert.AlertViewStyle = UIAlertViewStyle.Default;
                        alert.Clicked += (object s, UIButtonEventArgs ev) =>
                        {
                            if (ev.ButtonIndex == 0)
                            {
                                alert.Dispose();
                                UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                            }
                            else
                            {
                                alert.Dispose();
                            }
                        };
                        alert.Show();
                    });
                }
            }
        }

        bool? CheckLocationPermission()
        {
            var CLAuthorizationStatuss = CLLocationManager.Status;

            if (CLAuthorizationStatuss == CLAuthorizationStatus.NotDetermined ||
                CLAuthorizationStatuss == CLAuthorizationStatus.Restricted ||
                CLAuthorizationStatuss == CLAuthorizationStatus.Denied)
            {
                return false;
            }
            else if (CLAuthorizationStatuss == CLAuthorizationStatus.AuthorizedAlways ||
                     CLAuthorizationStatuss == CLAuthorizationStatus.AuthorizedWhenInUse)
            {
                return true;
            }
            else
            {
                return null;
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
