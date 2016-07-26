using Foundation;
using System;
using UIKit;
using System.Threading;
using RadialProgress;
using System.Drawing;
using System.Timers;
using System.CodeDom.Compiler;
using LocalAuthentication;
namespace SmartPass
{
    public partial class AccessCodeViewController : UIViewController
    {
		RadialProgressView progressView;
		System.Timers.Timer t = new System.Timers.Timer();
        public AccessCodeViewController (IntPtr handle) : base (handle)
        {
        }
		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}
		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			t.Stop();
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			t.Interval = 3000;
			t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
			t.Start();
			btnMenu.TouchUpInside += (sender, e) =>
			{
				RootController.SidebarController.ToggleMenu();
			};
			btnPortal.TouchUpInside += (sender, e) =>
			{
				WebViewController.url = "https://svc.mylogin.io";
				PerformSegue("sgPortal", this);
			};
			progressView = new RadialProgressView(null,RadialProgressViewStyle.Small);
			progressView.Center = new PointF((float)View.Center.X, (float)lblC.Center.Y + 60);
			progressView.Value = 0.0f;
			progressView.ProgressColor = UIColor.Orange;
			progressView.LabelHidden = true;

			View.AddSubview(progressView);

		}
		protected void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			var elapsed = progressView.Value;
			if (elapsed >= 1)
			{
				t.Stop();
				AuthenticateMe();
			}
			else
			{
				progressView.Value += 0.2f;
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
							progressView.Value = 0;
							Random r = new Random();
							lblC.Text = r.Next(100000, 999999).ToString().Insert(3, "-");
							t.Start();

						}
						else {
							var alert = new UIAlertView("Error", "You are not authorized on this device.", null, "Ok", null);
							alert.Show();
							lblC.Text = "------";
						}
					});

				});
				context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, myReason, replyHandler);
			}
		}
    }
}