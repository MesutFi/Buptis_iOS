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
using static Buptis_iOS.ChatVC;
using static Buptis_iOS.Mesajlar.MesajlarBaseVC;

namespace Buptis_iOS
{
    public partial class MesajlarIstekler : UIView
    {
        #region Tanimlamalar
        MesajlarBaseVC GelenBase1;
        List<MesajKisileri> mFriends = new List<MesajKisileri>();
        UITextField AraTxt;
        #endregion
        public MesajlarIstekler(IntPtr handle) : base(handle)
        {
        }
        public static MesajlarIstekler Create(MesajlarBaseVC GelenBase2, UITextField AraTxt2)
        {
            var arr = NSBundle.MainBundle.LoadNib("MesajlarIstekler", null, null);
            var v = Runtime.GetNSObject<MesajlarIstekler>(arr.ValueAt(0));
            v.BackgroundColor = UIColor.Clear;
            v.GelenBase1 = GelenBase2;
            v.Tablo.BackgroundColor = UIColor.Clear;
            v.Tablo.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            v.Tablo.TableFooterView = new UIView();
            v.AraTxt = AraTxt2;
            v.MeData = DataBase.MEMBER_DATA_GETIR()[0];
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
                        Tablo.Source = new MesajlarCustomTableCellSoruce(searchedFriends, this, FavorileriCagir());
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
        void SonMesajlariGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("chats/user");
            if (Donus != null)
            {
                var MeID = DataBase.MEMBER_DATA_GETIR()[0].id;
                var aa = Donus.ToString();
                mFriends = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MesajKisileri>>(Donus.ToString());
                mFriends = mFriends.FindAll(item => item.request == true);
                if (mFriends.Count > 0)
                {
                    
                    mFriends.Where(item => item.receiverId == MeID).ToList().ForEach(item2 => item2.unreadMessageCount = 0);
                    SaveKeys();
                    SonMesajKiminKontrolunuYap();
                    InvokeOnMainThread(() =>
                    {
                        //Geçmiş gelecekten daha büyüktür
                        mFriends.Sort((x, y) => DateTime.Compare(x.lastModifiedDate, y.lastModifiedDate));
                        mFriends.Reverse();
                        Tablo.Source = new MesajlarCustomTableCellSoruce(mFriends, this, FavorileriCagir());
                        Tablo.ReloadData();
                        Tablo.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                        Tablo.TableFooterView = new UIView();
                        CustomLoading.Hide();
                        BoostUygula();
                    });
                }
                else
                {
                    CustomAlert.GetCustomAlert(GelenBase1, "Hiç Mesaj Bulunamadı...");
                    CustomLoading.Hide();
                }
            }
            else
            {
                CustomLoading.Hide();
            }
        }

        void BoostUygula()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                for (int i = 0; i < mFriends.Count; i++)
                {
                    WebService webService = new WebService();
                    var Donus = webService.OkuGetir("users/" + mFriends[i].receiverId.ToString());
                    if (Donus != null)
                    {
                        var aa = Donus.ToString();
                        var Icerikk = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(Donus.ToString());
                        try
                        {
                            if (Icerikk.boostTime >= DateTime.Now.AddMinutes(-30) && Icerikk.boostTime <= DateTime.Now)
                            {
                                mFriends[i].BoostOrSuperBoost = true;
                            }
                            else if (Icerikk.superBoostTime >= DateTime.Now.AddMinutes(-30) && Icerikk.superBoostTime <= DateTime.Now)
                            {
                                mFriends[i].BoostOrSuperBoost = true;
                            }
                            else
                            {
                                mFriends[i].BoostOrSuperBoost = false;
                            }
                        }
                        catch {}
                    }
                }

                var PaketeGoreSirala = (from item in mFriends
                             orderby item.BoostOrSuperBoost descending
                             select item).ToList();
                mFriends = PaketeGoreSirala;

