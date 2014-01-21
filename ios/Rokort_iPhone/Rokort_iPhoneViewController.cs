using MonoTouch.UIKit;
using System.Drawing;
using System;
using System.Text.RegularExpressions;
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
		DateTime tripStartTime;
		private Boolean isTripStarted = false;
		private const string RowerId = "1541";
		private const string apiDateFormat = "yyyy-MM-dd hh:mm:ss";
		HttpClient hc;

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

			hc = makeHttpClient ();

			//---- wire up our click me button
			this.btnClickMe.TouchUpInside += (sender, e) => {
				this.btnClickMe.Enabled = false;
				if (isTripStarted) {
					stopTrip();
				} else {
					startTrip ();
				}
			};

		}

		HttpClient makeHttpClient ()
		{
			var handler = new HttpClientHandler {
				UseCookies = false,
				UseDefaultCredentials = false,
				Proxy = new WebProxy ("http://192.168.1.122:8888", false, new string[] {}),
				UseProxy = true,
			};
			HttpClient hc = new HttpClient (handler);
			return hc;
		}

		async void startTrip()
		{
			String sessionCookie = await login(hc);

			tripStartTime = DateTime.Now;

			var content = new ContentForRequest () {
				StartDateTime = tripStartTime
			}.build (sessionCookie);

			var response = await hc.PostAsync ("http://www.rokort.dk/workshop/row_update.php", content);

			this.btnClickMe.SetTitle("Stop tur", UIControlState.Normal);
			this.btnClickMe.Enabled = true;
			isTripStarted = true;
			Console.WriteLine ("Turen er startet, http status " + response.StatusCode);
		}

		class ContentForRequest
		{
			public ContentForRequest() {
				ID = "";
				xStart = null;
				xEnd = null;
				CheckPermission = "1";
				CheckDamages = "1";
				CheckBoatOrder = "1";
				CheckReservations = "1";
				action = "update";
				BoatID = "090";
				guest = "";
				route = "1";
				Description = "Brabrand Sø";
				StartDateTime = null;
				EndDateTime = null;
				Distance = "";
				Rower_list = "~" + RowerId;
				Member_list = "~A#~1006#~95#~1023#~89#~528#~137#~548#~447#~515#~330#~43#~34#~506#~243#~22#~386#~494#~77#~502#~498#~B#~144#~328#~390#~513#~393#~1002#~99#~412#~138#~360#~313#~91#~1022#~1031#~C#~175#~38#~422#~485#~446#~421#~15#~331#~120#~539#~391#~1550#~1540#~D#~188#~265#~404#~381#~152#~529#~116#~E#~28#~349#~1557#~511#~311#~210#~202#~172#~457#~69#~535#~107#~434#~406#~140#~F#~174#~426#~30#~3#~4#~G#~80#~113#~500#~9#~H#~196#~45#~473#~497#~12#~63#~222#~145#~129#~206#~546#~78#~474#~61#~20#~26#~I#~1539#~459#~427#~466#~450#~319#~477#~411#~52#~J#~432#~490#~1001#~146#~255#~92#~468#~443#~407#~514#~517#~114#~148#~491#~399#~352#~220#~463#~332#~186#~516#~284#~405#~32#~121#~540#~82#~K#~241#~155#~533#~208#~305#~508#~346#~388#~132#~L#~166#~501#~471#~436#~1#~1556#~211#~350#~442#~441#~430#~66#~542#~428#~273#~1003#~504#~103#~1012#~512#~42#~488#~351#~544#~532#~256#~2000#~M#~21#~518#~537#~482#~59#~354#~94#~538#~505#~479#~543#~503#~536#~415#~509#~176#~214#~254#~438#~224#~178#~205#~N#~445#~1029#~36#~76#~444#~359#~163#~338#~149#~180#~O#~14#~17#~1559#~522#~106#~18#~P#~85#~246#~253#~62#~519#~1028#~343#~8#~209#~193#~425#~368#~98#~207#~1027#~402#~401#~355#~1019#~1013#~R#~160#~312#~295#~531#~493#~521#~54#~304#~S#~84#~495#~541#~1538#~1440#~1005#~383#~520#~270#~547#~440#~86#~403#~10#~437#~7#~530#~1435#~499#~496#~T#~389#~117#~277#~1018#~1600#~534#~257#~492#~348#~345#~1021#~1542#~1015#~19#~V#~545#~307#~W#~414#~1300#~~";
				Completed = "0";
			}

			public string ID { get; set; }
			public Nullable<DateTime> xStart { get; set; }
			public Nullable<DateTime> xEnd { get; set; }
			public string CheckPermission { get; set; }
			public string CheckDamages { get; set; }
			public string CheckBoatOrder { get; set; }
			public string CheckReservations { get; set; }
			public string action { get; set; }
			public string BoatID { get; set; }
			public string guest { get; set; }
			public string route { get; set; }
			public string Description { get; set; }
			public Nullable<DateTime> StartDateTime { get; set; }
			public Nullable<DateTime> EndDateTime { get; set; }
			public string Distance { get; set; }
			public string Rower_list { get; set; }
			public string Member_list { get; set; }
			public String Completed { get; set; }

			public HttpContent build(String sessionCookie)
			{
				var formValues = new List<KeyValuePair<string, string>> {
					new KeyValuePair<string, string> ("ID", ID),
					new KeyValuePair<string, string> ("xStartDateTime", formatApiDate(xStart)),
					new KeyValuePair<string, string> ("xEndDateTime", formatApiDate(xEnd)),
					new KeyValuePair<string, string> ("CheckPermissions", CheckPermission),
					new KeyValuePair<string, string> ("CheckDamages", CheckDamages),
					new KeyValuePair<string, string> ("CheckBoatOrder", CheckBoatOrder),
					new KeyValuePair<string, string> ("CheckReservations", CheckReservations),
					new KeyValuePair<string, string> ("action", action),
					new KeyValuePair<string, string> ("BoatID", BoatID),
					new KeyValuePair<string, string> ("guest", guest),
					new KeyValuePair<string, string> ("route", route),
					new KeyValuePair<string, string> ("Description", Description),
					new KeyValuePair<string, string> ("StartDateTime", formatApiDate(StartDateTime)),
					new KeyValuePair<string, string> ("EndDateTime", formatApiDate(EndDateTime)),
					new KeyValuePair<string, string> ("Distance", Distance),
					new KeyValuePair<string, string> ("rower_list", Rower_list),
					new KeyValuePair<string, string> ("member_list", Member_list),
				};

				if (Completed != "0") {
					formValues.Add (new KeyValuePair<string, string> ("Completed", Completed));
				}

				HttpContent content = new FormUrlEncodedContent (formValues);

				content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
				content.Headers.Add ("DNT", "1");
				content.Headers.Add ("Cookie", sessionCookie);

				return content;
			}

			private String formatApiDate(Nullable<DateTime> value) 
			{
				if (value.HasValue) {
					return value.Value.ToString (apiDateFormat);
				} else {
					return "0";
				};
			}
		}

		async void stopTrip ()
		{
			String sessionCookie = await login(hc);

			String tripId = await fetchTripID (sessionCookie);

			var content = new ContentForRequest {
				ID = tripId,
				xStart = tripStartTime,
				xEnd = null,
				StartDateTime = tripStartTime,
				EndDateTime = DateTime.Now,
				Distance = "5",
				Completed = "1",
			}.build (sessionCookie);

			var response = await hc.PostAsync ("http://www.rokort.dk/workshop/row_update.php", content);

			this.btnClickMe.SetTitle("Start tur", UIControlState.Normal);
			this.btnClickMe.Enabled = true;
			Console.WriteLine ("Turen er stoppet, http status " + response.StatusCode);
		}

		async Task<String> login (HttpClient hc)
		{
			Task<HttpResponseMessage> getCookieTask = hc.GetAsync ("http://www.rokort.dk/workshop/index.php?siteid=4&guid=61BED0F9-CD54-4605-AD95-76785A6678D7");
			HttpResponseMessage cookieResponse = await getCookieTask;
			Console.WriteLine (cookieResponse.Headers.ToString ());
			var cookieHeaderValue = cookieResponse.Headers.GetValues ("Set-Cookie").First ().Split(';')[0];
			Console.WriteLine (cookieHeaderValue);
			return cookieHeaderValue;
		}

		async Task<String> fetchTripID (string sessionCookie)
		{
			HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "http://www.rokort.dk/workshop/workshop2.php");
			message.Headers.Add("Cookie", sessionCookie);

			HttpResponseMessage response = await hc.SendAsync (message);
			String responseBody = await response.Content.ReadAsStringAsync ();

			Match match = Regex.Match (responseBody, "<td onclick=\"showWin\\('row_edit.php\\?id=([0-9]*)'\\);\"><span class=\"tooltip\"><a href=\"workshop.php\\?lookup=r_" + RowerId + "\" onclick=\"javascript:return\\(false\\)\">[^<]*</a></span></td>");
			var tripId = match.Groups [1];
			Console.WriteLine ("Succes " + match.Success + "Group[1] " + tripId);
			return tripId.Value;
		}
	}
}
