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
    [Register ("AccessCodeViewController")]
    partial class AccessCodeViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPortal { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblC { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnMenu != null) {
                btnMenu.Dispose ();
                btnMenu = null;
            }

            if (btnPortal != null) {
                btnPortal.Dispose ();
                btnPortal = null;
            }

            if (lblC != null) {
                lblC.Dispose ();
                lblC = null;
            }
        }
    }
}