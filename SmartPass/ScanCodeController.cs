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
using System.Drawing;
using AVFoundation;
using Foundation;
using System.Drawing;
using CoreGraphics;
using CoreFoundation;
namespace SmartPass
{
	public class ContentView : UIView
	{
		AVCaptureVideoPreviewLayer layer;

		public ContentView(UIColor fillColor, AVCaptureVideoPreviewLayer layer, MyMetadataOutputDelegate metadataSource)
		{
			BackgroundColor = fillColor;


			this.layer = layer;
			layer.MasksToBounds = true;
			layer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;

			Frame = UIScreen.MainScreen.Bounds;
			layer.Frame = Frame;
			Layer.AddSublayer(layer);

			var label = new UILabel(new RectangleF(40, 80, 100, 80));
			label.Text = "Scanning ...";
			AddSubview(label);

			metadataSource.MetadataFound += (s, e) => label.Text = e.StringValue;

		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			layer.Frame = Bounds;
		}
	}
	public class MyMetadataOutputDelegate : AVCaptureMetadataOutputObjectsDelegate
	{
		public override void DidOutputMetadataObjects(AVCaptureMetadataOutput captureOutput, AVMetadataObject[] metadataObjects, AVCaptureConnection connection)
		{
			foreach (var m in metadataObjects)
			{
				if (m is AVMetadataMachineReadableCodeObject)
				{
					MetadataFound(this, m as AVMetadataMachineReadableCodeObject);
				}
			}
		}

		public event EventHandler<AVMetadataMachineReadableCodeObject> MetadataFound = delegate { };
	}
    public partial class ScanCodeController : UIViewController
    {
		AVCaptureSession session;
		AVCaptureMetadataOutput metadataOutput;
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
		}
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
			session = new AVCaptureSession();
			var camera = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
			var input = AVCaptureDeviceInput.FromDevice(camera);
			session.AddInput(input);

			//Add the metadata output channel
			metadataOutput = new AVCaptureMetadataOutput();
			var metadataDelegate = new MyMetadataOutputDelegate();
			metadataOutput.SetDelegate(metadataDelegate, DispatchQueue.MainQueue);
			session.AddOutput(metadataOutput);
			//Confusing! *After* adding to session, tell output what to recognize...

			metadataOutput.MetadataObjectTypes = AVMetadataObjectType.QRCode;

			var previewLayer = new AVCaptureVideoPreviewLayer(session);
			var view = new ContentView(UIColor.Blue, previewLayer, metadataDelegate);

			session.StartRunning();

			this.View = view;

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
			scanner = new ZXing.Mobile.MobileBarcodeScanner();
			scanner.UseCustomOverlay = false;
			result = await scanner.Scan();
		}
		public  void StartScanner_(ZXing.Result result)
		{
			try
			{
				if (result != null)
				{
					string _QRCode = "{\r\n  \"jwt\": \"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJJUCI6IjE5Mi4xNjguMS40IiwiVXNlciI6ImhkMUBpdGhlbWlzLnVrIiwiUm9sZXMiOiJ0aGVtaXNwYXNzIiwiZ3VpZCI6IjNjOTM1MWMwLTBjMzAtNDczNy1iZThlLTBjMTU5ZGRhMWI3YyIsIklkX2d1aWQiOiIwIiwiaXNzIjoic3ZjLm15bG9naW4uaW8iLCJhdWQiOiJubjJTNDVrNEdkIiwiZXhwIjoxNDcxNDI3MjgxLCJuYmYiOjE0NzE0MjYwODF9.QVj_fslCBZyY4oeNICyaFkgNsaoqxCwg-rXsR4oD0keNAtXywyNxyAVc5AQJKnKVDh3yXq36TzVdrS2RaU7gdx8kzO1nn5otWtg-xk7I1Wq3C_dELJh4jKdv215_s3jfSaDS1nWMoa0gmuxy9_T-42kIOinTlr58UFRZVgE2-I0o9ZxrSkrKcsVqqqptkBplsplSOua5u4UQ7N6a5ApSQZae1A8Jy9Xwz_yVLDEwI-ZM_Hmjn5WvR5DD27Eai2P7pyer9tVAlMkW3qlvUFsECQMMdAHqwolMhbPqO0fBfVSueNSBVSUTtVY1csd30nFJiTW0zL_TRjtz9_74cTmmZg\"\r\n}";
					_QRCode = result.Text.Replace("\\r\\n \\", "");
					_QRCode = _QRCode.Replace("\\r\\n", "");
					_QRCode = _QRCode.Replace("\\", "");
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
					long _ticks = long.Parse(JWT_Decoded["exp"].ToString());
					DateTime _exp = new DateTime(_ticks);
					if (_exp >= DateTime.Now)
					{
						var alert = new UIAlertView("Error", "This token is expired", null, "Ok", null);
						alert.Show();
						return;
					}
					tbCode.Text = JWT_Decoded["aud"].ToString();
					user.SetString(JWT_Decoded["iss"].ToString(), "PORTAL");
					generateCodeAndNavigate(tbCode.Text);
					scanner = null;
					result = null;
					GC.Collect();
				}
			}
			catch (Exception ex)
			{
				var alert = new UIAlertView("Error", ex.Message, null, "Ok", null);
				alert.Show();
			}
		}
    }
}