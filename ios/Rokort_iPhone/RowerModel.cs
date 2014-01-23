using System;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace Rokort_iPhone
{
	public class RowerModel: UIPickerViewModel
    {
		Dictionary<int, string> rowerNames;

		Dictionary<int, string> rowerIds;

		public RowerModel ()
        {
			rowerNames = new Dictionary<int, string> ();
			rowerNames.Add (0, "Trine Roesgaard Færch");
			rowerNames.Add (1, "Jakob Roesgaard Færch");

			rowerIds = new Dictionary<int, string> ();
			rowerIds.Add (0, "1542");
			rowerIds.Add (1, "1541");

        }

		public override int GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public override int GetRowsInComponent (UIPickerView pickerView, int component)
		{
			return 2;
		}

        public int GetRowForRowerId (string rowerId)
        {
            foreach(var entry in rowerIds)
            {
                if (entry.Value == rowerId) {
                    return entry.Key;
                }
            }

            return 0;
        }

		public override UIView GetView (UIPickerView picker, int row, int component, UIView view)
		{
			var label = new UILabel ();
			label.Text = rowerNames [row];
			label.TextColor = UIColor.White;
			label.TextAlignment = UITextAlignment.Center;
			label.Font = UIFont.FromName("Helvetica-Light", 18f);

			return label;
		}

		public string getRowerId(UIPickerView picker)
		{
			return rowerIds [picker.SelectedRowInComponent (0)];
		}

        public override void DidChangeValue (string forKey)
        {
            base.DidChangeValue (forKey);
            Console.WriteLine ("Rower change for key" + forKey);
        }
    }
}

