using System;
using Buptis_iOS.Web_Service;
using FFImageLoading;
using FFImageLoading.Work;
using Foundation;
using UIKit;

namespace Buptis_iOS.Mesajlar.ChatDetay
{
    public partial class GidenHediye : UITableViewCell
    {
        public static readonly NSString Key = new NSString("GidenHediye");
        public static readonly UINib Nib;
        string message;
        static GidenHediye()
        {
            Nib = UINib.FromName("GidenHediye", NSBundle.MainBundle);
        }

        protected GidenHediye(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public static GidenHediye Create(string message2)
        {
            var OlusanView = (GidenHediye)Nib.Instantiate(null, null)[0];
            OlusanView.message = message2;
            OlusanView.BackgroundColor = UIColor.Clear;
            OlusanView.HediyeImageView.BackgroundColor = UIColor.FromRGB(65, 65, 67);
            OlusanView.HediyeImageView.Layer.CornerRadius = 20f;
            OlusanView.HediyeImageView.ClipsToBounds = true;
            return OlusanView;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.ContentView.LayoutIfNeeded();
            var newfmm = this.ContentView.Frame;
            var genislik = 128+16+16; //resim genişliği + sağ margin + sol margin
            newfmm = new CoreGraphics.CGRect(newfmm.Width - genislik, newfmm.Y, genislik, 142f);
            this.ContentView.Frame = newfmm;
            SetGiftImage(message);
        }
        void SetGiftImage(string imgpath)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                var ParseLink = message.Split('#')[1];
                InvokeOnMainThread(delegate ()
                {
                    ImageService.Instance.LoadUrl(/*CDN.CDN_Path +*/ ParseLink).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Into(HediyeImageView);
                });

            })).Start();
        }
    }
}
