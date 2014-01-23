using System;
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

		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Window.RequestFeature (WindowFeatures.NoTitle);

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
			var adapterRowerNames = ArrayAdapter.CreateFromResource (
				this, Resource.Array.spinner_rower_names_array, Android.Resource.Layout.SimpleSpinnerItem);

			adapterRowerNames.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerRowerNames.Adapter = adapterRowerNames;


			Spinner spinnerBoats = FindViewById<Spinner> (Resource.Id.spinnerBoats);

			spinnerBoats.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerBoats_ItemSelected);
			var adapterBoats = ArrayAdapter.CreateFromResource (
				this, Resource.Array.spinner_boats_array, Android.Resource.Layout.SimpleSpinnerItem);

			adapterBoats.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerBoats.Adapter = adapterBoats;



			rokortService = new Rokort_Service ();
			isTripStarted = await rokortService.hasOngoingTrip ();
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

		private void spinnerRowerNames_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			String selectedRowerName = spinner.GetItemAtPosition (e.Position).ToString();
			if (selectedRowerName.Equals(stringArray[0])) {
				selectedRowerId = "1541";
			}
			else {
			    selectedRowerId = "1542";
			}
		}

		private void spinnerBoats_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			String selectedBoat = spinner.GetItemAtPosition (e.Position).ToString();

			if (selectedBoat.Equals(GetString(Resource.Array.spinner_boats_array[0]))) {
				selectedBoatId = "090";
			}
			else {
				selectedBoatId = "080";
			}
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


