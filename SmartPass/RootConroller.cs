using Foundation;
using System;
using UIKit;
using SidebarNavigation;

namespace SmartPass
{
    public partial class RootConroller : UINavigationController
    {
		
		{
			base.ViewDidLoad();
			UIAlertView v = new UIAlertView("Message", "This is the code", null, "Ok", null);
			v.Show();

		}
    }
}