                InvokeOnMainThread(() =>
                {
                    Tablo.Source = new MesajlarCustomTableCellSoruce(mFriends, this, FavorileriCagir());
                    Tablo.ReloadData();
                    Tablo.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                    Tablo.TableFooterView = new UIView();
                });

            })).Start();
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
                            UserID = mFriends[i].receiverId,
                            MessageKey = mFriends[i].key
                        });
                        //Hiç Yok Yeni Ekle
                    }
                }
            }
        }

        MEMBER_DATA MeData;
        void SonMesajKiminKontrolunuYap()
        {
            for (int i = 0; i < mFriends.Count; i++)
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("chats/user/" + mFriends[i].receiverId);
                if (Donus != null)
                {
                    var AA = Donus.ToString();
                    var NewChatList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChatDetayDTO>>(Donus.ToString());
                    if (NewChatList.Count > 0)//chatList
                    {

                        if (NewChatList[0].userId == MeData.id)
                        {
                            mFriends[i].unreadMessageCount = 0;
                        }
                    }
                }
            }
        }


        public void RowSelectt(MesajKisileri TiklananKisi)
        {
            GetUserInfo(TiklananKisi.receiverId.ToString(), TiklananKisi.key);
        }
        int Engelliler;
        void GetUserInfo(string UserID, string keyy)
        {

            WebService webService = new WebService();
            var Donus2 = webService.OkuGetir("blocked-user/block-list");
            if (Donus2 != null)
            {
                var EngelliKul = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EngelliKullanicilarDTO>>(Donus2.ToString());

                if (EngelliKul.Count > 0)
                {
                    InvokeOnMainThread(delegate ()
                    {
                        Engelliler = EngelliKul[0].blockUserId;

                    });
                }
            }
            var Donus = webService.OkuGetir("users/" + UserID);
            if (Donus != null)
            {
                var Userrr = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(Donus.ToString());
                MesajlarIcinSecilenKullanici.Kullanici = Userrr;
                MesajlarIcinSecilenKullanici.key = keyy;
                if (Engelliler == Userrr.id)
                {

                    CustomAlert.GetCustomAlert(GelenBase1, "Bu kullanıcıyı engellediğiniz için mesaj atamazsınız!");
                }
                else
                {
                    var LokasyonKisilerStory = UIStoryboard.FromName("MesajlarBaseVC", NSBundle.MainBundle);
                    ChatVC controller = LokasyonKisilerStory.InstantiateViewController("ChatVC") as ChatVC;
                    controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                    this.GelenBase1.PresentViewController(controller, true, null);
                }
            }
        }


        public class EngelliKullanicilarDTO
        {
            public int blockUserId { get; set; }
            public string createdDate { get; set; }
            public int id { get; set; }
            public string lastModifiedDate { get; set; }
            public string reasonType { get; set; }
            public string status { get; set; }
            public int userId { get; set; }
        }

        public class MesajlarCustomTableCellSoruce : UITableViewSource
        {
            List<MesajKisileri> TableItems;
            MesajlarIstekler MesajlarIstekler1;
            List<string> FollowListID;
            UITableView BuTablo;
            public MesajlarCustomTableCellSoruce(List<MesajKisileri> mekanlist, MesajlarIstekler MesajlarIstekler2, List<string> FollowListt)
            {
                TableItems = mekanlist;
                MesajlarIstekler1 = MesajlarIstekler2;
                FollowListID = FollowListt;
            }
            public void FavListGuncelle(string UserID, bool EkleKaldir, NSIndexPath CellIndexPathh)
            {
                if (EkleKaldir)
                {
                    FollowListID.Add(UserID.ToString());
                }
                else
                {
                    FollowListID.Remove(UserID.ToString());
                }

                BuTablo.BeginUpdates();
                BuTablo.ReloadRows(new NSIndexPath[] { CellIndexPathh }, UITableViewRowAnimation.Automatic);
                BuTablo.EndUpdates();
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
                BuTablo = tableView;
                var itemss = TableItems[indexPath.Row];
                var cell = (MesajlarCustomItemCell)tableView.DequeueReusableCell(MesajlarCustomItemCell.Key);
                if (cell == null)
                {
                    cell = MesajlarCustomItemCell.Create(FollowListID,this, indexPath);
                }
                cell.UpdateCell(itemss,false);
                return cell;
            }
            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                tableView.DeselectRow(indexPath, true);
                var item = TableItems[indexPath.Row];
                MesajlarIstekler1.RowSelectt(item);
            }
        }
    }
}
