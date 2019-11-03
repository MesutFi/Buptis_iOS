using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Web_Service;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using static Buptis_iOS.CoktanSecmeliSoruVC;

namespace Buptis_iOS
{
    public partial class ProfilSorulariBaseVC : UIViewController
    {
        public UIViewController[] Noktalar = new UIViewController[0];
        List<QuestionDTO> QuestionDTOs = new List<QuestionDTO>();
        List<OptionsDTO> OptionsDTOs = new List<OptionsDTO>();
        List<UserAnswersDTO> userAnswer = new List<UserAnswersDTO>();
        UIViewController NoktaItem;
        UIViewController SonucVc;

        bool Actinmi = false;
        public ProfilSorulariBaseVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CloseButton.TouchUpInside += CloseButton_TouchUpInside;
            IleriButton.TouchUpInside += IleriButton_TouchUpInside;
            GeriButton.TouchUpInside += GeriButton_TouchUpInside;
            AtlaButton.TouchUpInside += AtlaButton_TouchUpInside;
            ContentScroll.Scrolled += ContentScroll_Scrolled;
        }

        private void ContentScroll_Scrolled(object sender, EventArgs e)
        {
            var PageeIndex = (nint)(ContentScroll.ContentOffset.X / ContentScroll.Frame.Width);
            CountLabel.Text = (1 + PageeIndex).ToString() + "/" + Noktalar.Length;
        }

        private void AtlaButton_TouchUpInside(object sender, EventArgs e)
        {
            var PrivateProfileBaseVC1 = UIStoryboard.FromName("PrivateProfileBaseVC", NSBundle.MainBundle);
            SonucVC controller = PrivateProfileBaseVC1.InstantiateViewController("SonucVC") as SonucVC;
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            controller.ProfilSorulariBaseVC1 = this;
            this.PresentViewController(controller, true, null);
        }

        private void GeriButton_TouchUpInside(object sender, EventArgs e)
        {
            var PageeIndex = (nint)(ContentScroll.ContentOffset.X / ContentScroll.Frame.Width);
            ScrollToIndex((int)PageeIndex - 1);
        }

        private void IleriButton_TouchUpInside(object sender, EventArgs e)
        {
            var PageeIndex = (nint)(ContentScroll.ContentOffset.X / ContentScroll.Frame.Width);
            ScrollToIndex((int)PageeIndex + 1);
            if ((PageeIndex+1) == QuestionDTOs.Count)
            {
                var PrivateProfileBaseVC1 = UIStoryboard.FromName("PrivateProfileBaseVC", NSBundle.MainBundle);
                SonucVC controller = PrivateProfileBaseVC1.InstantiateViewController("SonucVC") as SonucVC;
                controller.ProfilSorulariBaseVC1 = this;
                this.PresentViewController(controller, true, null);
            }
            
        }
        void ScrollToIndex(int PageIndex)
        {
            CGRect Frameee = ContentScroll.Frame;
            Frameee.X = Frameee.Size.Width * PageIndex;
            Frameee.Y = 0;
            ContentScroll.ScrollRectToVisible(Frameee, true);
        }

