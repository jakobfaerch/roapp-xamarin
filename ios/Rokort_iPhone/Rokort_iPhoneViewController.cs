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

		BoatModel boatModel;

		RowerModel rowerModel;

		public Rokort_iPhoneViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}

        public override async void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);

            setInitialUiState();

            rokortService = new Rokort_Service ();

            // Possible TODO: Detect changes to the rower picker and update isTripStarted and UIstate

            setupPickerModels();

            this.rowerPicker.Select (rowerModel.GetRowForRowerId(rokortService.RowerId), 0, true);

            await updateUi();
        }
		
		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad();

            this.btnClickMe.TouchUpInside += async (sender, e) => {
				this.btnClickMe.Enabled = false;
				if (isTripStarted) {
                    await rokortService.stopTrip(this.pickerView.SelectedRowInComponent(0));
				} else {
                    rokortService.RowerId = rowerModel.getRowerId(rowerPicker);
                    await rokortService.startTrip (boatModel.GetBoatId(boatPicker));
				}
                await updateUi();
			};
		}

        void setupPickerModels ()
        {
            this.pickerView.Model = new MileageModel ();
            rowerModel = new RowerModel ();
            this.rowerPicker.Model = rowerModel;
            boatModel = new BoatModel ();
            this.boatPicker.Model = boatModel;
        }

        void setInitialUiState()
        {
            this.btnClickMe.SetTitle ("...", UIControlState.Disabled);
            this.pickerView.Hidden = true;
            this.rowerPicker.Hidden = true;
            this.boatPicker.Hidden = true;
        }

        async Task updateUi ()
		{
            this.btnClickMe.Enabled = false;
            isTripStarted = await rokortService.hasOngoingTrip ();
			var buttonTitle = isTripStarted ? "Stop tur" : "Start tur";
			this.btnClickMe.SetTitle (buttonTitle, UIControlState.Normal);
			this.btnClickMe.Enabled = true;
            this.pickerView.Hidden = !isTripStarted;
			this.boatPicker.Hidden = isTripStarted;
			this.rowerPicker.Hidden = isTripStarted;
		}
	}
}
