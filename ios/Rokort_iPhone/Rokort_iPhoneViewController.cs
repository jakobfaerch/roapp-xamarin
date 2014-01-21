using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System;
using System.Threading.Tasks;


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
		
		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.btnClickMe.Enabled = false;
			updateUi();

			this.pickerView.Model = new MileageModel ();

			rokortService = new Rokort_Service ();

			isTripStarted = await rokortService.hasOngoingTrip ();
			updateUi();
			Console.WriteLine("hasOnGoingTrip: " + isTripStarted);

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
