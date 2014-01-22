using System;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace Rokort_iPhone
{
	public class BoatModel: UIPickerViewModel
    {
		Dictionary<int, string> boatNames;
		Dictionary<int, string> boatIds;

		public BoatModel ()
        {
			boatNames = new Dictionary<int,string> ();
			boatNames.Add (0, "ÅKS Brabrand - ener");
			boatNames.Add (1, "Gæstebåd");
			boatNames.Add (2, "Ergometer");

			boatIds = new Dictionary<int, string> ();
			boatIds.Add (0, "090");
			boatIds.Add (1, "080");
			boatIds.Add (2, "500");
        }

		public override int GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public override int GetRowsInComponent (UIPickerView pickerView, int component)
		{
			return 3;
		}

		public override UIView GetView (UIPickerView picker, int row, int component, UIView view)
		{
			var label = new UILabel ();
			label.Text = boatNames [row];
			label.TextColor = UIColor.White;
			label.TextAlignment = UITextAlignment.Center;
			label.Font = UIFont.FromName("Helvetica-Light", 18f);

			return label;
		}

		public string GetBoatId(UIPickerView picker) 
		{
			return boatIds [picker.SelectedRowInComponent(0)];
		}
    }
}

