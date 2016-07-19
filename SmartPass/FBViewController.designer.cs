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
    [Register ("FBViewController")]
    partial class FBViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView aLoader { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAuth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSkip { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbValidation { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (aLoader != null) {
                aLoader.Dispose ();
                aLoader = null;
            }

            if (btnAuth != null) {
                btnAuth.Dispose ();
                btnAuth = null;
            }

            if (btnMenu != null) {
                btnMenu.Dispose ();
                btnMenu = null;
            }

            if (btnSkip != null) {
                btnSkip.Dispose ();
                btnSkip = null;
            }

            if (lbValidation != null) {
                lbValidation.Dispose ();
                lbValidation = null;
            }
        }
    }
}