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

		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Window.RequestFeature (WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			button = FindViewById<Button> (Resource.Id.myButton);
			editTextDistance = FindViewById<EditText> (Resource.Id.editTextDistance);
			textViewDistanceLabel = FindViewById<TextView> (Resource.Id.textViewDistanceLabel);
			textViewDistanceLabel.Text = GetString (Resource.String.distance_unit_hint);

			rokortService = new Rokort_Service ();
			isTripStarted = await rokortService.hasOngoingTrip ();
			updateUi();

			button.Click += async delegate {
				button.Enabled = false;
				if (isTripStarted) {
					await rokortService.stopTrip(Convert.ToInt16(editTextDistance.Text));
					isTripStarted = false;
				} else {
					await rokortService.startTrip ();
					isTripStarted = true;
				}
				updateUi();
			};
		}

		void updateUi ()
		{
			int buttonTitle = isTripStarted ? Resource.String.stop_trip : Resource.String.start_trip;
			button.Text = GetString (buttonTitle);
			button.Enabled = true;
			if (!isTripStarted) {
				textViewDistanceLabel.Visibility = ViewStates.Invisible;
				editTextDistance.Visibility = ViewStates.Invisible;
			} else {
				textViewDistanceLabel.Visibility = ViewStates.Visible;
				editTextDistance.Visibility = ViewStates.Visible;
			}
		}
	}
}


