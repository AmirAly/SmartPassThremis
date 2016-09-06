﻿using System;
using UIKit;

namespace SmartPass
{
	public static class QRCodeGlobalObject
	{
		public static AppDelegate TheAppDel{get;set;}
		public static QRCodeReaderViewController TheQRCodeReaderViewController{get;set;}


		#region ExtendMethod

		public static void ShowQRCodeReaderViewController(this UIViewController controller,QRCodeReaderCallback callback)
		{
			QRCodeReaderViewController.Intance().Show(controller.NavigationController,(result)=>{
				callback.Invoke(result);
			});
		}

		#endregion
	}
}

