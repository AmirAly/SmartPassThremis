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
				string _QRCode = "{\r\n  \"jwt\": \"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJJUCI6IjE5Mi4xNjguMS40IiwiVXNlciI6ImhkMUBpdGhlbWlzLnVrIiwiUm9sZXMiOiJ0aGVtaXNwYXNzIiwiZ3VpZCI6IjNjOTM1MWMwLTBjMzAtNDczNy1iZThlLTBjMTU5ZGRhMWI3YyIsIklkX2d1aWQiOiIwIiwiaXNzIjoic3ZjLm15bG9naW4uaW8iLCJhdWQiOiJubjJTNDVrNEdkIiwiZXhwIjoxNDcxNDI3MjgxLCJuYmYiOjE0NzE0MjYwODF9.QVj_fslCBZyY4oeNICyaFkgNsaoqxCwg-rXsR4oD0keNAtXywyNxyAVc5AQJKnKVDh3yXq36TzVdrS2RaU7gdx8kzO1nn5otWtg-xk7I1Wq3C_dELJh4jKdv215_s3jfSaDS1nWMoa0gmuxy9_T-42kIOinTlr58UFRZVgE2-I0o9ZxrSkrKcsVqqqptkBplsplSOua5u4UQ7N6a5ApSQZae1A8Jy9Xwz_yVLDEwI-ZM_Hmjn5WvR5DD27Eai2P7pyer9tVAlMkW3qlvUFsECQMMdAHqwolMhbPqO0fBfVSueNSBVSUTtVY1csd30nFJiTW0zL_TRjtz9_74cTmmZg\"\r\n}";
				try
				{
					_QRCode = result.Text.Replace("\\r\\n \\","");
					_QRCode = _QRCode.Replace("\\r\\n","");
					_QRCode = _QRCode.Replace("\\","");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					JObject jObject_Encoded = JObject.Parse(_QRCode);
					string JWT_Encoded_String = jObject_Encoded["jwt"].ToString();
					string JWT_String_Decoded = JsonWebToken.Decode(JWT_Encoded_String, "", false);
					JObject JWT_Decoded = JObject.Parse(JWT_String_Decoded);
					tbCode.Text = JWT_Decoded["aud"].ToString();
					generateCodeAndNavigate(tbCode.Text);
					btnPeer.Enabled = false;
					scanner = null;
					result = null;
					GC.Collect();
				}
				catch (Exception ex)
				{
					var alert = new UIAlertView("Error", "Unexpected code scanned: " +_QRCode, null, "Ok", null);
					alert.Show();
				}
				}

		}
    }
}