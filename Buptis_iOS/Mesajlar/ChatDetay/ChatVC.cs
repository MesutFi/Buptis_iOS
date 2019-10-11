using Buptis_iOS.ChatDetay.Cells;
using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Mesajlar.ChatDetay;
using Buptis_iOS.PublicProfile;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UIKit;
using static Buptis_iOS.LokasyondakiKisiler.LokasyondakiKisilerBaseVC;
using static Buptis_iOS.Mesajlar.MesajlarBaseVC;

namespace Buptis_iOS
{
    public partial class ChatVC : UIViewController,IUITextFieldDelegate
    {
        #region Tanimlamalar
        nfloat normalKisit, MesajBGViewwFrameBottom, AralikDurumu;
        List<ChatDetayDTO> ChatDetayDTO1 = new List<ChatDetayDTO>();
        MEMBER_DATA MeDTO;
        List<string> FollowListID = new List<string>();
        List<HazirMesaklarDTO> HazirMesaklarDTO1 = new List<HazirMesaklarDTO>();
        UIButton[] Noktalar = new UIButton[0];
        #endregion

        #region Genel Ýþler
        public ChatVC (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWillShow);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyboardDidShow);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardWillHide);
            //ChatTableView.AddGestureRecognizer(new UIGestureRecognizer(() =>
            //{
            //    this.MesajText.ResignFirstResponder();
            //}));

            MesajText.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            normalKisit = this.ButtomKisitlamaa.Constant;
            
            GetUserInfo();
            BackButton.TouchUpInside += BackButton_TouchUpInside;
            FavButton.TouchUpInside += FavButton_TouchUpInside;
            GonderButton.TouchUpInside += GonderButton_TouchUpInside;
            HediyeButton.TouchUpInside += HediyeButton_TouchUpInside;
        }

        private void HediyeButton_TouchUpInside(object sender, EventArgs e)
        {
            var FotografEkle1 = UIStoryboard.FromName("MesajlarBaseVC", NSBundle.MainBundle);
            HediyeGonderVC controller = FotografEkle1.InstantiateViewController("HediyeGonderVC") as HediyeGonderVC;
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
            controller.ChatVC1 = this;
            this.PresentViewController(controller, true, null);
        }

        private void GonderButton_TouchUpInside(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MesajText.Text))
            {
                MesajGonderGenericMetod(MesajText.Text);
            }
        }

        private void FavButton_TouchUpInside(object sender, EventArgs e)
        {
            FavoriIslemleri();
        }
        bool Actinmi1 = false;
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (!Actinmi1)
            {
                HeaderView.Hidden = true;
                BackButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
                FavButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
                HediyeButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
                GonderButton.ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
                ChatTableView.BackgroundColor = UIColor.Clear;
                ChatTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                ChatTableView.TableFooterView = new UIView();
                ChatTableView.Source = null;
                ChatTableView.ReloadData();
                MeDTO = DataBase.MEMBER_DATA_GETIR()[0];
                FavorileriCagir();
                IconlariAyarla(HediyeButton);
                IconlariAyarla(GonderButton);
                Actinmi1 = true;
            }
        }
        void IconlariAyarla(UIButton Buttonn)
        {
            var IconImage = Buttonn.ImageView.Image;
            var TintImage = IconImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            Buttonn.SetImage(TintImage,UIControlState.Normal);
            Buttonn.ImageView.TintColor = UIColor.FromRGB(238,0,63);
        }
        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }
        bool IlkAcilis = false;
        nfloat keyboardHeight = 0;
        nfloat iphpnexIlkKisit = 0;
        private void KeyboardWillShow(NSNotification notification)
        {
            #region Keyboardd
            //if (keyboardHeight <= 0)
            //{
            //    keyboardHeight = ((NSValue)notification.UserInfo.ValueForKey(UIKeyboard.FrameBeginUserInfoKey)).RectangleFValue.Height;
            //}
            //var maxYuseklik = UIScreen.MainScreen.Bounds.Height;
            keyboardHeight = ((NSValue)notification.UserInfo.ValueForKey(UIKeyboard.FrameEndUserInfoKey)).RectangleFValue.Height;
            UIView.Animate(0.1, () => {
                if (!IlkAcilis)
                {
                    //var AralikDurumu = this.View.Frame.Height - MesajBGViewwFrameBottom;
                    if (AralikDurumu > 1)//For Iphone X ++++
                    {
                        iphpnexIlkKisit = keyboardHeight - (this.View.Frame.Height - MesajBGViewwFrameBottom);
                        this.ButtomKisitlamaa.Constant = iphpnexIlkKisit;
                        this.View.LayoutIfNeeded();
                    }
                    else
                    {
                        this.ButtomKisitlamaa.Constant = keyboardHeight;
                        this.View.LayoutIfNeeded();
                    }
                    IlkAcilis = true;
                }
                else
                {

                    if (AralikDurumu > 1)//For Iphone X ++++
                    {
                        this.ButtomKisitlamaa.Constant = iphpnexIlkKisit;
                        this.View.LayoutIfNeeded();
                    }
                    else
                    {
                        this.ButtomKisitlamaa.Constant = keyboardHeight /*+ (normalKisit * 1) + MesajBGVieww.Frame.Height*/;
                        this.View.LayoutIfNeeded();
                    }

                }

                //var framee = this.MesajYazBGView.Frame;
                //MesajYazBGView.Frame = new CoreGraphics.CGRect(framee.X, framee.Y - keyboardHeight, framee.Width, framee.Height);
            });
            #endregion
        }
        private void KeyboardDidShow(NSNotification notification)
        {
            if (ChatDetayDTO1.Count > 0)
            {
                var bottomIndexPath = NSIndexPath.FromRowSection(ChatTableView.NumberOfRowsInSection(0) - 1, 0);
                try
                {
                    ChatTableView.ScrollToRow(bottomIndexPath, UITableViewScrollPosition.Bottom, true);
                }
                catch 
                {
                }
                
            }

           

        }
        private void KeyboardWillHide(NSNotification notification)
        {
            UIView.Animate(0.1, () => {
                this.ButtomKisitlamaa.Constant = normalKisit;
                this.View.LayoutIfNeeded();
            });
        }
        bool Actinmi2 = false;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!Actinmi2)
            {
                TasarimiDuzenle();
                MesajAtinAltiniDoldur();
                MesajBGViewwFrameBottom = MesajBGVieww.Frame.Bottom;
                nfloat AralikDurumu = this.View.Frame.Height - MesajBGVieww.Frame.Bottom;
                ChatTableView.BackgroundColor = UIColor.Clear;
                ChatTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                ChatTableView.TableFooterView = new UIView();
                MessageListenerr();
                KategoriyeGoreHazirMesajlariGetir();
                Actinmi2 = true;
            }
        }
        void MesajAtinAltiniDoldur()
        {
            AralikDurumu = this.View.Frame.Height - MesajBGVieww.Frame.Bottom;
            if (AralikDurumu > 1)
            {
                var BosView = new UIView();
                BosView.Frame = new CGRect(0, MesajBGVieww.Frame.Bottom, this.View.Frame.Width, this.View.Frame.Height - MesajBGVieww.Frame.Height);
                BosView.BackgroundColor = UIColor.White;
                this.View.AddSubview(BosView);
            }
        }
        #endregion

        #region  UI Tasarimlar
        void TasarimiDuzenle()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = HeaderView.Frame;
            HeaderView.Layer.InsertSublayer(gradientLayer, 0);
            HeaderView.Layer.CornerRadius = 30;
            HeaderView.ClipsToBounds = true;
            HeaderView.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);


            //MesajBGVieww.Layer.CornerRadius = 20.5f;
            //MesajBGVieww.ClipsToBounds = true;
            //MesajBGVieww.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);

            //left side
            var path = UIBezierPath.FromRoundedRect(MesajBGVieww.Bounds, UIRectCorner.TopLeft | UIRectCorner.TopRight, new CGSize(20.5f, 20.5f));
            var mask = new CAShapeLayer();
            mask.Path = path.CGPath;
            MesajBGVieww.Layer.Mask = mask;
            //right side
            MesajBGVieww.ClipsToBounds = true;
            //MesajBGVieww.Layer.CornerRadius = 100;
            //MesajBGVieww.Layer.MaskedCorners = CACornerMask.MaxXMaxYCorner | CACornerMask.MaxXMinYCorner;
            HeaderView.Hidden = false;

        }

        #endregion

        #region Mesaj Gonderme
        void MesajGonderGenericMetod(string Message)
        {
            ChatDetayDTO chatRecyclerViewDataModel = new ChatDetayDTO()
            {
                userId = MeDTO.id,
                receiverId = MesajlarIcinSecilenKullanici.Kullanici.id,
                text = Message,
                key = MesajlarIcinSecilenKullanici.key
            };
            WebService webService = new WebService();
            string jsonString = JsonConvert.SerializeObject(chatRecyclerViewDataModel);
            var Donus = webService.ServisIslem("chats", jsonString);
            if (Donus != "Hata")
            {
                var Icerikk = Newtonsoft.Json.JsonConvert.DeserializeObject<KeyIslemleriIcinDTO>(Donus.ToString());
                MesajText.Text = "";
                SaveKeys(Icerikk);
            }
            else
            {
                CustomAlert.GetCustomAlert(this,"Mesaj Gönderilemedi!");
                return;
            }
        }
        
        public void HediyeGonder(string GelenPath)
        {
            MesajGonderGenericMetod("sendGift#" + CDN.CDN_Path + GelenPath);
        }

        #endregion

        #region KeySorgulaKaydet
        void SaveKeys(KeyIslemleriIcinDTO GelenKeyIcerigi)
        {
            var LocalKeys = DataBase.CHAT_KEYS_GETIR();
            if (LocalKeys.Count > 0)
            {
                var KeyVarmi = LocalKeys.FindAll(item => item.MessageKey == GelenKeyIcerigi.key & item.UserID == MesajlarIcinSecilenKullanici.Kullanici.id);
                if (KeyVarmi.Count > 0)
                {
                    MesajlarIcinSecilenKullanici.key = GelenKeyIcerigi.key;
                }
                else
                {
                    var KullaniciyaAitHerhangiBirKey = LocalKeys.FindAll(item => item.UserID == MesajlarIcinSecilenKullanici.Kullanici.id);
                    if (KullaniciyaAitHerhangiBirKey.Count > 0)
                    {
                        if (DataBase.CHAT_KEYS_Guncelle(new CHAT_KEYS() { MessageKey = GelenKeyIcerigi.key, UserID = MesajlarIcinSecilenKullanici.Kullanici.id }))
                        {
                            MesajlarIcinSecilenKullanici.key = GelenKeyIcerigi.key;
                        }
                    }
                    else
                    {
                        if (DataBase.CHAT_KEYS_EKLE(new CHAT_KEYS() { MessageKey = GelenKeyIcerigi.key, UserID = MesajlarIcinSecilenKullanici.Kullanici.id }))
                        {
                            MesajlarIcinSecilenKullanici.key = GelenKeyIcerigi.key;
                        }
                    }
                }
            }
            else
            {
                if (DataBase.CHAT_KEYS_EKLE(new CHAT_KEYS() { MessageKey = GelenKeyIcerigi.key, UserID = MesajlarIcinSecilenKullanici.Kullanici.id }))
                {
                    MesajlarIcinSecilenKullanici.key = GelenKeyIcerigi.key;
                }
            }
        }

        #endregion

        #region GetUserInfo
        void GetUserInfo()
        {
            GetUserImage(MesajlarIcinSecilenKullanici.Kullanici.id.ToString(), UserImageView);
            UserNameLabel.Text = MesajlarIcinSecilenKullanici.Kullanici.firstName + " " + MesajlarIcinSecilenKullanici.Kullanici.lastName.Substring(0, 1) + ".";
            UserImageView.TouchUpInside += UserImageView_TouchUpInside;
        }

        private void UserImageView_TouchUpInside(object sender, EventArgs e)
        {
            SecilenKisi.SecilenKisiDTO = MesajlarIcinSecilenKullanici.Kullanici;
            var PublicProfileBaseVC1 = UIStoryboard.FromName("PublicProfileBaseVC", NSBundle.MainBundle);
            PublicProfileBaseVC controller = PublicProfileBaseVC1.InstantiateViewController("PublicProfileBaseVC") as PublicProfileBaseVC;
            this.PresentViewController(controller, true, null);
        }

        void GetUserImage(string USERID, UIButton UserImage)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("images/user/" + USERID);
                if (Donus != null)
                {
                    BeginInvokeOnMainThread(delegate () {

                        var Images = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserImageDTO>>(Donus.ToString());
                        if (Images.Count > 0)
                        {
                            ImageService.Instance.LoadUrl(CDN.CDN_Path + Images[Images.Count - 1].imagePath).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Transform(new CircleTransformation(15, "#FFFFFF")).Into(UserImage);
                        }
                    });
                }
            })).Start();
        }
        #endregion

        #region Favori Islemleri
        void FavorileriCagir()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus4 = webService.OkuGetir("users/favList/" + MeDTO.id.ToString());
                if (Donus4 != null)
                {
                    var JSONStringg = Donus4.ToString().Replace("[", "").Replace("]", "").Replace(" ","");
                    if (!string.IsNullOrEmpty(JSONStringg))
                    {
                        var dizi = JSONStringg.Split(',');
                        FollowListID =new List<string>(dizi);
                    }
                    var IsFollow = FollowListID.FindAll(item => item == MesajlarIcinSecilenKullanici.Kullanici.id.ToString());
                    if (IsFollow.Count > 0)
                    {
                        InvokeOnMainThread(delegate ()
                        {
                            ButtonAktifPasifBgYap(true);
                        });

                    }
                    else
                    {
                        InvokeOnMainThread(delegate ()
                        {
                            ButtonAktifPasifBgYap(false);
                        });

                    }
                }
                else
                {
                    InvokeOnMainThread(delegate ()
                    {
                        FavButton.Hidden = true;
                    });

                }
            })).Start();

        }
        void FavoriIslemleri()
        {
            WebService webService = new WebService();
            FavoriDTO favoriDTO = new FavoriDTO()
            {
                userId = MeDTO.id,
                favUserId = MesajlarIcinSecilenKullanici.Kullanici.id
            };
            string jsonString = JsonConvert.SerializeObject(favoriDTO);
            var Donus = webService.ServisIslem("users/fav", jsonString);
            if (Donus != "Hata")
            {
                //CustomAlert.GetCustomAlert(this, "Favorilere Ekledi.");
                ButtonAktifPasifBgYap(true);
                FavorileriCagir();
                return;
            }
            else
            {
                CustomAlert.GetCustomAlert(this, "Bir Sorun Oluþtu.");
                ButtonAktifPasifBgYap(false);
                return;
            }
        }
        void ButtonAktifPasifBgYap(bool durum)
        {
            FavButton.BackgroundColor = UIColor.Clear;
            FavButton.Layer.BorderWidth = 0;
            FavButton.Layer.BorderColor = UIColor.Clear.CGColor;
            if (!durum)
            {
                FavButton.SetImage(UIImage.FromBundle("Images/fav_pasif.png"), UIControlState.Normal);
            }
            else
            {
                FavButton.SetImage(UIImage.FromBundle("Images/fav_aktif.png"), UIControlState.Normal);
            }
        }
        #endregion

        #region Hazir Mesajlar
        void KategoriyeGoreHazirMesajlariGetir()
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
                        HazirMesajlariCagir(LokasyonCatids.catIds[i].ToString());
                    }
                    if (HazirMesaklarDTO1.Count > 0)
                    {
                        EtiketleriYerlestir();
                    }
                    else
                    {
                        HazirMesajGizle();
                       // HazirMesalScrollBaseHazne.Visibility = ViewStates.Gone;
                    }
                }
                else
                {
                    HazirMesajGizle();
                   //HazirMesalScrollBaseHazne.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                HazirMesajGizle();
                //HazirMesalScrollBaseHazne.Visibility = ViewStates.Gone;
            }
        }
        void HazirMesajGizle()
        {
            HazirMesajlarScroll.Hidden = true;
            var ChatTableFrame = ChatTableView.Frame;
            ChatTableFrame.Height = ChatArkaHazne.Frame.Height;
            ChatTableView.Frame = ChatTableFrame;
        }
        void EtiketleriYerlestir()
        {
            Noktalar = new UIButton[HazirMesaklarDTO1.Count];
            for (int i = 0; i < HazirMesaklarDTO1.Count; i++)
            {
                var NoktaItem = new UIButton(UIButtonType.Custom);
                NoktaItem.SetTitleColor(UIColor.White, UIControlState.Normal);
                NoktaItem.BackgroundColor =UIColor.FromRGB(238, 0, 63);
                NoktaItem.SetTitle(HazirMesaklarDTO1[i].name, UIControlState.Normal);
                NoktaItem.Layer.CornerRadius = 20f;
                NoktaItem.ClipsToBounds = true;
                if (i == 0)
                {
                    NoktaItem.Frame = new CoreGraphics.CGRect(5f, 0, ButonGenislikHesapla(HazirMesaklarDTO1[i].name), 40f);
                }
                else
                {
                    var BirOncekiGenislik = Noktalar[i-1].Frame.Right;
                    var CurrentGenislik = ButonGenislikHesapla(HazirMesaklarDTO1[i].name);
                    NoktaItem.Frame = new CoreGraphics.CGRect(BirOncekiGenislik + 5f, 0, CurrentGenislik, 40f);
                }

                HazirMesajlarScroll.AddSubview(NoktaItem);
                NoktaItem.TouchUpInside += NoktaItem_TouchUpInside;
                Noktalar[i] = NoktaItem;
            }

            HazirMesajlarScroll.ContentSize = new CoreGraphics.CGSize(Noktalar[Noktalar.Length - 1].Frame.Right, 40f);
            HazirMesajlarScroll.ShowsVerticalScrollIndicator = false;
            HazirMesajlarScroll.ShowsHorizontalScrollIndicator = false;
        }

        private void NoktaItem_TouchUpInside(object sender, EventArgs e)
        {
            var HazirMesajText = ((UIButton)sender).Title(UIControlState.Normal);
            if (!string.IsNullOrEmpty(HazirMesajText))
            {
                MesajGonderGenericMetod(HazirMesajText);
            }
        }

        nfloat ButonGenislikHesapla(string Content)
        {
            var SanalLabel = new UILabel();
            SanalLabel.Text = Content;
            SanalLabel.LayoutIfNeeded();
            return  (SanalLabel.IntrinsicContentSize.Width + 20f);
        }

        void HazirMesajlariCagir(string CatID)
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("questions/category/" + CatID);
            if (Donus != null)
            {
                var HazirMesajCopy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HazirMesaklarDTO>>(Donus.ToString());
                HazirMesajCopy = HazirMesajCopy.FindAll(item => item.type == "CATEGORY_QUESTION");
                HazirMesaklarDTO1.AddRange(HazirMesajCopy);
            }
        }
        #endregion

        #region Hediyeler
        /*
         var FotografEkle1 = UIStoryboard.FromName("PrivateProfileBaseVC", NSBundle.MainBundle);
            GalleryView controller = FotografEkle1.InstantiateViewController("GalleryView") as GalleryView;
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
            controller.GenelBase = this;
            this.PresentViewController(controller, true, null); 
             
        */
        #endregion

        #region Message Listener
        bool MesajlariGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("chats/user/" + MesajlarIcinSecilenKullanici.Kullanici.id.ToString());
            if (Donus != null)
            {
                var AA = Donus.ToString(); ;
                var NewChatList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChatDetayDTO>>(Donus.ToString());
                if (NewChatList.Count > 0)//chatList
                {
                    NewChatList.Reverse();

                    if (NewChatList.Count != ChatDetayDTO1.Count)
                    {
                        ChatDetayDTO1 = NewChatList;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        System.Threading.Timer _timer;
        void MessageListenerr()
        {
            var Mee = DataBase.MEMBER_DATA_GETIR()[0];
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {

                _timer = new System.Threading.Timer((o) =>
                {
                    try
                    {
                        var Durum = MesajlariGetir();
                        InvokeOnMainThread(() =>
                        {
                            if (Durum) //Ýçerik  Deðiþmiþse Uygula
                            {


                                //ChatTableView.TranslatesAutoresizingMaskIntoConstraints = false;

                                ChatTableView.AllowsSelection = false;
                                ChatTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                                ChatTableView.RegisterClassForCellReuse(typeof(IncomingCell), IncomingCell.CellId);
                                ChatTableView.RegisterClassForCellReuse(typeof(OutgoingCell), OutgoingCell.CellId);
                                ChatTableView.RegisterClassForCellReuse(typeof(OutgoingCell_Resim), OutgoingCell_Resim.CellId);
                                ChatTableView.RegisterClassForCellReuse(typeof(IncomingCell_Resim), IncomingCell_Resim.CellId);
                                //View.AddSubview(tableView);
                                ChatTableView.Source = new ChatCustomTableCellSoruce(ChatDetayDTO1, this, Mee);
                                ChatTableView.ReloadData();
                                if (ChatDetayDTO1.Count > 0)
                                {
                                    var bottomIndexPath = NSIndexPath.FromRowSection(ChatTableView.NumberOfRowsInSection(0) - 1, 0);
                                    ChatTableView.ScrollToRow(bottomIndexPath, UITableViewScrollPosition.Bottom, true);
                                }

                                //ChatTableView.Source = new ChatCustomTableCellSoruce(ChatDetayDTO1, this, Mee);
                                ChatTableView.BackgroundColor = UIColor.Clear;
                                //ChatTableView.RowHeight = UITableView.AutomaticDimension;
                                //ChatTableView.EstimatedRowHeight = 100f;
                                //ChatTableView.ReloadData();
                                //ChatTableView.AllowsSelection = false;
                                //if (ChatDetayDTO1.Count > 0)
                                //{
                                //    var bottomIndexPath = NSIndexPath.FromRowSection(ChatTableView.NumberOfRowsInSection(0) - 1, 0);
                                //    ChatTableView.ScrollToRow(bottomIndexPath, UITableViewScrollPosition.Bottom, true);
                                //}
                                MesajOkunduYap();
                            }
                        });
                    }
                    catch
                    {
                    }

                }, null, 0, 3000);
            })).Start();
        }
        void MesajOkunduYap()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                //Bana gelen ve okumadýklarým
                var BanaGelenler = ChatDetayDTO1.FindAll(item => item.read == false && item.receiverId == MeDTO.id);
                for (int i = 0; i < BanaGelenler.Count; i++)
                {
                    WebService webService = new WebService();
                    BanaGelenler[i].read = true;
                    string jsonString = JsonConvert.SerializeObject(BanaGelenler[i]);
                    webService.ServisIslem("chats", jsonString, Method: "PUT");
                }
            })).Start();
        }
        #endregion

        #region Chat Table Source
        public class ChatCustomTableCellSoruce : UITableViewSource
        {
            static readonly NSString IncomingCellId = new NSString("Incoming");
            static readonly NSString OutgoingCellId = new NSString("Outgoing");
            static readonly NSString Outgoing_Resim_CellId = new NSString("OutgoingCell_Resim");
            static readonly NSString Incoming_Resim_CellId = new NSString("IncomingCell_Resim");

            IList<ChatDetayDTO> messages;
            ChatVC ChatVC1;
            MEMBER_DATA ME;

            readonly BubbleCell[] sizingCells;

            public ChatCustomTableCellSoruce(IList<ChatDetayDTO> messages, ChatVC ChatVC2, MEMBER_DATA ME2)
            {
                if (messages == null)
                    throw new ArgumentNullException(nameof(messages));

                this.messages = messages;
                sizingCells = new BubbleCell[2];
                ChatVC1 = ChatVC2;
                ME = ME2;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return messages.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                BubbleCell cell = null;
                ChatDetayDTO msg = messages[indexPath.Row];

                cell = (BubbleCell)tableView.DequeueReusableCell(GetReuseId(msg));
                cell.Message = msg;
                cell.BackgroundColor = UIColor.Clear;
                return cell;
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                ChatDetayDTO msg = messages[indexPath.Row];
                return CalculateHeightFor(msg, tableView);
            }

            public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
            {
                ChatDetayDTO msg = messages[indexPath.Row];
                return CalculateHeightFor(msg, tableView);
            }

            nfloat CalculateHeightFor(ChatDetayDTO msg, UITableView tableView)
            {
                var Boll = msg.text.Split('#');
                if (Boll.Length <= 1)//Resim
                {
                    int index = -1;
                    if (ME.id == msg.receiverId) //GelenMesaj
                    {
                        index = 0;
                    }
                    else
                    {
                        index = 1;
                    }
                    BubbleCell cell = sizingCells[index];
                    if (cell == null)
                        cell = sizingCells[index] = (BubbleCell)tableView.DequeueReusableCell(GetReuseId(msg));

                    cell.Message = msg;

                    cell.SetNeedsLayout();
                    cell.LayoutIfNeeded();
                    CGSize size = cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize);

                    return NMath.Ceiling(size.Height) + 1;
                }
                else
                {
                    return NMath.Ceiling(200f) + 1;
                }

            }

            NSString GetReuseId(ChatDetayDTO GelenDTO)
            {
                if (ME.id == GelenDTO.receiverId)//Incoming
                {
                    var Boll = GelenDTO.text.Split('#');
                    if (Boll.Length > 1)//Resim
                    {
                        return Incoming_Resim_CellId;
                    }
                    else
                    {
                        return IncomingCellId;
                    }
                }
                else //OutGoing
                {
                    var Boll = GelenDTO.text.Split('#');
                    if (Boll.Length > 1)//Resim
                    {
                        return Outgoing_Resim_CellId;
                    }
                    else
                    {
                        return OutgoingCellId;
                    }


                }
            }
        }
        #endregion

        #region DTOS
        public class ChatDetayDTO
        {
            public string createdDate { get; set; }
            public string id { get; set; }
            public string key { get; set; }
            public string lastModifiedDate { get; set; }
            public bool read { get; set; }
            public int receiverId { get; set; }
            public string text { get; set; }
            public int userId { get; set; }

            //CUSTOM 
            public nfloat BelirlenenBoyut { get; set; }
        }
        public class UserImageDTO
        {
            public string createdDate { get; set; }
            public int id { get; set; }
            public string imagePath { get; set; }
            public string lastModifiedDate { get; set; }
            public int userId { get; set; }
        }
        public class FavoriDTO
        {
            public int favUserId { get; set; }
            public int userId { get; set; }
        }
        public class EnSonLokasyonCategoriler
        {
            public List<int> catIds { get; set; }
        }
        public class HazirMesaklarDTO
        {
            public int categoryId { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }
        public class KeyIslemleriIcinDTO
        {
            public string createdDate { get; set; }
            public int id { get; set; }
            public string key { get; set; }
            public string lastModifiedDate { get; set; }
            public bool read { get; set; }
            public int receiverId { get; set; }
            public string text { get; set; }
            public int userId { get; set; }
        }
        #endregion

    }
}