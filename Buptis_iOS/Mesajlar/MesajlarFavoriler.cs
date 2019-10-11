using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Mesajlar;
using Buptis_iOS.Web_Service;
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using static Buptis_iOS.Mesajlar.MesajlarBaseVC;

namespace Buptis_iOS
{
    public partial class MesajlarFavoriler : UIView
    {
        #region Tanimlamalar
        MesajlarBaseVC GelenBase1;
        List<MesajKisileri> mFriends = new List<MesajKisileri>();
        UITextField AraTxt;
        #endregion

        public MesajlarFavoriler (IntPtr handle) : base (handle)
        {
        }

        public static MesajlarFavoriler Create(MesajlarBaseVC GelenBase2, UITextField AraTxt2)
        {
            var arr = NSBundle.MainBundle.LoadNib("MesajlarFavoriler", null, null);
            var v = Runtime.GetNSObject<MesajlarFavoriler>(arr.ValueAt(0));
            v.BackgroundColor = UIColor.Clear;
            v.GelenBase1 = GelenBase2;
            v.AraTxt = AraTxt2;
            v.Tablo.BackgroundColor = UIColor.Clear;
            v.Tablo.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            v.Tablo.TableFooterView = new UIView();
            return v;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CustomLoading.Show(GelenBase1, "Mesajlar Bekleniyor...");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                SonMesajlariGetir();

            })).Start();
            AraTxt.EditingChanged += AraTxt_EditingChanged;
        }

        private void AraTxt_EditingChanged(object sender, EventArgs e)
        {
            var LastText = AraTxt.Text;
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                List<MesajKisileri> searchedFriends = (from friend in mFriends
                                                       where friend.firstName.Contains(LastText, StringComparison.OrdinalIgnoreCase)
                                                       || friend.lastChatText.Contains(LastText, StringComparison.OrdinalIgnoreCase)
                                                       select friend).ToList<MesajKisileri>();
                if (searchedFriends.Count > 0)
                {
                    InvokeOnMainThread(() =>
                    {
                        Tablo.Source = new MesajlarCustomTableCellSoruce(mFriends, this, FavorileriCagir());
                        Tablo.ReloadData();
                    });
                }
                else
                {
                    InvokeOnMainThread(() =>
                    {
                        Tablo.Source = null;
                        Tablo.ReloadData();
                    });
                }
            })).Start();
        }
        public void RowSelectt(MesajKisileri TiklananKisi)
        {
            GetUserInfo(TiklananKisi.receiverId.ToString(), TiklananKisi.key);
        }

        void GetUserInfo(string UserID, string keyy)
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("users/" + UserID);
            if (Donus != null)
            {
                var Userrr = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(Donus.ToString());
                MesajlarIcinSecilenKullanici.Kullanici = Userrr;
                MesajlarIcinSecilenKullanici.key = keyy;

                var LokasyonKisilerStory = UIStoryboard.FromName("MesajlarBaseVC", NSBundle.MainBundle);
                ChatVC controller = LokasyonKisilerStory.InstantiateViewController("ChatVC") as ChatVC;
                this.GelenBase1.PresentViewController(controller, true, null);
            }
        }

        public class MesajlarCustomTableCellSoruce : UITableViewSource
        {
            List<MesajKisileri> TableItems;
            MesajlarFavoriler MesajlarFavoriler1;
            List<string> FavList;
            public MesajlarCustomTableCellSoruce(List<MesajKisileri> mekanlist, MesajlarFavoriler MesajlarFavoriler2, List<string> FavList2)
            {
                TableItems = mekanlist;
                MesajlarFavoriler1 = MesajlarFavoriler2;
                FavList = FavList2;
            }
            public void FavListGuncelle(string UserID)
            {
                var Indexx = TableItems.FindIndex(item => item.receiverId.ToString() == UserID);

                if (Indexx != -1)
                {
                    TableItems.RemoveAt(Indexx);
                    tableView1.ReloadData();
                }
            }
            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Count;
            }
            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 92f;

            }
            UITableView tableView1;
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                tableView1 = tableView;
                var itemss = TableItems[indexPath.Row];
                var cell = (MesajlarCustomItemCell)tableView.DequeueReusableCell(MesajlarCustomItemCell.Key);
                if (cell == null)
                {
                    cell = MesajlarCustomItemCell.Create(FavList, this);
                }
                cell.UpdateCell(itemss,false);
                return cell;
            }
            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                tableView.DeselectRow(indexPath, true);
                var item = TableItems[indexPath.Row];
                MesajlarFavoriler1.RowSelectt(item);
            }
        }
        void SonMesajlariGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("chats/user");
            if (Donus != null)
            {
                var MeID = DataBase.MEMBER_DATA_GETIR()[0].id;
                var aa = Donus.ToString();
                mFriends = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MesajKisileri>>(Donus.ToString());
                //mFriends = mFriends.FindAll(item => item.request == true);
                FavorileriAyir();
                if (mFriends.Count > 0)
                {

                    mFriends.Where(item => item.receiverId == MeID).ToList().ForEach(item2 => item2.unreadMessageCount = 0);
                    SaveKeys();
                    InvokeOnMainThread(() =>
                    {
                        //var boldd = Typeface.CreateFromAsset(this.Activity.Assets, "Fonts/muliBold.ttf");
                        //var normall = Typeface.CreateFromAsset(this.Activity.Assets, "Fonts/muliRegular.ttf");
                        Tablo.Source = new MesajlarCustomTableCellSoruce(mFriends, this, FavorileriCagir());
                        Tablo.ReloadData();
                        Tablo.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                        Tablo.TableFooterView = new UIView();
                        CustomLoading.Hide();
                    });
                }
                else
                {
                    CustomAlert.GetCustomAlert(GelenBase1, "Hiç Mesaj Bulunamadý...");
                    CustomLoading.Hide();
                }
            }
            else
            {
                CustomLoading.Hide();
            }
        }
        void SaveKeys()
        {
            var LocalKeys = DataBase.CHAT_KEYS_GETIR();
            if (LocalKeys.Count > 0)
            {
                for (int i = 0; i < mFriends.Count; i++)
                {
                    var KeyKarsilastirmaDurum = LocalKeys.FindAll(item => item.UserID == mFriends[i].receiverId);
                    if (KeyKarsilastirmaDurum.Count > 0)
                    {
                        if (KeyKarsilastirmaDurum[KeyKarsilastirmaDurum.Count - 1].MessageKey != mFriends[i].key)
                        {
                            //Güncelle
                            DataBase.CHAT_KEYS_Guncelle(new CHAT_KEYS()
                            {
                                UserID = KeyKarsilastirmaDurum[KeyKarsilastirmaDurum.Count - 1].UserID,
                                MessageKey = mFriends[i].key
                            });

                        }
                        else
                        {
                            //Eþitse birþey yapma
                            continue;
                        }
                    }
                    else
                    {
                        DataBase.CHAT_KEYS_EKLE(new CHAT_KEYS()
                        {
                            UserID = KeyKarsilastirmaDurum[KeyKarsilastirmaDurum.Count - 1].UserID,
                            MessageKey = mFriends[i].key
                        });
                        //Hiç Yok Yeni Ekle
                    }
                }
            }
        }
        void FavorileriAyir()
        {
            var FavList = FavorileriCagir();
            List<FavListDTO> newList = new List<FavListDTO>();
            for (int i = 0; i < FavList.Count; i++)
            {
                newList.Add(new FavListDTO()
                {
                    FavUserID = Convert.ToInt32(FavList[i])
                });
            }
            var Ayiklanmis = (from list1 in mFriends
                              join list2 in newList
                              on list1.receiverId equals list2.FavUserID
                              select list1).ToList();
            mFriends = Ayiklanmis;
        }
        List<string> FavorileriCagir()
        {
            List<string> FollowListID = new List<string>();
            WebService webService = new WebService();
            var MeDTO = DataBase.MEMBER_DATA_GETIR()[0];
            var Donus4 = webService.OkuGetir("users/favList/" + MeDTO.id.ToString());
            if (Donus4 != null)
            {
                var JSONStringg = Donus4.ToString().Replace("[", "").Replace("]", "").Replace(" ", "");
                if (!string.IsNullOrEmpty(JSONStringg))
                {
                    FollowListID = JSONStringg.Split(',').ToList();
                    FollowListID.ForEach(item => item.Trim());
                }
            }
            return FollowListID;
        }
        public class FavListDTO
        {
            public int FavUserID { get; set; }
        }
        class SonFavorilerListViewDataModel
        {
            public string firstName { get; set; }
            public string key { get; set; }
            public string lastChatText { get; set; }
            public string lastModifiedDate { get; set; }
            public string lastName { get; set; }
            public int receiverId { get; set; }
            public bool request { get; set; }
            public int unreadMessageCount { get; set; }
        }
    }
}