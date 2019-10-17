using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.LokasyondakiKisiler;
using Buptis_iOS.Lokasyonlar;
using Buptis_iOS.Web_Service;
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using static Buptis_iOS.EngelleVC;
using System.Linq;
using static Buptis_iOS.Lokasyonlar.LokasyonlarBaseVC;

namespace Buptis_iOS
{
    public partial class LokasyondakiKisilerBeklenenler : UIView
    {
        #region Tanimlamalar
        LokasyondakiKisilerUcluSet[] Noktalar = new LokasyondakiKisilerUcluSet[20];
        List<MEMBER_DATA> LokasyondakiKisilerList = new List<MEMBER_DATA>();
        LokasyondakiKisilerBaseVC gelenbase;
        public Mekanlar_Location GelenMekan;
        #endregion
        public LokasyondakiKisilerBeklenenler(IntPtr handle) : base(handle)
        {
        }

        public static LokasyondakiKisilerBeklenenler Create(Mekanlar_Location GelenMekan2, LokasyondakiKisilerBaseVC gelenbase2)
        {
            var arr = NSBundle.MainBundle.LoadNib("LokasyondakiKisilerBeklenenler", null, null);
            var v = Runtime.GetNSObject<LokasyondakiKisilerBeklenenler>(arr.ValueAt(0));
            v.BackgroundColor = UIColor.Clear;
            v.GelenMekan = GelenMekan2;
            v.gelenbase = gelenbase2;
            return v;
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            GetLocation();
        }
        void FilterUsers()
        {
            var GetUserFilter1 = DataBase.FILTRELER_GETIR();
            if (GetUserFilter1.Count > 0)
            {
                var GetUserFilter = GetUserFilter1[0];
                var minDT = DateTime.Now.AddYears((-1) * (GetUserFilter.minAge));//2015
                var maxDate = DateTime.Now.AddYears((-1)* GetUserFilter.maxAge);//1990
                if (GetUserFilter.Cinsiyet != 0)
                {
                    if (GetUserFilter.Cinsiyet == 1)
                    {
                        LokasyondakiKisilerList = LokasyondakiKisilerList.FindAll(item => item.gender == "Erkek" & item.birthDayDate <= minDT & item.birthDayDate >= maxDate);
                    }
                    else if (GetUserFilter.Cinsiyet == 2)
                    {
                        LokasyondakiKisilerList = LokasyondakiKisilerList.FindAll(item => item.gender == "Kadýn" & item.birthDayDate <= minDT & item.birthDayDate >= maxDate);
                    }
                    else
                    {
                        LokasyondakiKisilerList = LokasyondakiKisilerList.FindAll(item => item.gender == "Kadýn" | item.gender == "Erkek" & item.birthDayDate <= minDT & item.birthDayDate >= maxDate);
                    }
                }
            }
            FilterBlockedUser();
        }
        void FilterBlockedUser()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("blocked-user/block-list");
            if (Donus != null)
            {
                var EngelliKullanicilarDTOs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EngelliKullanicilarDTO>>(Donus.ToString());
                if (EngelliKullanicilarDTOs.Count > 0)
                {
                    List<MEMBER_DATA> Ayiklanmis = (from list1 in LokasyondakiKisilerList
                                                    join list2 in EngelliKullanicilarDTOs
                                                    on list1.id equals list2.blockUserId
                                                    select list1).ToList();

                    List<MEMBER_DATA> Karsilastir = LokasyondakiKisilerList.Except(Ayiklanmis).ToList();
                    LokasyondakiKisilerList = Karsilastir.ToList();
                }
            }
        }
        void GetLocation()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("users/location/" + GelenMekan.id + "/waiting");
            var MEDID = DataBase.MEMBER_DATA_GETIR()[0].id;
            if (Donus != null)
            {
                var aa = Donus.ToString();
                LokasyondakiKisilerList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MEMBER_DATA>>(Donus.ToString());
                LokasyondakiKisilerList = LokasyondakiKisilerList.FindAll(item => item.id != MEDID);
                if (LokasyondakiKisilerList.Count > 0)
                {
                 
                    InvokeOnMainThread(() => {
                        FilterUsers();
                        SuperBoostKullaniminaGoreSirala();
                        FillDataModel();
                    });
                }
                else
                {
                    CustomAlert.GetCustomAlert(gelenbase, "Henüz bu lokasyonda kimse yok...");
                    CustomLoading.Hide();
                }
            }
            else
            {
                CustomAlert.GetCustomAlert(gelenbase, "Henüz bu lokasyonda kimse yok...");
                CustomLoading.Hide();
            }
        }
        void SuperBoostKullaniminaGoreSirala()
        {
            //SUPER BOSTA GÖRE SIRANMASI GEREK ÞUAN BOOSTA GÖRE SIRALIYOR
            var PaketeGoreSirala = (from item in LokasyondakiKisilerList
                                    orderby item.superBoost descending
                                    select item).ToList();
            LokasyondakiKisilerList = PaketeGoreSirala;
        }

        void HucreleriDuzenle()
        {
            if (LokasyondakiKisilerList.Count % 3 == 0)
            {
                var NewList2 = new List<List<MEMBER_DATA>>();//3lü listelerin listesi
                var DonusSayisi = UcluRowSayisiGetir();
                for (int i2 = 0; i2 < LokasyondakiKisilerList.Count; i2 += 3)
                {
                    var NewListt = new List<MEMBER_DATA>();//3lü liste
                    NewListt.Add(LokasyondakiKisilerList[i2]);//0 - 3
                    NewListt.Add(LokasyondakiKisilerList[i2 + 1]);//1 - 4
                    NewListt.Add(LokasyondakiKisilerList[i2 + 2]);//2 - 5
                    NewList2.Add(NewListt);
                }
                var testt = NewList2;
                for (int i = 0; i < Noktalar.Length; i++)//2
                {
                    Noktalar[i].UpdateCell2(NewList2[i]);
                }
            }
            else
            {
                var NewList2 = new List<List<MEMBER_DATA>>();//3lü listelerin listesi
                for (int i2 = 0; i2 < LokasyondakiKisilerList.Count; i2 += 3)
                {
                    var NewListt = new List<MEMBER_DATA>();//3lü liste
                    NewListt.Add(LokasyondakiKisilerList[i2]);//0 - 3

                    if (LokasyondakiKisilerList.Count >= (i2 + 1 + 1))
                    {
                        NewListt.Add(LokasyondakiKisilerList[i2 + 1]);//1 - 4
                    }

                    if (LokasyondakiKisilerList.Count >= (i2 + 2 + 1))
                    {
                        NewListt.Add(LokasyondakiKisilerList[i2 + 2]);//1 - 4
                    }

                    NewList2.Add(NewListt);
                }
                var testt = NewList2;
                for (int i = 0; i < Noktalar.Length; i++)//2
                {
                    Noktalar[i].UpdateCell2(NewList2[i]);
                }
            }
        }

        void FillDataModel()
        {
            var yukseklik = (UIKit.UIScreen.MainScreen.Bounds.Width - 2) / 3;
            yukseklik += (yukseklik) / 2f;
            var RowCount = UcluRowSayisiGetir();
            Noktalar = new LokasyondakiKisilerUcluSet[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                var NoktaItem = LokasyondakiKisilerUcluSet.Create();

                if (i == 0)
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, yukseklik);
                }
                else
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(0, yukseklik * i, UIKit.UIScreen.MainScreen.Bounds.Width, yukseklik);
                }

                ScrollVieww.AddSubview(NoktaItem);
                Noktalar[i] = NoktaItem;
            }

            if (Noktalar.Length > 0)
            {
                ScrollVieww.ContentSize = new CoreGraphics.CGSize(UIKit.UIScreen.MainScreen.Bounds.Width, Noktalar[Noktalar.Length - 1].Frame.Bottom);
                BekletVeUygulaAsync();
            }
        }
        int UcluRowSayisiGetir()
        {
            if (LokasyondakiKisilerList.Count % 3 == 0)
            {
                return (int)(LokasyondakiKisilerList.Count / 3);
            }
            else
            {
                var NetRakam = Convert.ToDouble(LokasyondakiKisilerList.Count) / (double)3;
                var YukariYuvarla = Math.Ceiling(NetRakam);
                return (int)YukariYuvarla;
            }
        }
        async Task BekletVeUygulaAsync()
        {
            await Task.Run(async () =>
            {
                await Task.Delay(1000);
                InvokeOnMainThread(delegate ()
                {

                    HucreleriDuzenle();
                });
            });
        }

        public class LokasyondakiKisilerDatModel
        {
            public string ID { get; set; }
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

    }
}