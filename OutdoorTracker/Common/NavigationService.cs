// 
// Outdoor Tracker - Copyright(C) 2016 Meinard Jean-Richard
//  
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using OutdoorTracker.Views.EditTrack;
using OutdoorTracker.Views.Layers;
using OutdoorTracker.Views.LocationInfo;
using OutdoorTracker.Views.Map;
using OutdoorTracker.Views.Settings;
using OutdoorTracker.Views.Tracks;

namespace OutdoorTracker.Common
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