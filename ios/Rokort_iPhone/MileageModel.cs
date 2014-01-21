using System;
using MonoTouch.UIKit;

namespace Rokort_iPhone
{
	public class MileageDataSource: UIPickerViewModel
    {
        public MileageDataSource ()
        {
        }

		#region implemented abstract members of UIPickerViewDataSource

		public override int GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public override int GetRowsInComponent (UIPickerView pickerView, int component)
		{
			return 20;
		}

		#endregion

		public override string GetTitle(UIPickerView view, int row, int component)
		{
			return ""+row;
		}
    }
}

