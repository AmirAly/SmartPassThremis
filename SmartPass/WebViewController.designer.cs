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
    [Register ("WebViewController")]
    partial class WebViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCloseWebView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCloseWebView != null) {
                btnCloseWebView.Dispose ();
                btnCloseWebView = null;
            }
        }
    }
}