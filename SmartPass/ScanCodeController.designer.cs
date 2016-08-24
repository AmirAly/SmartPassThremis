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
    [Register ("ScanCodeController")]
    partial class ScanCodeController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnScan { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField tbCode { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnScan != null) {
                btnScan.Dispose ();
                btnScan = null;
            }

            if (tbCode != null) {
                tbCode.Dispose ();
                tbCode = null;
            }
        }
    }
}