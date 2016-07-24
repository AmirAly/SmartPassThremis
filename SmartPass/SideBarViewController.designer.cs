// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SmartPass
{
    [Register ("SideBarViewController")]
    partial class SideBarViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLegal { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPortal { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnRePeer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnLegal != null) {
                btnLegal.Dispose ();
                btnLegal = null;
            }

            if (btnPortal != null) {
                btnPortal.Dispose ();
                btnPortal = null;
            }

            if (btnRePeer != null) {
                btnRePeer.Dispose ();
                btnRePeer = null;
            }
        }
    }
}