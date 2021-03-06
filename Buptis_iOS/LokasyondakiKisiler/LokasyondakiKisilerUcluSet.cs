using Buptis_iOS.Database;
using Buptis_iOS.LokasyondakiKisiler;
using Buptis_iOS.PublicProfile;
using Buptis_iOS.Web_Service;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using static Buptis_iOS.LokasyondakiKisiler.LokasyondakiKisilerBaseVC;

namespace Buptis_iOS
{
    public partial class LokasyondakiKisilerUcluSet : UIView
    {

        List<MEMBER_DATA> GelenUser = new List<MEMBER_DATA>();
        public LokasyondakiKisilerUcluSet (IntPtr handle) : base (handle)
        {
        }
        public static LokasyondakiKisilerUcluSet Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("LokasyondakiKisilerUcluSet", null, null);
            var v = Runtime.GetNSObject<LokasyondakiKisilerUcluSet>(arr.ValueAt(0));
            v.BackgroundColor = UIColor.Clear;
            v.Hazne1.Hidden = true;
            v.Hazne2.Hidden = true;
            v.Hazne3.Hidden = true;
            v.Hazne1.BackgroundColor = v.Hazne2.BackgroundColor = v.Hazne3.BackgroundColor = UIColor.Clear;
          
            return v;
        }
        List<MEMBER_DATA> gelenUser2;
        public void UpdateCell2(List<MEMBER_DATA> gelenUser)
        {
            gelenUser2 = gelenUser;
            switch (gelenUser.Count)
            {
                case 1:
                    Photo1.Tag = 0;
                    GetUserImage(Photo1, gelenUser[0].id.ToString());
                    AdSoyadText.Text = gelenUser2[0].firstName + " " + gelenUser2[0].lastName.Substring(0, 1).ToString() + ".";
                    Hazne1.Hidden = false;
                    Photo1.TouchUpInside += Photo_TouchUpInside;
                    break;
                case 2:
                    Photo1.Tag = 0;
                    GetUserImage(Photo1, gelenUser[0].id.ToString());
                    AdSoyadText.Text = gelenUser2[0].firstName + " " + gelenUser2[0].lastName.Substring(0, 1).ToString() + ".";
                    Hazne1.Hidden = false;
                    Photo1.TouchUpInside += Photo_TouchUpInside;
                    //****
                    Photo2.Tag = 1;
                    GetUserImage(Photo2, gelenUser[1].id.ToString());
                    AdSoyadText2.Text = gelenUser2[1].firstName + " " + gelenUser2[1].lastName.Substring(0, 1).ToString() + ".";
                    Hazne2.Hidden = false;
                    Photo2.TouchUpInside += Photo_TouchUpInside;
                    break;
                case 3:
                    Photo1.Tag = 0;
                    GetUserImage(Photo1, gelenUser[0].id.ToString());
                    AdSoyadText.Text = gelenUser2[0].firstName + " " + gelenUser2[0].lastName.Substring(0, 1).ToString() + ".";
                    Hazne1.Hidden = false;
                    Photo1.TouchUpInside += Photo_TouchUpInside;
                    //****
                    Photo2.Tag = 1;
                    GetUserImage(Photo2, gelenUser[1].id.ToString());
                    AdSoyadText2.Text = gelenUser2[1].firstName + " " + gelenUser2[1].lastName.Substring(0, 1).ToString() + ".";
                    Hazne2.Hidden = false;
                    Photo2.TouchUpInside += Photo_TouchUpInside;
                    //*****
                    Photo3.Tag = 2;
                    GetUserImage(Photo3, gelenUser[2].id.ToString());
                    AdSoyadText3.Text = gelenUser2[2].firstName + " " + gelenUser2[2].lastName.Substring(0, 1).ToString() + ".";
                    Hazne3.Hidden = false;
                    Photo3.TouchUpInside += Photo_TouchUpInside;
                    break;
                default:
                    break;
            }
        }

        private void Photo_TouchUpInside(object sender, EventArgs e)
        {
            Console.WriteLine("DURUMMM 2 =>> " + Hazne1.Frame.Height + "X" + Hazne1.Frame.Width);
            Console.WriteLine("DURUMMM 5 =>> " + Photo1.Frame.Height + "X" + Photo1.Frame.Width);
            var Tagg = (int)((UIButton)sender).Tag;
            var tiklananUser = gelenUser2[Tagg];

            var PublicProfileBaseVC1 = UIStoryboard.FromName("PublicProfileBaseVC", NSBundle.MainBundle);
            PublicProfileBaseVC controller = PublicProfileBaseVC1.InstantiateViewController("PublicProfileBaseVC") as PublicProfileBaseVC;
            SecilenKisi.SecilenKisiDTO = tiklananUser;
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            LokasyondakiKisilerBaseVC_Kopya.LokasyondakiKisilerBaseVC1.PresentViewController(controller, true, null);
        }
        void GetUserImage(UIButton UserImage, string USERID)
        {
           
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("images/user/" + USERID);
                if (Donus != null)
                {
                    InvokeOnMainThread(delegate ()
                    {
                        var Images = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UsaerImageDTO>>(Donus.ToString());
                        if (Images.Count > 0)
                        {

                            ImageService.Instance.LoadUrl(CDN.CDN_Path + Images[Images.Count - 1].imagePath).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Into(UserImage);

                        }
                    });
                }
            })).Start();
        }
        
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Duzenlee(Photo1);
            Duzenlee(Photo2);
            Duzenlee(Photo3);
            Console.WriteLine("DURUMMM 3 =>> " + Hazne1.Frame.Height + "X" + Hazne1.Frame.Width);
        }
        void Duzenlee(UIButton UserImage)
        {
            UserImage.LayoutIfNeeded();
            Hazne1.LayoutIfNeeded();
            AdSoyadText.LayoutIfNeeded();
            UserImage.ClipsToBounds = true;
            var newFrame = UserImage.Frame;
            newFrame.Width = Hazne1.Frame.Height - AdSoyadText.Frame.Height-4;
            newFrame.Height = newFrame.Width;
            newFrame.X = 2;
            newFrame.Y = 2;
            UserImage.ContentMode = UIViewContentMode.ScaleAspectFill;
            UserImage.ImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            //var EksiXPosition = (newFrame.Height - newFrame.Width) / 2;
            //newFrame.Height = newFrame.Width;
            //newFrame.X -= EksiXPosition;
            UserImage.Frame = newFrame;
            UserImage.Layer.CornerRadius = UserImage.Frame.Height / 2;
            UserImage.Layer.BorderWidth = 4f;
            UserImage.Layer.BorderColor = UIColor.White.CGColor;
            Console.WriteLine("DURUMMM 1 =>> " + UserImage.Frame.Height + "X" + UserImage.Frame.Width);
        }
        public override void DidChange(NSString forKey, NSKeyValueSetMutationKind mutationKind, NSSet objects)
        {
            base.DidChange(forKey, mutationKind, objects);
            Console.WriteLine("DURUMMM 4 =>> " + Hazne1.Frame.Height + "X" + Hazne1.Frame.Width);
        }
        public class UsaerImageDTO
        {
            public string createdDate { get; set; }
            public int id { get; set; }
            public string imagePath { get; set; }
            public string lastModifiedDate { get; set; }
            public int userId { get; set; }
        }
    }
}