using OutdoorTraker.Views.EditTrack;
using OutdoorTraker.Views.Layers;
using OutdoorTraker.Views.LocationInfo;
using OutdoorTraker.Views.Map;
using OutdoorTraker.Views.Settings;
using OutdoorTraker.Views.Tracks;

namespace OutdoorTraker.Common
{
	public class NavigationService
	{
		private readonly App _app;

		public NavigationService(App app)
		{
			_app = app;
		}

		public void NavigateToSettings()
		{
			_app.RootFrame.Navigate(typeof(SettingsPage));
		}

		public void NavigateToLayers()
		{
			_app.RootFrame.Navigate(typeof(LayersPage));
		}

		public void NavigateToTracks()
		{
			_app.RootFrame.Navigate(typeof(TracksPage));
		}

		public void GoBack()
		{
			_app.RootFrame.GoBack();
		}

		public void NavigateToLocationInfo()
		{
			_app.RootFrame.Navigate(typeof(LocationInfoPage));
		}

		public void NavigateToEditTrack(int trackId)
		{
			_app.RootFrame.Navigate(typeof(EditTrackPage), trackId);
		}

		public void NavigateToNewTrack()
		{
			_app.RootFrame.Navigate(typeof(EditTrackPage));
		}

		public void NavigateToMap()
		{
			_app.RootFrame.Navigate(typeof(MapPage));
		}
	}
}