        private void CloseButton_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!Actinmi)
            {
                var marginn = 10f;
                CloseButton.ContentEdgeInsets = new UIEdgeInsets(marginn, marginn, marginn, marginn);
                IleriButton.ContentEdgeInsets = new UIEdgeInsets(marginn, marginn, marginn, marginn);
                GeriButton.ContentEdgeInsets = new UIEdgeInsets(marginn, marginn, marginn, marginn);
                ViewBackground();
                ButtonlariDuzenle(CloseButton);
                ButtonlariDuzenle(IleriButton);
                ButtonlariDuzenle(GeriButton);
                BekletveSorulariOlustur();
                Actinmi = true;
            }
          
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            CountLabel.Text = "";
        }
        void BekletveSorulariOlustur()
        {
            Task.Run(delegate() {
                Task.Delay(500);
                InvokeOnMainThread(delegate () {
                    CreateScrollViews();
                });
            });
        }
        void CreateScrollViews()
        {
            GetUserAnswers();
            var PageeIndex = (nint)(ContentScroll.ContentOffset.X / ContentScroll.Frame.Width);
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("questions");
            if (Donus != null)
            {
                QuestionDTOs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionDTO>>(Donus.ToString());
                QuestionDTOs = QuestionDTOs.FindAll(item => item.type != "CATEGORY_QUESTION");
                CountLabel.Text = (PageeIndex + 1).ToString() + "/" + QuestionDTOs.Count.ToString();
            }
            if (QuestionDTOs.Count > 0)
            {
                Noktalar = new UIViewController[QuestionDTOs.Count];
               
                for (int i = 0; i < QuestionDTOs.Count; i++)
                {
                    
                    if (QuestionDTOs[i].type == "MULTIPLE_CHOICE")
                    {
                        NoktaItem = this.Storyboard.InstantiateViewController("CoktanSecmeliSoruVC") as CoktanSecmeliSoruVC;
                        (NoktaItem as CoktanSecmeliSoruVC).GelenSoru = QuestionDTOs[i];
                        (NoktaItem as CoktanSecmeliSoruVC).userAnswer = this.userAnswer;
                        NoktaItem.View.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, ContentScroll.Frame.Height);
                    }
                    else if (QuestionDTOs[i].type == "OPEN_TIP")
                    {
                        NoktaItem = this.Storyboard.InstantiateViewController("RangeSoruVC") as RangeSoruVC;
                        (NoktaItem as RangeSoruVC).GelenSoru = QuestionDTOs[i];
                        (NoktaItem as RangeSoruVC).userAnswer = this.userAnswer;
                        NoktaItem.View.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, ContentScroll.Frame.Height);
                    }
                    if(i==0)
                    {
                        NoktaItem.View.Frame = new CoreGraphics.CGRect(0, 0, UIKit.UIScreen.MainScreen.Bounds.Width, ContentScroll.Frame.Height);
                    }
                    else
                    {
                        NoktaItem.View.Frame = new CoreGraphics.CGRect(UIKit.UIScreen.MainScreen.Bounds.Width * i, 0, UIKit.UIScreen.MainScreen.Bounds.Width, ContentScroll.Frame.Height);
                    }
                    NoktaItem.WillMoveToParentViewController(this);
                    ContentScroll.AddSubview(NoktaItem.View);
                    this.AddChildViewController(NoktaItem);
                    NoktaItem.DidMoveToParentViewController(this);
                    Noktalar[i] = NoktaItem;
                }
                CountLabel.Text = "1/" + Noktalar.Length;
                var RightPointt = Noktalar[Noktalar.Length - 1].View.Frame.Right;
                ContentScroll.ContentSize = new CoreGraphics.CGSize(RightPointt, ContentScroll.Frame.Height);
                ContentScroll.PagingEnabled = true;
            }
            else
            {
                CustomAlert.GetCustomAlert(this,"Henüz profil sorularýn hazýr deðil.");
                this.DismissViewController(true,null);
            }
        }


        void GetUserAnswers()
        {
            WebService webService = new WebService();
            var UserLogin = DataBase.MEMBER_DATA_GETIR()[0];
            var Donus = webService.OkuGetir("answers/user/" + UserLogin.login);
            if (Donus != null)
            {
                userAnswer = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserAnswersDTO>>(Donus.ToString());
            }
        }

        #region Tasarim Duzenlemelemeleri
        void ButtonlariDuzenle(UIButton GelenButton)
        {
            GelenButton.Layer.CornerRadius = GelenButton.Frame.Height / 2;
            GelenButton.Layer.BorderColor = UIColor.White.CGColor;
            GelenButton.Layer.BorderWidth = 3f;
            GelenButton.ClipsToBounds = true;
        }

        void ViewBackground()
        {
            var Color1 = UIColor.FromRGB(15, 0, 241).CGColor;
            var Color2 = UIColor.FromRGB(2, 0, 100).CGColor;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new CoreGraphics.CGColor[] { Color1, Color2 };
            gradientLayer.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradientLayer.EndPoint = new CoreGraphics.CGPoint(1, 1);
            gradientLayer.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            this.View.Layer.InsertSublayer(gradientLayer, 0);
            this.View.ClipsToBounds = true;
          //  this.View.Layer.MaskedCorners = (CACornerMask.MaxXMaxYCorner) | (CACornerMask.MinXMaxYCorner);
        }
        #endregion
        public class QuestionDTO
        {
            public int categoryId { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }

        public class UserAnswersDTO
        {
            public string id { get; set; }
            public string option { get; set; }
            public int questionId { get; set; }
        }
    }

}