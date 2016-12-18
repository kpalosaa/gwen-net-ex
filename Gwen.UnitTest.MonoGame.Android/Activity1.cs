using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Gwen.UnitTest.MonoGame.Android
{
	[Activity(Label = "Gwen.UnitTest.MonoGame.Android"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = global::Android.Content.PM.LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.SensorLandscape
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
	public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			var g = new UnitTestGame();
			SetContentView((View)g.Services.GetService(typeof(View)));
			g.Run();
		}
	}
}

