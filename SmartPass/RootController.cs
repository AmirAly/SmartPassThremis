using Foundation;
using System;
using UIKit;
using SidebarNavigation;
using System.CodeDom.Compiler;
using LocalAuthentication;
using System.Threading;
namespace SmartPass
{
    public partial class RootController : UIViewController
    {
		public static SidebarController SidebarController { set; get; }

		public RootController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ObjCRuntime.Class.ThrowOnInitFailure = false;
			var context = new LAContext();
			NSError AuthError;
			if (!context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
			{
				var alert = new UIAlertView("Error", "TouchID not available", null, "Ok", null);
				alert.Show();
				UserAuthnticated();

			}
			AuthenticateMe();
		}
		public void UserAuthnticated()
		{
			var user = NSUserDefaults.StandardUserDefaults;
			string _peer = user.StringForKey("PEER");
			if (_peer != "PEER")
			{
				var content = this.Storyboard.InstantiateViewController("scController") as ScanCodeController;
				var menu = this.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
				SidebarController = new SidebarController(this, content, menu);
			}
			else
			{
				var content = this.Storyboard.InstantiateViewController("acController") as AccessCodeViewController;
				var menu = this.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
				SidebarController = new SidebarController(this, content, menu);
			}
		}
		public void AuthenticateMe()
		{
			ObjCRuntime.Class.ThrowOnInitFailure = false;
			var context = new LAContext();
			NSError AuthError;
			var myReason = new NSString("Themis Smart Pass need your authorization");
			if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
			{
				var replyHandler = new LAContextReplyHandler((success, error) =>
				{

					this.InvokeOnMainThread(() =>
					{
						if (success)
						{
							UserAuthnticated();
						}
						else {
							var alert = new UIAlertView("Error", "You are not authorized on this device.", null, "Ok", null);
							alert.Show();
						}
					});

				});
				context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, myReason, replyHandler);
			};
		}
    }
}