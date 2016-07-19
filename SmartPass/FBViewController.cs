using Foundation;
using System;
using UIKit;
using System.CodeDom.Compiler;
using LocalAuthentication;

namespace SmartPass
{
    public partial class FBViewController : UIViewController
    {

		public FBViewController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			ObjCRuntime.Class.ThrowOnInitFailure = false;
			var context = new LAContext();
			NSError AuthError;
			if (!context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
			{
				var alert = new UIAlertView("Error", "TouchID not available", null, "Ok", null);
				alert.Show();
				aLoader.Hidden = true;
				btnSkip.Hidden = false;
				btnAuth.Hidden = true;

			}
			btnMenu.TouchUpInside += (sender, e) =>
			{
				RootController.SidebarController.ToggleMenu();
			};
			btnAuth.TouchUpInside += (sender, e) =>
			{
				aLoader.Hidden = false;
				btnAuth.Hidden = true;
				AuthenticateMe();
			};

		}
		public void AuthenticateMe()
		{
			ObjCRuntime.Class.ThrowOnInitFailure = false;
			var context = new LAContext();
			NSError AuthError;
			var myReason = new NSString("Themis validation");


			if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
			{
				var replyHandler = new LAContextReplyHandler((success, error) =>
				{

					this.InvokeOnMainThread(() =>
					{
						if (success)
						{
							aLoader.Hidden = true;
							ViewPinController controller = Storyboard.InstantiateViewController("vpController") as ViewPinController;
							// Display the new view
							this.NavigationController.PushViewController(controller, true);
						}
						else {
							lbValidation.Hidden = true;
							btnAuth.Hidden = false;
						}
					});

				});
				context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, myReason, replyHandler);
			};
		}
    }
}