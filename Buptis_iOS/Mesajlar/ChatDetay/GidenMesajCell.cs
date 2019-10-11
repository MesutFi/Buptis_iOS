using System;

using Foundation;
using UIKit;

namespace Buptis_iOS.Mesajlar.ChatDetay
{
    public partial class GidenMesajCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("GidenMesajCell");
        public static readonly UINib Nib;
        string message;

        static GidenMesajCell()
        {
            Nib = UINib.FromName("GidenMesajCell", NSBundle.MainBundle);
        }

        protected GidenMesajCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public static GidenMesajCell Create(string message2)
        {
            var OlusanView = (GidenMesajCell)Nib.Instantiate(null, null)[0];
            OlusanView.message = message2;
            OlusanView.BackgroundColor = UIColor.Clear;
            return OlusanView;
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.ContentView.LayoutIfNeeded();
            MesajTextVieww.LayoutIfNeeded();

            //this.ContentIns

            var newfmm = this.ContentView.Frame;
            var genislik = GenislikGetir();
            newfmm = new CoreGraphics.CGRect(newfmm.Width - genislik, newfmm.Y, genislik, newfmm.Height);
            this.ContentView.Frame = newfmm;

            //Console.WriteLine("DURUMM :" + this.Frame.Height);
        }
        ChatCustomTextContentView MesajlarFavoriler1;
        public void BubleAyarla()
        {
            this.LayoutIfNeeded();
            MesajTextVieww.BackgroundColor = UIColor.FromRGB(65, 65, 67);
            MesajTextVieww.Layer.CornerRadius = 20f;
            MesajTextVieww.ClipsToBounds = true;
            MesajTextVieww.Text = message;
            MesajTextVieww.TextContainerInset = new UIEdgeInsets(16, 8, 8, 8);
            MesajTextVieww.LayoutIfNeeded();
            var size = MesajTextVieww.SizeThatFits(MesajTextVieww.Frame.Size);
            var frm = MesajTextVieww.Frame;
            frm.Width = size.Width;
            frm.Height = size.Height;
            MesajTextVieww.Frame = frm;
        }
        nfloat GenislikGetir()
        {
            var MesajlarFavoriler3 = ChatCustomTextContentView.Create(message);
            MesajlarFavoriler3.LayoutIfNeeded();
            var boyutgetir = MesajlarFavoriler3.GetRowSize();
            if (boyutgetir.Width >= UIScreen.MainScreen.Bounds.Width)
            {
                return UIScreen.MainScreen.Bounds.Width ;
            }
            else
            {
                return (boyutgetir.Width /*+ 32*/);
            }
            
        }
    }
}
