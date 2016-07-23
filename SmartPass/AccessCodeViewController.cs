using Foundation;
using System;
using UIKit;
using System.Threading;
using RadialProgress;
using System.Drawing;
namespace SmartPass
{
    public partial class AccessCodeViewController : UIViewController
    {
		
        public AccessCodeViewController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			btnMenu.TouchUpInside += (sender, e) =>
			{
				RootController.SidebarController.ToggleMenu();
			};
			var progressView = new RadialProgressView(null,RadialProgressViewStyle.Small);
			progressView.Center = new PointF((float)View.Center.X, (float)lblC.Center.Y + 60);
			progressView.Value = 0.7f;
			progressView.ProgressColor = UIColor.Orange;
			progressView.LabelHidden = true;

			View.AddSubview(progressView);

		}
    }
}