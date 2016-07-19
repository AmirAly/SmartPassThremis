using Foundation;
using System;
using UIKit;
using SidebarNavigation;
namespace SmartPass
{
    public partial class RootController : UIViewController
    {
		public static SidebarController SidebarController { set; get; }

		public RootController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			var content = this.Storyboard.InstantiateViewController("fpController") as FBViewController;
			var menu = this.Storyboard.InstantiateViewController("sbController") as SideBarViewController;
			SidebarController = new SidebarController(this, content, menu);
		}
    }
}