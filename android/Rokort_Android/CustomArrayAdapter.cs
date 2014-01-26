using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Rokort_Android
{
	public class CustomArrayAdapter : ArrayAdapter
	{
		List<KeyValuePair<String, String>> _listItems;

		public CustomArrayAdapter(Context context, List<KeyValuePair<String, String>> listItems, int id) : base(context, id)
		{
			_listItems = listItems;
		}

		public override int Count {
			get { return _listItems.Count; }
		}

		public override Java.Lang.Object GetItem (int position) {
			KeyValuePair<String, String> tblItem = _listItems[position];
			return tblItem.Key;
		}
	}
}

