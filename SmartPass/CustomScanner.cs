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
	public class CustomOverlayView : ZXing.Mobile.ZXingScannerView
	{
		public UIButton ButtonCancel;
		nfloat TopMargin = 200;
		nfloat SideMargins = 50;
		public CustomOverlayView()
		{
			this.StartScanning(HandleScan);
			this.CancelButtonText = "";
			ButtonCancel = UIButton.FromType(UIButtonType.RoundedRect);
			ButtonCancel.Frame = new CGRect(0, this.Frame.Height - 60, this.Frame.Width / 2 - 100, 100);
			ButtonCancel.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleRightMargin;
			ButtonCancel.SetTitle("Cancel", UIControlState.Normal);
			this.Subviews[0].AddSubview(ButtonCancel);
		}
		public void HandleScan(ZXing.Result _res)
		{
			//return _res;
		}
		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
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

