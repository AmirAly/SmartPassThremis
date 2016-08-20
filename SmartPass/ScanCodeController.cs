using Foundation;
using System;
using UIKit;
using SidebarNavigation;
using System.CodeDom.Compiler;
using LocalAuthentication;
using JWT;
using JWT.jws;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace SmartPass
{
    public partial class ScanCodeController : UIViewController
    {

		public static ZXing.Mobile.MobileBarcodeScanner scanner;
		public static ZXing.Result result;
		NSUserDefaults user = NSUserDefaults.StandardUserDefaults;
		public ScanCodeController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			user.StringForKey("");

			btnScan.TouchUpInside += (sender, e) =>
			{
				StartScanner();
			};
			this.tbCode.ShouldReturn += (textField) =>
			{
				textField.ResignFirstResponder();
				return true;
			};
			btnPeer.TouchUpInside += (sender, e) =>
			{
				string _Code = tbCode.Text;
				int _test;
				if (_Code.Length != 10)
					lbValidation.Hidden = false;
				else if (!int.TryParse(_Code, out _test))
				{
					lbValidation.Hidden = false;
				}
				else
				{
					generateCodeAndNavigate(tbCode.Text);

				}
			};
		}
		public void generateCodeAndNavigate(string _key)
		{
			user.SetString("PEER_"+_key, "PEER");
			var content = this.Storyboard.InstantiateViewController("acController") as AccessCodeViewController;
			var menu = this.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
			RootController.SidebarController = new SidebarController(this, content, menu);
		}
		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
							 .Where(x => x % 2 == 0)
							 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
							 .ToArray();
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
					string _QRCode = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJJUCI6IjE5Mi4xNjguMS40IiwiVXNlciI6ImhkMUBpdGhlbWlzLnVrIiwiUm9sZXMiOiJ0aGVtaXNwYXNzIiwiZ3VpZCI6IjNjOTM1MWMwLTBjMzAtNDczNy1iZThlLTBjMTU5ZGRhMWI3YyIsIklkX2d1aWQiOiIwIiwiaXNzIjoic3ZjLm15bG9naW4uaW8iLCJhdWQiOiJubjJTNDVrNEdkIiwiZXhwIjoxNDcxNDI3MjgxLCJuYmYiOjE0NzE0MjYwODF9.QVj_fslCBZyY4oeNICyaFkgNsaoqxCwg-rXsR4oD0keNAtXywyNxyAVc5AQJKnKVDh3yXq36TzVdrS2RaU7gdx8kzO1nn5otWtg-xk7I1Wq3C_dELJh4jKdv215_s3jfSaDS1nWMoa0gmuxy9_T-42kIOinTlr58UFRZVgE2-I0o9ZxrSkrKcsVqqqptkBplsplSOua5u4UQ7N6a5ApSQZae1A8Jy9Xwz_yVLDEwI-ZM_Hmjn5WvR5DD27Eai2P7pyer9tVAlMkW3qlvUFsECQMMdAHqwolMhbPqO0fBfVSueNSBVSUTtVY1csd30nFJiTW0zL_TRjtz9_74cTmmZg";
					//string Signature = "11CBBF516FA74976D1AAFF6FD295EF60A7C030405DF7DDB276A2E6E17C4E481845CD9861DFCE17ED64D139272C462D0EBEA3583782E7F589E31AC3E683B9E9E466BA186ACA 1F 2B C1 6C 9A 6D 3E CB 0C F5 5F FB 8E 38 94 08 16 6C 8D E3 28 1B C6 61 FA 16 DD E3 B3 B5 36 63 03 A3 28 B6 92 18 AE 27 0D 9A 64 30 82 03 C2 87 C5 EB 7E 8D AD 4F 60 E8 74 DF 89 06 C5 04 47 AB CC F7 F1 B3 38 FC FE 25 48 AE 90 A8 1E 0D ED 3D 1B 22 F4 98 84 7F 4E 45 B2 AE 15 3C 33 A1 28 3E 20 76 27 7F A9 95 0F B7 EC 66 79 55 9F AB 2C 9F 42 B3 DE E6 C2 88 ED 2D D4 AC 03 0D 34 D1 91 53 EB 38 84 77 30 0E F0 B2 F5 50 D5 30 60 AE E2 C4 FB F1 60 09 1B 5A 45 17 D0 A0 C9 06 3D 9E 11 23 C4 3E 8E E2 EE 2A AB A9 F4 A2 7E 0A 4D 48 EC 42 47 31 6D 94 34 B7 4C 65 F7 FE D9";
					//Signature = System.Text.RegularExpressions.Regex.Replace(Signature, @"\s+", "");
					string JWT = JsonWebToken.Decode(_QRCode, "", false);
					JObject jObject = JObject.Parse(JWT);
					tbCode.Text = jObject["aud"].ToString();
					generateCodeAndNavigate(tbCode.Text);
					btnPeer.Enabled = false;
					scanner = null;
					result = null;
					GC.Collect();
				}

		}
    }
}