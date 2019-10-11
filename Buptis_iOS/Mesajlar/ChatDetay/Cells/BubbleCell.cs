using System;

using UIKit;
using CoreGraphics;
using Foundation;
using static Buptis_iOS.ChatVC;
using FFImageLoading;
using FFImageLoading.Work;

namespace Buptis_iOS.ChatDetay.Cells
{
	public abstract class BubbleCell : UITableViewCell
	{
		public UIImageView BubbleImageView { get; private set; }
		public UILabel MessageLabel { get; private set; }
		public UIImage BubbleImage { get; set; }
		public UIImage BubbleHighlightedImage { get; set; }
        public UIImageView Hediye { get;  set; }

        ChatDetayDTO msg;

		public ChatDetayDTO Message {
			get {
				return msg;
			}
			set {
				msg = value;
				BubbleImageView.Image = BubbleImage;
				BubbleImageView.HighlightedImage = BubbleHighlightedImage;
				MessageLabel.Text = msg.text;
				MessageLabel.UserInteractionEnabled = true;
				BubbleImageView.UserInteractionEnabled = false;
                Hediye.UserInteractionEnabled = false;
                Hediye.ContentMode = UIViewContentMode.ScaleAspectFit;

                var Boll = msg.text.Split('#');
                if (Boll.Length > 1)//Resim
                    SetGiftImage(msg.text);
            }
		}

		public BubbleCell (IntPtr handle)
			: base (handle)
		{
			Initialize ();
		}

		public BubbleCell ()
		{
			Initialize ();
		}

		[Export ("initWithStyle:reuseIdentifier:")]
		public BubbleCell (UITableViewCellStyle style, string reuseIdentifier)
			: base (style, reuseIdentifier)
		{
			Initialize ();
		}

		void Initialize ()
		{
			BubbleImageView = new UIImageView {
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			MessageLabel = new UILabel {
				TranslatesAutoresizingMaskIntoConstraints = false,
				Lines = 0,
				PreferredMaxLayoutWidth = 220f
			};

            Hediye = new UIImageView()
            {
                //TranslatesAutoresizingMaskIntoConstraints = false,
            };

            Hediye.Frame = new CGRect(0, 0, 200, 200);
           // Hediye.Image = UIImage.FromBundle("Images/gow.png");

            ContentView.AddSubviews(BubbleImageView,MessageLabel, Hediye);
            ContentView.BackgroundColor = UIColor.Clear;
        }

        void SetGiftImage(string imgpath)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                var ParseLink = imgpath.Split('#')[1];
                InvokeOnMainThread(delegate ()
                {
                    ImageService.Instance.LoadUrl(/*CDN.CDN_Path +*/ ParseLink).LoadingPlaceholder("https://demo.intellifi.tech/demo/Buptis/Generic/auser.jpg", ImageSource.Url).Into(Hediye);
                });
            })).Start();
        }

        public override void SetSelected (bool selected, bool animated)
		{
			base.SetSelected (selected, animated);
			BubbleImageView.Highlighted = selected;
		}

		protected static UIImage CreateColoredImage (UIColor color, UIImage mask)
		{
			var rect = new CGRect (CGPoint.Empty, mask.Size);
			UIGraphics.BeginImageContextWithOptions (mask.Size, false, mask.CurrentScale);
			CGContext context = UIGraphics.GetCurrentContext ();
			mask.DrawAsPatternInRect (rect);
			context.SetFillColor (color.CGColor);
			context.SetBlendMode (CGBlendMode.SourceAtop);
			context.FillRect (rect);
			UIImage result = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return result;
		}

		protected static UIImage CreateBubbleWithBorder (UIImage bubbleImg, UIColor bubbleColor)
		{
			bubbleImg = CreateColoredImage (bubbleColor, bubbleImg);
			CGSize size = bubbleImg.Size;

			UIGraphics.BeginImageContextWithOptions (size, false, 0);
			var rect = new CGRect (CGPoint.Empty, size);
			bubbleImg.Draw (rect);

			var result = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();

			return result;
		}
	}
}