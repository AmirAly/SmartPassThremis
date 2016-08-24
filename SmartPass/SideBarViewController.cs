using Foundation;
using System;
using UIKit;
using SidebarNavigation;
namespace SmartPass
{
    public partial class SideBarViewController : UIViewController
    {
		NSUserDefaults user = NSUserDefaults.StandardUserDefaults;
		public SideBarViewController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			btnPortal.TouchUpInside += (sender, e) =>
			{
				var _url = user.StringForKey("PORTAL");
				if (_url == null)
					_url = "https://svc.mylogin.io";
				WebViewController.url = _url;
				PerformSegue("sgWebView",this);
			};
			btnLegal.TouchUpInside += (sender, e) =>
			{
				WebViewController.url = "https://svc.themis.io/legal.html";
				PerformSegue("sgWebView", this);

			};
			btnRePeer.TouchUpInside += (sender, e) => {
				//var user = NSUserDefaults.StandardUserDefaults;
				//user.SetString("", "PEER");
				//PerformSegue("sgRePeer", this
				var content = this.Storyboard.InstantiateViewController("scController") as ScanCodeController;
				RootController.SidebarController.ChangeContentView(content);
				//var menu = this.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
				//RootController.SidebarController = new SidebarController(this, content, menu);


			};
			base.ViewDidLoad();

		}
    }
}