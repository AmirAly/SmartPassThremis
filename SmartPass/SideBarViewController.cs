using Foundation;
using System;
using UIKit;

namespace SmartPass
{
    public partial class SideBarViewController : UIViewController
    {
        public SideBarViewController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			btnPortal.TouchUpInside += (sender, e) =>
			{
				WebViewController.url = "https://svc.mylogin.io";
				PerformSegue("sgWebView",this);
			};
			btnLegal.TouchUpInside += (sender, e) =>
			{
				WebViewController.url = "https://svc.themis.io/legal.html";
				PerformSegue("sgWebView", this);

			};
			base.ViewDidLoad();

		}
    }
}