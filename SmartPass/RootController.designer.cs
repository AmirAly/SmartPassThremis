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
    [Register ("RootController")]
    partial class RootController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSegScan { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSegScan != null) {
                btnSegScan.Dispose ();
                btnSegScan = null;
            }
        }
    }
}