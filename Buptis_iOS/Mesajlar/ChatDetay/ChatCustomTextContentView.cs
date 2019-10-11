using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System;
using UIKit;

namespace Buptis_iOS
{
    public partial class ChatCustomTextContentView : UIView
    {
        public ChatCustomTextContentView (IntPtr handle) : base (handle)
        {
        }

        public static ChatCustomTextContentView Create(string messagee)
        {
            var arr = NSBundle.MainBundle.LoadNib("ChatCustomTextContentView", null, null);
            var v = Runtime.GetNSObject<ChatCustomTextContentView>(arr.ValueAt(0));
            v.MesajTextVieww.Text = messagee;
            v.BackgroundColor = UIColor.Clear;
            v.MesajTextVieww.TextContainerInset = new UIEdgeInsets(8,8,8,8);
            return v;
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            //this.ContentView.BackgroundColor = UIColor.Clear;
            MesajTextVieww.Layer.CornerRadius = 10f;
            MesajTextVieww.ClipsToBounds = true;
        }
        public CGSize GetRowSize()
        {
            this.LayoutIfNeeded();
            MesajTextVieww.LayoutIfNeeded();
            return MesajTextVieww.SizeThatFits(MesajTextVieww.Frame.Size);
        }
    }
}