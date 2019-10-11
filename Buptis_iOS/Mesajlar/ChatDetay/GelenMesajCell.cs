using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Buptis_iOS.Mesajlar.ChatDetay
{
    public partial class GelenMesajCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("GelenMesajCell");
        public static readonly UINib Nib;
        string message;
        static GelenMesajCell()
        {
            Nib = UINib.FromName("GelenMesajCell", NSBundle.MainBundle);
        }
        protected GelenMesajCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public static GelenMesajCell Create(string message2)
        {
            var OlusanView = (GelenMesajCell)Nib.Instantiate(null, null)[0];
            OlusanView.message = message2;
            OlusanView.BackgroundColor = UIColor.Clear;
            return OlusanView;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.ContentView.LayoutIfNeeded();
            var newfmm = this.ContentView.Frame;
            newfmm.Width = GenislikGetir();
            this.ContentView.Frame = newfmm;

            //Console.WriteLine("DURUMM :" + this.Frame.Height);
        }
        ChatCustomTextContentView MesajlarFavoriler1;
        public void BubleAyarla()
        {
            this.LayoutIfNeeded();
            MesajTextVieww.BackgroundColor = UIColor.FromRGB(241, 242, 242);
            MesajTextVieww.Layer.CornerRadius = 20f;
            MesajTextVieww.ClipsToBounds=true;
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
