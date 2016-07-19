using Foundation;
using System;
using UIKit;

namespace SmartPass
{
    public partial class WebViewController : UIViewController
    {
        public WebViewController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			UIWebView webView = new UIWebView(View.Bounds);
			View.AddSubview(webView);
			View.SendSubviewToBack(webView);
			webView.ScalesPageToFit = true;
			var url = "https://svc.themis.io"; // NOTE: https secure request
			webView.LoadRequest(new NSUrlRequest(new NSUrl(url)));
		}
    }
}