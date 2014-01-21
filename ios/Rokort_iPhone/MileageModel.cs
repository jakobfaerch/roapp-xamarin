using System;
using MonoTouch.UIKit;

namespace Rokort_iPhone
{
	public class MileageModel: UIPickerViewModel
    {
        public MileageModel ()
        {
        }

		public override int GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public override int GetRowsInComponent (UIPickerView pickerView, int component)
		{
			return 20;
		}

		public override UIView GetView (UIPickerView picker, int row, int component, UIView view)
		{
			var label = new UILabel ();
			label.Text = "" + row;
			label.TextColor = UIColor.White;
			label.TextAlignment = UITextAlignment.Center;
			return label;
		}
    }
}

