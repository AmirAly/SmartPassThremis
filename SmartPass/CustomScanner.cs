using System;

#if __UNIFIED__
using Foundation;
using CoreFoundation;
using CoreGraphics;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;

using System.Drawing;
using CGRect = global::System.Drawing.RectangleF;
using CGPoint = global::System.Drawing.PointF;
#endif

namespace SmartPass
{
	public class CustomOverlayView : UIView
	{
		public UIButton ButtonTorch;
		public UIButton ButtonCancel;

		public CustomOverlayView() : base()
		{
			ButtonCancel = UIButton.FromType(UIButtonType.RoundedRect);
			ButtonCancel.Frame = new CGRect(0, this.Frame.Height - 60, this.Frame.Width / 2 - 100, 100);
			ButtonCancel.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleRightMargin;
			ButtonCancel.SetTitle("Cancel", UIControlState.Normal);
			base.BackgroundColor = UIColor.White;

			foreach (UIView v in base.Subviews)
			{
				var alert = new UIAlertView("Error", v.Description, null, "Ok", null);
				alert.Show();
			}
			this.AddSubview(ButtonCancel);

		}
		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			using (CGContext g = UIGraphics.GetCurrentContext())
			{

				//set up drawing attributes
				g.SetLineWidth(0);
				UIColor.DarkGray.SetFill();
				UIColor.Black.SetStroke();

				//Create top Rect
				var path = new CGPath();

				path.AddLines(new CGPoint[]{
					new CGPoint (0, 0),
					new CGPoint (this.Frame.Width, 0),
					new CGPoint (this.Frame.Width, 225),
					new CGPoint (0, 225)});

				path.CloseSubpath();

				g.AddPath(path);
				g.DrawPath(CGPathDrawingMode.FillStroke);

				//Create bottom Rect
				path = new CGPath();

				path.AddLines(new CGPoint[]{
					new CGPoint (0, this.Frame.Height),
					new CGPoint (this.Frame.Width, this.Frame.Height),
					new CGPoint (this.Frame.Width, this.Frame.Height-225),
					new CGPoint (0, this.Frame.Height-225)});

				path.CloseSubpath();

				g.AddPath(path);
				g.DrawPath(CGPathDrawingMode.FillStroke);


				//Create left rect
				path = new CGPath();

				path.AddLines(new CGPoint[]{
					new CGPoint (0, 225),
					new CGPoint (50, 225),
					new CGPoint (50, this.Frame.Height-225),
					new CGPoint (0, this.Frame.Height-225)});

				path.CloseSubpath();

				//add geometry to graphics context and draw it
				g.AddPath(path);
				g.DrawPath(CGPathDrawingMode.FillStroke);

				//Create right rect
				path = new CGPath();

				path.AddLines(new CGPoint[]{
					new CGPoint (this.Frame.Width-50, 225),
					new CGPoint (this.Frame.Width, 225),
					new CGPoint (this.Frame.Width, this.Frame.Height-225),
					new CGPoint (this.Frame.Width-50, this.Frame.Height-225)});

				path.CloseSubpath();

				//add geometry to graphics context and draw it
				g.AddPath(path);
				g.DrawPath(CGPathDrawingMode.FillStroke);
			}

		}
		public override void LayoutSubviews()
		{
			ButtonCancel.Frame = new CGRect(this.Frame.Width/2-75, this.Frame.Height - 200, 150, 40);
			ButtonCancel.SetTitleColor(UIColor.White, UIControlState.Normal);
			ButtonCancel.Layer.BorderWidth = 1;
			ButtonCancel.Layer.BorderColor = new CGColor(255, 255, 255);
		}

	}
}

