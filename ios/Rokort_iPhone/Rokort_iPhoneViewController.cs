using MonoTouch.UIKit;
using System.Drawing;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace Rokort_iPhone
{
	public partial class Rokort_iPhoneViewController : UIViewController
	{
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
			
			//any additional setup after loading the view, typically from a nib.
			
			//---- wire up our click me button
			this.btnClickMe.TouchUpInside += (sender, e) => {
				startTrip ();
			};

		}

		static CookieContainer cookies = new CookieContainer();

		static HttpClient makeHttpClient ()
		{
			var handler = new HttpClientHandler {
				UseCookies = false,
				UseDefaultCredentials = false,
				Proxy = new WebProxy ("http://10.0.1.6:8888", false, new string[] {}),
				UseProxy = true,
			};
			HttpClient hc = new HttpClient (handler);
			return hc;
		}

		static async void startTrip()
		{
			var hc = makeHttpClient ();

			String sessionCookie = await login(hc);

			HttpContent content = new FormUrlEncodedContent(
				new List<KeyValuePair<string, string>> { 
					new KeyValuePair<string, string>("key1", "value1")
				});
			content.Headers.Add ("Cookie", sessionCookie);
			await hc.PostAsync ("http://www.rokort.dk/workshop/row_update.php", content);

			Console.WriteLine ("Turen er startet");
		}

		static async Task<String> login (HttpClient hc)
		{
			Task<HttpResponseMessage> getCookieTask = hc.GetAsync ("http://www.rokort.dk/workshop/index.php?siteid=4&guid=61BED0F9-CD54-4605-AD95-76785A6678D7");
			HttpResponseMessage cookieResponse = await getCookieTask;
			Console.WriteLine (cookieResponse.Headers.ToString ());
			var cookieHeaderValue = cookieResponse.Headers.GetValues ("Set-Cookie").First ().Split(';')[0];
			Console.WriteLine (cookieHeaderValue);
			return cookieHeaderValue;
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Release any retained subviews of the main view.
			// e.g. this.myOutlet = null;
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}
