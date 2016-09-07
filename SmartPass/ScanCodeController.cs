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
		NSUserDefaults user = NSUserDefaults.StandardUserDefaults;
		float TopMargin;
		float SideMargins;
		UILabel label;
		UILabel lblSq;
		public ContentView(UIColor fillColor, AVCaptureVideoPreviewLayer layer, MyMetadataOutputDelegate metadataSource)
		{
			BackgroundColor = fillColor;

			 TopMargin = (float)this.Frame.Height / 3;
			 SideMargins = (float)this.Frame.Width / 6;
			this.layer = layer;
			layer.MasksToBounds = true;
			layer.VideoGravity = AVLayerVideoGravity.ResizeAspect;

			Frame = UIScreen.MainScreen.Bounds;
			layer.Frame = Frame;
			Layer.AddSublayer(layer);

			label = new UILabel(new RectangleF(TopMargin-60, SideMargins,(float)(this.Frame.Width - (SideMargins*2)),50));
			label.Text = "Please center the code in the middle of te square";
			label.BackgroundColor = UIColor.Black;
			AddSubview(label);
			lblSq = new UILabel(new RectangleF(TopMargin, SideMargins, (float)(this.Frame.Width - (SideMargins * 2)), TopMargin));
			lblSq.Text = "_________________________";
			lblSq.Layer.BorderColor = new CGColor(255,255,255);
			lblSq.Layer.BorderWidth = 2;
			AddSubview(lblSq);
			bool _Scanned = false;
			metadataSource.MetadataFound += (s, e) => {
				if (_Scanned)
					return;
				var alert = new UIAlertView("Info", "Captured", null, "Ok", null);
				alert.Show();
				_Scanned = true;
				processQRCode(e.StringValue);
			};

		}
		public override void LayoutSubviews()
		{

			layer.Frame = Bounds;
			TopMargin = (float)this.Frame.Height / 3;
			SideMargins = (float)this.Frame.Width / 6;

			label.Frame = new CGRect(0,TopMargin-70, SideMargins*6, 60);
			label.TextColor = UIColor.Black;
			label.Text = "Please center the code in the middle of te square";
			lblSq.Frame = new CGRect(SideMargins, TopMargin, (float)(this.Frame.Width - (SideMargins * 2)), TopMargin);
			lblSq.Text = "";
			label.BackgroundColor = UIColor.White;
			label.TextAlignment = UITextAlignment.Center;
			base.LayoutSubviews();

		}
		public void generateCodeAndNavigate(string _key)
		{
			user.SetString("PEER_" + _key, "PEER");
			var content = this.InputViewController.Storyboard.InstantiateViewController("acController") as AccessCodeViewController;
			var menu = this.InputViewController.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
			RootController.SidebarController = new SidebarController(this.InputViewController, content, menu);
		}
		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
							 .Where(x => x % 2 == 0)
							 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
							 .ToArray();
		}
		public void processQRCode(string _result)
		{
			try
			{
				if (_result != null)
				{
					string _QRCode = "{\r\n  \"jwt\": \"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJJUCI6IjE5Mi4xNjguMS40IiwiVXNlciI6ImhkMUBpdGhlbWlzLnVrIiwiUm9sZXMiOiJ0aGVtaXNwYXNzIiwiZ3VpZCI6IjNjOTM1MWMwLTBjMzAtNDczNy1iZThlLTBjMTU5ZGRhMWI3YyIsIklkX2d1aWQiOiIwIiwiaXNzIjoic3ZjLm15bG9naW4uaW8iLCJhdWQiOiJubjJTNDVrNEdkIiwiZXhwIjoxNDcxNDI3MjgxLCJuYmYiOjE0NzE0MjYwODF9.QVj_fslCBZyY4oeNICyaFkgNsaoqxCwg-rXsR4oD0keNAtXywyNxyAVc5AQJKnKVDh3yXq36TzVdrS2RaU7gdx8kzO1nn5otWtg-xk7I1Wq3C_dELJh4jKdv215_s3jfSaDS1nWMoa0gmuxy9_T-42kIOinTlr58UFRZVgE2-I0o9ZxrSkrKcsVqqqptkBplsplSOua5u4UQ7N6a5ApSQZae1A8Jy9Xwz_yVLDEwI-ZM_Hmjn5WvR5DD27Eai2P7pyer9tVAlMkW3qlvUFsECQMMdAHqwolMhbPqO0fBfVSueNSBVSUTtVY1csd30nFJiTW0zL_TRjtz9_74cTmmZg\"\r\n}";
					_QRCode = _result.Replace("\\r\\n \\", "");
					_QRCode = _QRCode.Replace("\\r\\n", "");
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
					var _aud = JWT_Decoded["aud"].ToString();
					user.SetString(JWT_Decoded["iss"].ToString(), "PORTAL");
					generateCodeAndNavigate(_aud);
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
		public ScanCodeController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			btnScan.TouchUpInside += (sender, e) =>
			{
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

			};
			this.tbCode.ShouldReturn += (textField) =>
			{
				textField.ResignFirstResponder();
				return true;
			};

		}

    }
}