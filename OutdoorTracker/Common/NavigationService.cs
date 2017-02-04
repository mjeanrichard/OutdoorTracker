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

using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Microsoft.Toolkit.Uwp;

using OutdoorTracker.Views.EditTrack;
using OutdoorTracker.Views.Layers;
using OutdoorTracker.Views.LocationInfo;
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

        public async Task NavigateToSettings()
        {
            await NavigateTo<SettingsPage>();
        }

        public async Task NavigateToLayers()
        {
            await NavigateTo<LayersPage>();
        }

        public async Task NavigateToTracks()
        {
            await NavigateTo<TracksPage>();
        }

        public async Task GoBack()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => _app.RootFrame.GoBack());
        }

        public async Task NavigateToLocationInfo()
        {
            await NavigateTo<LocationInfoPage>();
        }

        public async Task NavigateToEditTrack(int trackId)
        {
            await NavigateTo<EditTrackPage>(trackId);
        }

        public async Task NavigateToNewTrack()
        {
            await NavigateTo<EditTrackPage>();
        }

        public async Task NavigateBackToMap()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IList<PageStackEntry> backStack = _app.RootFrame.BackStack;
                for (int i = backStack.Count - 1; i > 0; i--)
                {
                    _app.RootFrame.BackStack.RemoveAt(i);
                }
                _app.RootFrame.GoBack();
            });
        }

        private async Task NavigateTo<TPage>() where TPage : Page
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => _app.RootFrame.Navigate(typeof(TPage)));
        }

        private async Task NavigateTo<TPage>(object parameter) where TPage : Page
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => _app.RootFrame.Navigate(typeof(TPage), parameter));
        }
    }
}