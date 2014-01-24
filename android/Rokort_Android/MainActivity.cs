using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Rokort_iPhone;

namespace Rokort_Android
{
	[Activity (Label = "Rokort", MainLauncher = true)]
	public class MainActivity : Activity
	{
		bool isTripStarted;
		Rokort_Service rokortService;
		Button button;
		EditText editTextDistance;
		TextView textViewDistanceLabel;
		LinearLayout startTripWrapperLayout;
		LinearLayout stopTripWrapperLayout;

		String selectedRowerId;
		String selectedBoatId;

		static List<KeyValuePair<String,String>> listRowerNames = new List<KeyValuePair<String,String>> ();
		static List<KeyValuePair<String,String>> listBoatIds = new List<KeyValuePair<String,String>> ();

		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Window.RequestFeature (WindowFeatures.NoTitle);

			// Initialize data
			listRowerNames.Add (new KeyValuePair<String, String>("Jakob Roesgaard Færch", "1541"));
				listRowerNames.Add (new KeyValuePair<String, String>("Trine Roesgaard Færch", "1542"));

			listBoatIds.Add (new KeyValuePair<String, String>("ÅKS Brabrand - Ener", "090"));
			listBoatIds.Add (new KeyValuePair<String, String>("Gæstebåd", "080"));
			listBoatIds.Add (new KeyValuePair<String, String>("Ergometer", "500"));

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			startTripWrapperLayout = FindViewById<LinearLayout> (Resource.Id.start_trip_wrapper);
			stopTripWrapperLayout = FindViewById<LinearLayout> (Resource.Id.stop_trip_wrapper);
			button = FindViewById<Button> (Resource.Id.myButton);
			editTextDistance = FindViewById<EditText> (Resource.Id.editTextDistance);
			textViewDistanceLabel = FindViewById<TextView> (Resource.Id.textViewDistanceLabel);
			textViewDistanceLabel.Text = GetString (Resource.String.distance_unit_hint);


			Spinner spinnerRowerNames = FindViewById<Spinner> (Resource.Id.spinnerRowerNames);

			spinnerRowerNames.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerRowerNames_ItemSelected);
			var adapterRowerNames = new CustomArrayAdapter(
				this, listRowerNames, Resource.Layout.SpinnerItem);

			adapterRowerNames.SetDropDownViewResource (Resource.Layout.SpinnerItem);
			spinnerRowerNames.Adapter = adapterRowerNames;


			Spinner spinnerBoats = FindViewById<Spinner> (Resource.Id.spinnerBoats);

			spinnerBoats.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerBoats_ItemSelected);
			var adapterBoats = new CustomArrayAdapter(
				this, listBoatIds, Resource.Layout.SpinnerItem);

			adapterBoats.SetDropDownViewResource (Resource.Layout.SpinnerItem);
			spinnerBoats.Adapter = adapterBoats;



			rokortService = new Rokort_Service ();
			//isTripStarted = await rokortService.hasOngoingTrip ();
			updateUi();

			button.Click += async delegate {
				button.Enabled = false;
				button.Text = GetString(Resource.String.button_disabled_text);
				if (isTripStarted) {
					await rokortService.stopTrip(Convert.ToInt16(editTextDistance.Text));
					isTripStarted = false;
				} else {
					rokortService.RowerId = selectedRowerId;
					await rokortService.startTrip (selectedBoatId);
					isTripStarted = true;
				}
				updateUi();
			};
		}

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

		private void spinnerRowerNames_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			KeyValuePair<String, String> tblItem=listRowerNames[e.Position];
			selectedRowerId = tblItem.Value;
			Console.WriteLine (selectedRowerId);
		}

		private void spinnerBoats_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			KeyValuePair<String, String> tblItem=listBoatIds[e.Position];
			selectedBoatId = tblItem.Value;
			Console.WriteLine (selectedBoatId);
		}

		void updateUi ()
		{
			int buttonTitle = isTripStarted ? Resource.String.stop_trip : Resource.String.start_trip;
			button.Text = GetString (buttonTitle);
			button.Enabled = true;
			if (!isTripStarted) {
				startTripWrapperLayout.Visibility = ViewStates.Visible;
				stopTripWrapperLayout.Visibility = ViewStates.Gone;
			} else {
				startTripWrapperLayout.Visibility = ViewStates.Gone;
				stopTripWrapperLayout.Visibility = ViewStates.Visible;
			}
		}
	}
}


