using Foundation;
using System;
using UIKit;
using System.Threading;
using RadialProgress;
using System.Drawing;
using System.Timers;
using System.CodeDom.Compiler;
using LocalAuthentication;
using System.Security.Cryptography;
using System.Linq;
namespace SmartPass
{
    public partial class AccessCodeViewController : UIViewController
    {
		NSUserDefaults user = NSUserDefaults.StandardUserDefaults;
		string _peer;
		RadialProgressView progressView;
		System.Timers.Timer t = new System.Timers.Timer();
		static string allowedCharacters = "ABCDEFGHIGKLMNOPQRSTUVWXYZabcdefghigklmnopqrstuvwxyz0123456789";
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
			_peer = user.StringForKey("PEER");
			_peer = _peer.Replace("PEER_", "");
			lblC.Text = _peer;
			lblC.Text = GetCode(_peer);
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
				progressView.Value += 0.1f;
			}
		}
		private static string GetCode(string secretKey)
		{
			long timeIndex = DateTime.Now.Ticks;
			var secretKeyBytes = Base32Encode(secretKey);
			HMACSHA1 hmac = new HMACSHA1(secretKeyBytes);
			byte[] challenge = BitConverter.GetBytes(timeIndex);
			if (BitConverter.IsLittleEndian) Array.Reverse(challenge);
			byte[] hash = hmac.ComputeHash(challenge);
			int offset = hash[19] & 0xf;
			int truncatedHash = hash[offset] & 0x7f;
			for (int i = 1; i < 4; i++)
			{
				truncatedHash <<= 8;
				truncatedHash |= hash[offset + i] & 0xff;
			}
			truncatedHash %= 1000000;
			return truncatedHash.ToString("D6");
		}

		private static byte[] Base32Encode(string source)
		{
			var bits = source.ToUpper().ToCharArray().Select(c =>
				Convert.ToString(allowedCharacters.IndexOf(c), 2).PadLeft(5, '0')).Aggregate((a, b) => a + b);
			return Enumerable.Range(0, bits.Length / 8).Select(i => Convert.ToByte(bits.Substring(i * 8, 8), 2)).ToArray();
		}
		public void Recycle()
		{
			progressView.Value = 0;
			lblC.Text = GetCode(_peer);
			t.Start();

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
							Recycle();
						}
						else {
							var alert = new UIAlertView("Error", "You are not authorized on this device.", null, "Ok", null);
							alert.Show();
							lblC.Text = "------";
							Recycle();
						}
					});

				});
				context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, myReason, replyHandler);
			}
		}
    }
}