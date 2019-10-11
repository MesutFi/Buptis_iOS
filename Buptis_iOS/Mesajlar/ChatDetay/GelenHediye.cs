using System;
using Buptis_iOS.Web_Service;
using FFImageLoading;
using FFImageLoading.Work;
using Foundation;
using UIKit;

namespace Buptis_iOS.Mesajlar.ChatDetay
{
    public partial class GelenHediye : UITableViewCell
    {
        public static readonly NSString Key = new NSString("GelenHediye");
        public static readonly UINib Nib;
        string message;
        static GelenHediye()
        {
            Nib = UINib.FromName("GelenHediye", NSBundle.MainBundle);
        }

        protected GelenHediye(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public static GelenHediye Create(string message2)
        {
            var OlusanView = (GelenHediye)Nib.Instantiate(null, null)[0];
            OlusanView.message = message2;
            OlusanView.BackgroundColor = UIColor.Clear;
            OlusanView.HediyeImageView.BackgroundColor = UIColor.FromRGB(241, 242, 242);
            OlusanView.HediyeImageView.Layer.CornerRadius = 20f;
            OlusanView.HediyeImageView.ClipsToBounds = true;
            return OlusanView;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.ContentView.LayoutIfNeeded();
            var newfmm = this.ContentView.Frame;
            var genislik = 128 + 16 + 16; //resim genişliği + sağ margin + sol margin
            newfmm = new CoreGraphics.CGRect(0, newfmm.Y, genislik, 142f);
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
