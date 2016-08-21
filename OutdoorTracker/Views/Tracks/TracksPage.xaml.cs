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

using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

using OutdoorTracker.Common;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Views.Tracks
{
    public class TracksPageBase : AppPage<TracksViewModel>
    {
    }

    public sealed partial class TracksPage : TracksPageBase
    {
        public TracksPage()
        {
            InitializeComponent();
        }

        private void TracksView_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void TracksView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedTracks = tracksView.SelectedItems.Cast<Track>().ToList();
        }
    }
}