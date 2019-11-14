using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Mesajlar.ChatDetay;
using Buptis_iOS.PrivateProfile;
using Buptis_iOS.Web_Service;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace Buptis_iOS
{
    public partial class HediyeGonderVC : UIViewController
    {
        #region Tanýmlamalar
        List<HediyelerDataModel> GaleriDataModel1 = new List<HediyelerDataModel>();
        public ChatVC ChatVC1;
        HediyelerCustomListItem[] Noktalar = new HediyelerCustomListItem[0];

        CoreGraphics.CGPoint viewCenter;
        #endregion

        public HediyeGonderVC (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            GeriButton.TouchUpInside += GeriButton_TouchUpInside;
        }

        private void GeriButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissModalViewController(true);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            viewCenter = ViewHazne.Center;
           
            //ViewAnimation();
            FillDataModel();
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            GalleryViewUI();
        }

        #region UI Desing
        public void GalleryViewUI()
        {
            //this.ContentView.BackgroundColor = UIColor.Clear;
            ViewHazne.BackgroundColor = UIColor.White;
            ViewHazne.Layer.CornerRadius = 30f;
            ViewHazne.ClipsToBounds = true;
        }

        #endregion

        public void ViewAnimation()
        {
            ViewHazne.Hidden = false;
            SlideVerticaly(ViewHazne, true, false);

            new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
            {
                await Task.Run(async delegate () {

                    await Task.Delay(1000);
                });
                InvokeOnMainThread(delegate
                {

                    UIView.Animate(0.8, 0, UIViewAnimationOptions.CurveEaseInOut,
                        () =>
                        {
                            ViewHazne.Center = viewCenter;
                            //new CGPoint(UIScreen.MainScreen.Bounds.Right - Listeislemleributon.Frame.Width / 2, Listeislemleributon.Center.Y);
                        },
                        () =>
                        {
                            ViewHazne.Center = viewCenter;
                        }
                    );
                });
            })).Start();
        }

        public static void SlideVerticaly(UIView view, bool isIn, bool fromTop, double duration = 0.3, Action onFinished = null)
        {
            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;
            var minTransform = CGAffineTransform.MakeTranslation(0, (fromTop ? -1 : 1) * view.Bounds.Height);
            var maxTransform = CGAffineTransform.MakeIdentity();

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
                () => {
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                    view.Transform = isIn ? maxTransform : minTransform;
                },
                onFinished
            );
        }

        void FillDataModel()
        {
            CustomLoading.Show(this, "Yükleniyor...");
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                KategoriyeGoreHediyeleriGetir();
                CustomLoading.Hide();
            })).Start();
        }

        void KategoriyeGoreHediyeleriGetir()
        {
            var MeID = DataBase.MEMBER_DATA_GETIR()[0].id;
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("locations/user/" + MeID);
            if (Donus != null)
            {
                var LokasyonCatids = Newtonsoft.Json.JsonConvert.DeserializeObject<EnSonLokasyonCategoriler>(Donus.ToString());
                if (LokasyonCatids.catIds.Count > 0)
                {
                    for (int i = 0; i < LokasyonCatids.catIds.Count; i++)
                    {
                        ResimleriGetir(LokasyonCatids.catIds[i].ToString());
                    }
                    if (GaleriDataModel1.Count > 0)
                    {
                        InvokeOnMainThread(() => {

                            CreateScrollView();
                        });
                        CustomLoading.Hide();
                    }
                    else
                    {
                        InvokeOnMainThread(() => {
                            CustomAlert.GetCustomAlert(this, "Hediye bulunamadı...");
                            this.DismissModalViewController(true);
                        });
                    }
                }
                else
                {
                    InvokeOnMainThread(() => {
                        CustomAlert.GetCustomAlert(this, "Hediye bulunamadı...");
                        this.DismissModalViewController(true);
                    });
                }
            }
            else
            {
                InvokeOnMainThread(() => {
                    CustomAlert.GetCustomAlert(this, "Hediye bulunamadı...");
                    this.DismissModalViewController(true);
                });
            }
        }
        void ResimleriGetir(string CatId)
        {
            WebService webService = new WebService();

            var Donus = webService.OkuGetir("gifts/category/" + CatId.ToString());
            if (Donus != null)
            {
                var aa = Donus.ToString();
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HediyelerDataModel>>(Donus.ToString());
                if (Icerik.Count > 0)
                {
                    GaleriDataModel1.AddRange(Icerik);
                }
            }
        }

        void CreateScrollView()
        {
            Noktalar = new HediyelerCustomListItem[GaleriDataModel1.Count];
            for (int i = 0; i < GaleriDataModel1.Count; i++)
            {
                var NoktaItem = HediyelerCustomListItem.Create(this, GaleriDataModel1[i]);

                if (i == 0)
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, 120, 166f);
                }
                else
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(120 * i, 0, 120, 166f);
                }
                NoktaItem.UpdateCell();
                HediyelerScroll.AddSubview(NoktaItem);
                Noktalar[i] = NoktaItem;
            }

            HediyelerScroll.ContentSize = new CoreGraphics.CGSize(Noktalar[Noktalar.Length - 1].Frame.Right, 166f);
        }
        public void SecilenImage(HediyelerDataModel SecilenHediyeDTO)
        {
            ChatVC1.HediyeGonder(SecilenHediyeDTO.path);
            this.DismissModalViewController(true);
        }
        public class EnSonLokasyonCategoriler
        {
            public List<int> catIds { get; set; }
        }
        public class HediyelerDataModel
        {
            public int categoryId { get; set; }
            public int id { get; set; }
            public string path { get; set; }
        }
    }
}
