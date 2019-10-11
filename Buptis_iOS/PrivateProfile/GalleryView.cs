using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.PrivateProfile;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UIKit;

namespace Buptis_iOS
{
    public partial class GalleryView : UIViewController
    {
        CustomImageTableCell[] Noktalar = new CustomImageTableCell[0];
        public List<GalleryDataModel> GaleriDataModel1 = new List<GalleryDataModel>();
        public PrivateProfileVC GenelBase;
        public GalleryView (IntPtr handle) : base (handle)
        {
        }
        #region Life Cycle
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewHazne.Hidden = true;
            this.View.BackgroundColor = UIColor.Clear;

        }
        CoreGraphics.CGPoint viewCenter;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            viewCenter = ViewHazne.Center;

            ViewAnimation();
            SetBackGround();

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ImageBackButton.TouchUpInside += ImageBackButton_TouchUpInside;
            GalleryViewUI();
            GetPhotos();
            CreateScrollViews();
        }
        private void ImageBackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.View.BackgroundColor = UIColor.Clear;
            Task.Run(delegate () {
                InvokeOnMainThread(delegate ()
                {
                    this.DismissViewController(true, null);
                });
            });
        }
        void GetPhotos()
        {
            WebService webService = new WebService();
            var MeID = DataBase.MEMBER_DATA_GETIR()[0].id;
            var Donus = webService.OkuGetir("images/user/" + MeID.ToString());
            if (Donus != null)
            {
                var aa = Donus.ToString();
                GaleriDataModel1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GalleryDataModel>>(Donus.ToString());
                if (GaleriDataModel1.Count > 0)
                {
                    GaleriDataModel1.Reverse();
                    InvokeOnMainThread(() => {

                        if (GaleriDataModel1.Count >= 10)
                        {
                            List<GalleryDataModel> GaleriDataModel1_COPY = new List<GalleryDataModel>();
                            for (int i = 0; i < 10; i++)
                            {
                                GaleriDataModel1_COPY.Add(GaleriDataModel1[i]);
                            }
                            GaleriDataModel1 = GaleriDataModel1_COPY;
                            GaleriDataModel1.Insert(0, new GalleryDataModel()
                            {
                                isAddedCell = true
                            });
                        }
                        else
                        {
                            List<GalleryDataModel> GaleriDataModel1_COPY = new List<GalleryDataModel>();
                            for (int i = 0; i < GaleriDataModel1.Count; i++)
                            {
                                GaleriDataModel1_COPY.Add(GaleriDataModel1[i]);
                            }
                            GaleriDataModel1 = GaleriDataModel1_COPY;
                            GaleriDataModel1.Insert(0, new GalleryDataModel()
                            {
                                isAddedCell = true
                            });
                        }
                    });
                }
                else
                {
                    GaleriDataModel1.Add(new GalleryDataModel()
                    {
                        isAddedCell = true
                    });
                Atla:
                    try
                    {
                    }
                    catch
                    {
                        goto Atla;
                    }

                    CustomLoading.Hide();
                }
            }
            else
            {
                GaleriDataModel1.Add(new GalleryDataModel()
                {
                    isAddedCell = true
                });
            Atla:
                try
                {
                }
                catch
                {
                    goto Atla;
                }

                CustomLoading.Hide();
            }
        }
        public void SetPhoto(UIImage impath)
        {
            string _base64String;
            NSData imageData = impath.AsJPEG(compressionQuality: 0.1f);
            
            _base64String = imageData.GetBase64EncodedString(NSDataBase64EncodingOptions.None);
            
            var UserId = DataBase.MEMBER_DATA_GETIR()[0].id;
            GalleryDataModel fotografEkleDataModel = new GalleryDataModel()
            {
                imagePath = _base64String,
                userId = UserId,
                createdDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                lastModifiedDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssZ")
            };

            WebService webService = new WebService();
            string jsonString = JsonConvert.SerializeObject(fotografEkleDataModel);
            var Donus = webService.ServisIslem("images", jsonString);
            if (Donus != "Hata")
            {
                for (int i = 0; i < Noktalar.Length; i++)
                {
                    Noktalar[i].RemoveFromSuperview();
                }
                GetPhotos();
                CreateScrollViews();
                CustomLoading.Hide();
                GenelBase.GetUserImage(UserId);
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Fotoðraf Yüklenemedi!...");
                CustomLoading.Hide();
            }
           
        }
        public void ViewAnimation()
        {
            ViewHazne.Hidden = false;
            SlideVerticaly(ViewHazne, true, false);
           
            new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
            {
               await Task.Run(async delegate () {

                  await  Task.Delay(1000);
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
        void SetBackGround()
        {
            float sayac = 0;
            Task.Run(async delegate () {
                Atla:
                await Task.Delay(1);
                InvokeOnMainThread(delegate () {
                    sayac += 0.01f;
                    this.View.BackgroundColor = UIColor.FromRGB(0, 0, 245).ColorWithAlpha(sayac);

                });
                if (sayac <= 0.8f)
                {
                    goto Atla;
                }
            });
        }
        #endregion
        void CreateScrollViews()
        {

            Noktalar = new CustomImageTableCell[GaleriDataModel1.Count];
            for (int i = 0; i < GaleriDataModel1.Count; i++)
            {
                var NoktaItem = CustomImageTableCell.Create(this, GaleriDataModel1[i]);
                
                if (i == 0)
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(0, 0, 120, 166f);
                }
                else
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(120 * i, 0, 120, 166f);
                }
                NoktaItem.UpdateCell();
                ImageScrollView.AddSubview(NoktaItem);
                Noktalar[i] = NoktaItem;
            }

            ImageScrollView.ContentSize = new CoreGraphics.CGSize(Noktalar[Noktalar.Length - 1].Frame.Right, 166f);
        }
     
        public void FotografSildiktenSonraGuncelle()
        {
            for (int i = 0; i < Noktalar.Length; i++)
            {
                Noktalar[i].RemoveFromSuperview();
            }
            GetPhotos();
            CreateScrollViews();
            CustomLoading.Hide();
            var MeID = DataBase.MEMBER_DATA_GETIR()[0].id;
            GenelBase.GetUserImage(MeID);
        }


        #region UI Desing
        public void GalleryViewUI()
        {
            //this.ContentView.BackgroundColor = UIColor.Clear;
            ViewHazne.BackgroundColor = UIColor.White;
            ViewHazne.Layer.CornerRadius = 30f;
            ViewHazne.ClipsToBounds = true;
            ImageOkButton.BackgroundColor = UIColor.FromRGB(226, 0, 93);
            ImageOkButton.Layer.CornerRadius = ImageOkButton.Frame.Height / 2;
            ImageOkButton.ClipsToBounds = true;
        }
        #endregion
        public class GalleryDataModel
        {
            public string createdDate { get; set; }
            public string id { get; set; }
            public string imagePath { get; set; }
            public string lastModifiedDate { get; set; }
            public int userId { get; set; }
            //--------------------
            public bool isAddedCell { get; set; }
        }
    }
}