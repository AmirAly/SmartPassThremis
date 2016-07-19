using Foundation;
using System;
using UIKit;
using System.Threading;

namespace SmartPass
{
    public partial class FingerPrintViewController : UIViewController
    {
        public FingerPrintViewController (IntPtr handle) : base (handle)
        {
			TimerCallback timerDelegate = new TimerCallback(MoveNow);

			// Create a timer that waits one second, then invokes every second.
			Timer timer = new Timer(timerDelegate, null, 1000, 1000);
        }
		public void MoveNow(Object Status)
		{
			ViewPinController _pinCode = this.Storyboard.InstantiateViewController("ViewPinController") as ViewPinController;
			if (_pinCode != null)
			{
				this.NavigationController.PushViewController(_pinCode, true);
			}
		}
    }
}