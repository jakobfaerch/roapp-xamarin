using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System;


namespace Rokort_iPhone
{
	public partial class Rokort_iPhoneViewController : UIViewController
	{
		private Boolean isTripStarted = false;
		Rokort_Service rokortService;

		public Rokort_iPhoneViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Console.WriteLine (this.pickerView);
			this.pickerView.Model = new MileageDataSource ();
			this.pickerView.Hidden = true;

			rokortService = new Rokort_Service ();

			//---- wire up our click me button
			this.btnClickMe.TouchUpInside += async (sender, e) => {
				this.btnClickMe.Enabled = false;
				if (isTripStarted) {
					await rokortService.stopTrip(this.pickerView.SelectedRowInComponent(0));
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
			var buttonTitle = isTripStarted ? "Stop tur" : "Start tur";
			this.btnClickMe.SetTitle (buttonTitle, UIControlState.Normal);
			this.btnClickMe.Enabled = true;
			this.pickerView.Hidden = !isTripStarted;
		}
	}
}
