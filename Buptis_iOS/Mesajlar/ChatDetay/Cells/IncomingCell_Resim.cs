using System;

using Foundation;
using UIKit;

namespace Buptis_iOS.ChatDetay.Cells
{
	[Register ("IncomingCell_Resim")]
	public class IncomingCell_Resim : BubbleCell
	{
		static readonly UIImage normalBubbleImage;
		static readonly UIImage highlightedBubbleImage;

		public static readonly NSString CellId = new NSString ("IncomingCell_Resim");

		static IncomingCell_Resim ()
		{
			UIImage mask = UIImage.FromBundle ("BubbleIncoming");

			var cap = new UIEdgeInsets {
				Top = 17f,
				Left = 26f,
				Bottom = 17f,
				Right = 21f,
			};

			var normalColor = UIColor.FromRGB (241, 242, 242);
			var highlightedColor = UIColor.FromRGB (241, 242, 242);

			normalBubbleImage = CreateColoredImage (normalColor, mask).CreateResizableImage (cap);
			highlightedBubbleImage = CreateColoredImage (highlightedColor, mask).CreateResizableImage (cap);
		}

		public IncomingCell_Resim (IntPtr handle)
			: base (handle)
		{
			Initialize ();
		}

		public IncomingCell_Resim ()
		{
			Initialize ();
		}

		[Export ("initWithStyle:reuseIdentifier:")]
		public IncomingCell_Resim (UITableViewCellStyle style, string reuseIdentifier)
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
            Hediye.Layer.BorderColor = UIColor.FromRGB(241, 242, 242).CGColor;
            Hediye.BackgroundColor = UIColor.FromRGB(241, 242, 242);
            MessageLabel.Hidden = true;
            ContentView.BackgroundColor = UIColor.Clear;
        }
	}
}
