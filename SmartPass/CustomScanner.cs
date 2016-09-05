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
		nfloat TopMargin = 200;
		nfloat SideMargins = 50;
		public CustomOverlayView() : base()
		{
			ButtonCancel = UIButton.FromType(UIButtonType.RoundedRect);
			ButtonCancel.Frame = new CGRect(0, this.Frame.Height - 60, this.Frame.Width / 2 - 100, 100);
			ButtonCancel.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleRightMargin;
			ButtonCancel.SetTitle("Cancel", UIControlState.Normal);
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
			TopMargin = this.Frame.Height / 3;
			SideMargins = this.Frame.Width / 6;
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
					new CGPoint (this.Frame.Width, TopMargin),
					new CGPoint (0, TopMargin)});

				path.CloseSubpath();

				g.AddPath(path);
				g.DrawPath(CGPathDrawingMode.FillStroke);

				//Create bottom Rect
				path = new CGPath();

				path.AddLines(new CGPoint[]{
					new CGPoint (0, this.Frame.Height),
					new CGPoint (this.Frame.Width, this.Frame.Height),
					new CGPoint (this.Frame.Width, this.Frame.Height-TopMargin),
					new CGPoint (0, this.Frame.Height-TopMargin)});

				path.CloseSubpath();

				g.AddPath(path);
				g.DrawPath(CGPathDrawingMode.FillStroke);


				//Create left rect
				path = new CGPath();

				path.AddLines(new CGPoint[]{
					new CGPoint (0, TopMargin),
					new CGPoint (SideMargins, TopMargin),
					new CGPoint (SideMargins, this.Frame.Height-TopMargin),
					new CGPoint (0, this.Frame.Height-TopMargin)});

				path.CloseSubpath();

				//add geometry to graphics context and draw it
				g.AddPath(path);
				g.DrawPath(CGPathDrawingMode.FillStroke);

				//Create right rect
				path = new CGPath();

				path.AddLines(new CGPoint[]{
					new CGPoint (this.Frame.Width-SideMargins, TopMargin),
					new CGPoint (this.Frame.Width, TopMargin),
					new CGPoint (this.Frame.Width, this.Frame.Height-TopMargin),
					new CGPoint (this.Frame.Width-SideMargins, this.Frame.Height-TopMargin)});

				path.CloseSubpath();

				//add geometry to graphics context and draw it
				g.AddPath(path);
				g.DrawPath(CGPathDrawingMode.FillStroke);
			}

		}
		public override void LayoutSubviews()
		{
			ButtonCancel.Frame = new CGRect(this.Frame.Width/2-75, this.Frame.Height - TopMargin +40, 150, 40);
			ButtonCancel.SetTitleColor(UIColor.White, UIControlState.Normal);
			ButtonCancel.Layer.BorderWidth = 1;
			ButtonCancel.Layer.BorderColor = new CGColor(255, 255, 255);
		}

	}
}

