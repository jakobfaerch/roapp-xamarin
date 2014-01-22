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

		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			button = FindViewById<Button> (Resource.Id.myButton);

			rokortService = new Rokort_Service ();
			isTripStarted = await rokortService.hasOngoingTrip ();
			updateUi();

			button.Click += async delegate {
				button.Enabled = false;
				if (isTripStarted) {
					await rokortService.stopTrip(0);
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
		}
	}
}


