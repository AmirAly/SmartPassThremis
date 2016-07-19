using Foundation;
using System;
using UIKit;

namespace SmartPass
{
    public partial class ViewPinController : UIViewController
    {
        public ViewPinController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			UIAlertView v = new UIAlertView("Message", "This is the code", null, "Ok", null);
			//v.Show();
		}
    }
}