using Foundation;
using System;
using UIKit;
using SidebarNavigation;
using System.Drawing;
namespace SmartPass
{
    public partial class WebViewController : UIViewController
    {
		public static string url = "";
        public WebViewController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			btnCloseWebView.TouchUpInside += (sender, e) =>
			{
				var content = this.Storyboard.InstantiateViewController("acController") as AccessCodeViewController;
				var menu = this.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
				RootController.SidebarController = new SidebarController(this, content, menu);
			};
			UIWebView webView = new UIWebView(View.Bounds);
			webView.Center = new PointF((float)View.Center.X, (float)View.Center.Y);
			View.AddSubview(webView);
			View.SendSubviewToBack(webView);
			webView.ScalesPageToFit = true;
			webView.LoadRequest(new NSUrlRequest(new NSUrl(url)));
		}
    }
}