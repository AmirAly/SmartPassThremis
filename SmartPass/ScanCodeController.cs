using Foundation;
using System;
using UIKit;
using SidebarNavigation;
namespace SmartPass
{
    public partial class ScanCodeController : UIViewController
    {

		public static ZXing.Mobile.MobileBarcodeScanner scanner;
		public static ZXing.Result result;
		public ScanCodeController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var user = NSUserDefaults.StandardUserDefaults;
			user.StringForKey("");

			btnScan.TouchUpInside += (sender, e) =>
			{
				StartScanner();
			};

			btnPeer.TouchUpInside += (sender, e) =>
			{
				string _Code = tbCode.Text;
				int _test;
				if (_Code.Length != 6)
					lbValidation.Hidden = false;
				else if (!int.TryParse(_Code, out _test))
				{
					lbValidation.Hidden = false;
				}
				else
				{
					user.SetString("PEER", "PEER");
					var content = this.Storyboard.InstantiateViewController("acController") as AccessCodeViewController;
					var menu = this.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
					RootController.SidebarController = new SidebarController(this, content, menu);

				}
			};
		}
		public async void StartScanner()
		{
			if (scanner == null)
			{
				scanner = new ZXing.Mobile.MobileBarcodeScanner();
				scanner.BottomText = "Or enter it manually";
				scanner.TopText = "Please align the code in the middle of the view";
				scanner.CancelButtonText = "Enter manually";

			}
				result = await scanner.Scan();
				if (result != null)
				{
					tbCode.Text = result.Text;
					btnPeer.Enabled = false;
					scanner = null;
					result = null;
					GC.Collect();
				}

		}
    }
}