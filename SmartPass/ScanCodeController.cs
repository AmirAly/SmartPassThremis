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
		UILabel _scanArea;
		UIButton btnCancel;
		bool _Scanned = false;
		UIViewController vc;
		public ContentView(UIColor fillColor, AVCaptureVideoPreviewLayer layer, MyMetadataOutputDelegate metadataSource)
		{
			BackgroundColor = fillColor;
			var window = UIApplication.SharedApplication.KeyWindow;
			vc = window.RootViewController;
			while (vc.PresentedViewController != null)
			{
				vc = vc.PresentedViewController;
			}
			 TopMargin = (float)this.Frame.Height / 3;
			 SideMargins = (float)this.Frame.Width / 6;
			this.layer = layer;
			layer.MasksToBounds = true;
			layer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;

			Frame = UIScreen.MainScreen.Bounds;
			layer.Frame = Frame;
			Layer.AddSublayer(layer);

			label = new UILabel(new RectangleF(TopMargin-60, SideMargins,(float)(this.Frame.Width - (SideMargins*2)),50));
			label.Text = "Align the code to the square";
			label.BackgroundColor = UIColor.White;
			label.TextColor = UIColor.Black;
			label.TextAlignment = UITextAlignment.Center;
			label.Layer.BorderColor = new CGColor(255, 255, 255);
			AddSubview(label);
			_scanArea= new UILabel(new RectangleF(TopMargin, SideMargins, SideMargins * 4,TopMargin));
			_scanArea.Layer.BorderColor = new CGColor(255, 255, 255);
			AddSubview(_scanArea);
			btnCancel = new UIButton(new RectangleF(TopMargin, SideMargins, (float)(this.Frame.Width - (SideMargins * 2)), TopMargin));
			btnCancel.TitleLabel.Text = "Cancel";
			btnCancel.Layer.BorderColor = new CGColor(255,255,255);
			btnCancel.Layer.BorderWidth = 2;
			btnCancel.SetTitle("Cancel", UIControlState.Normal);
			AddSubview(btnCancel);
			btnCancel.TouchUpInside += (sender, e) =>
			{
				var content = vc.Storyboard.InstantiateViewController("scController") as ScanCodeController;
				var menu = vc.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
				RootController.SidebarController = new SidebarController(vc, content, menu);
			};
			metadataSource.MetadataFound += (s, e) => {
				if (_Scanned)
					return;
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
			label.Text = "Align the code to the square";
			label.Layer.BorderColor = new CGColor(255, 255, 255);

			btnCancel.Frame = new CGRect((this.Frame.Width/2)-50,this.Frame.Height- 60, 100, 40);
			btnCancel.TitleLabel.Text = "Cancel";
			btnCancel.TitleLabel.TextColor = UIColor.Black;
			btnCancel.TitleLabel.TextAlignment = UITextAlignment.Center;

			_scanArea.Frame = new CGRect(SideMargins, TopMargin, SideMargins * 4, TopMargin);
			_scanArea.Layer.BorderColor = new CGColor(255, 255, 255);
			_scanArea.Layer.BorderWidth = 2;
			_scanArea.Text = "  ";
			base.LayoutSubviews();

		}
		public void generateCodeAndNavigate(string _key)
		{
			user.SetString("PEER_" + _key, "PEER");
			var content = vc.Storyboard.InstantiateViewController("acController") as AccessCodeViewController;
			var menu = vc.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
			RootController.SidebarController = new SidebarController(vc, content, menu);
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
			try{
				if (_result != null)
				{
					string _QRCode = "{\n  \"jwt\": \"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJJUCI6IjE5Mi4xNjguMS40IiwiVXNlciI6ImhkMUBpdGhlbWlzLnVrIiwiUm9sZXMiOiJ0aGVtaXNwYXNzIiwiZ3VpZCI6IjVhNmQyMmI2LWNjYjItNDcxOS1hMjJhLThlYjJhMjZmMWE2ZCIsIklkX2d1aWQiOiIwIiwiaXNzIjoic3ZjLm15bG9naW4uaW8iLCJhdWQiOiJRTkc4ZVRqUTREIiwiZXhwIjoxNDczMjQxNjYyLCJuYmYiOjE0NzMyNDA0NjJ9.NDyYHsvcrHskU2Hw3oibLeYbZeSPLYUdPYN7TpdC9FNlNMKUwi6AWjie3Jt7aRFkqQfRO0_GsZvavAgNOkE4gogG_mF5YO-zga3jgK2bdL7xRK8llgd-aLgp_88nrpWwk1jChii6dBVGiB8aKnn3TtaMVeCiOuPMFmId5RdFBNNbtkuDSkQhygT81DJvX33c9lVqhWHaExGYvwjhKOgAziGhNSov_ofe3C8xxG7Tui_WTwbq4xUl_yD9aC9mUz2J-7-hhDOjY9eY3cM2NB2AC1gJ9TyZ0_n5qLnevYask8ol3sK3OaOfv0k-7mUi1sQVxeqlVQp42UQqLoI67KGgRw\"\n}";
					_QRCode = _result.Replace("\\r\\n \\", "");
					_QRCode = _QRCode.Replace("\\r\\n", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\", "");
					_QRCode = _QRCode.Replace("\\n  \\", "");
					_QRCode = _QRCode.Replace("\\n", "");
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
				var content = vc.Storyboard.InstantiateViewController("scController") as AccessCodeViewController;
				var menu = vc.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
				RootController.SidebarController = new SidebarController(vc, content, menu);
			}
			finally
			{
				
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