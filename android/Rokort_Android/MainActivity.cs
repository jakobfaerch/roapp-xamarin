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

		protected override void OnCreate (Bundle bundle)
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

			Spinner spinnerRowerNames = FindViewById<Spinner> (Resource.Id.spinnerRowerNames);
			spinnerRowerNames.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerRowerNames_ItemSelected);
			configureSpinner (spinnerRowerNames, new CustomArrayAdapter (this, listRowerNames, Resource.Layout.SpinnerItem));

			Spinner spinnerBoats = FindViewById<Spinner> (Resource.Id.spinnerBoats);
			spinnerBoats.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerBoats_ItemSelected);
			configureSpinner (spinnerBoats, new CustomArrayAdapter (this, listBoatIds, Resource.Layout.SpinnerItem));

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

		void configureSpinner (Spinner spinnerRowerNames, ArrayAdapter adapter)
		{
			adapter.SetDropDownViewResource (Resource.Layout.SpinnerItem);
			spinnerRowerNames.Adapter = adapter;
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


