// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace Rokort_iPhone
{
	[Register ("Rokort_iPhoneViewController")]
	partial class Rokort_iPhoneViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIPickerView boatPicker { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnClickMe { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIPickerView pickerView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIPickerView rowerPicker { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnClickMe != null) {
				btnClickMe.Dispose ();
				btnClickMe = null;
			}

			if (pickerView != null) {
				pickerView.Dispose ();
				pickerView = null;
			}

			if (rowerPicker != null) {
				rowerPicker.Dispose ();
				rowerPicker = null;
			}

			if (boatPicker != null) {
				boatPicker.Dispose ();
				boatPicker = null;
			}
		}
	}
}
