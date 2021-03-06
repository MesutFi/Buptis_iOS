﻿using System;

using UIKit;
using Foundation;

namespace Buptis_iOS.ChatDetay.Cells
{
	[Register ("OutgoingCell_Resim")]
	public class OutgoingCell_Resim : BubbleCell
	{
		static readonly UIImage normalBubbleImage;
		static readonly UIImage highlightedBubbleImage;

		public static readonly NSString CellId = new NSString ("OutgoingCell_Resim");

		static OutgoingCell_Resim ()
		{
			UIImage mask = UIImage.FromBundle ("BubbleOutgoing");

			var cap = new UIEdgeInsets {
				Top = 17f,
				Left = 21f,
				Bottom = 17f,
				Right = 26f
			};

			var normalColor = UIColor.FromRGB (65, 65, 67);
			var highlightedColor = UIColor.FromRGB (65, 65, 67);

			normalBubbleImage = CreateColoredImage (normalColor, mask).CreateResizableImage (cap);
			highlightedBubbleImage = CreateColoredImage (highlightedColor, mask).CreateResizableImage (cap);
		}

		public OutgoingCell_Resim (IntPtr handle)
			: base (handle)
		{
			Initialize ();
		}

		public OutgoingCell_Resim ()
		{
			Initialize ();
		}

		[Export ("initWithStyle:reuseIdentifier:")]
		public OutgoingCell_Resim (UITableViewCellStyle style, string reuseIdentifier)
			: base (style, reuseIdentifier)
		{
			Initialize ();
		}

		void Initialize ()
		{
			BubbleHighlightedImage = highlightedBubbleImage;
			BubbleImage = normalBubbleImage;


            Hediye.Layer.CornerRadius = 25;
            Hediye.Layer.BorderWidth = 10f;
            Hediye.ClipsToBounds = true;
            Hediye.Layer.BorderColor= UIColor.FromRGB(65, 65, 67).CGColor;
            Hediye.BackgroundColor=UIColor.FromRGB(65, 65, 67);


            MessageLabel.Hidden = true;


            this.ContentView.LayoutIfNeeded();
            var newfmm = this.Hediye.Frame;
            var ekranGenisligi = UIScreen.MainScreen.Bounds.Width;
            newfmm = new CoreGraphics.CGRect(ekranGenisligi - 215f, newfmm.Y, 200f, 200f);
            this.Hediye.Frame = newfmm;
            //ContentView.BackgroundColor = UIColor.Red;
            ContentView.BackgroundColor = UIColor.Clear;
        }
	}
}