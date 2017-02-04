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

using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using OutdoorTracker.Common;

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

        protected override void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (tracksView.SelectionMode == ListViewSelectionMode.Multiple)
            {
                tracksView.SelectionMode = ListViewSelectionMode.Single;
                e.Handled = true;
            }
            base.BackRequested(sender, e);
        }

        private void TracksView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedTracks = tracksView.SelectedItems.Cast<TrackViewModel>().ToList();
        }

        private void DeleteTapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.DeleteTrackCommand.Execute();
        }

        private void SwitchMultiSelect(object sender, TappedRoutedEventArgs e)
        {
            if (tracksView.SelectionMode == ListViewSelectionMode.Single)
            {
                tracksView.SelectionMode = ListViewSelectionMode.Multiple;
            }
            else
            {
                tracksView.SelectionMode = ListViewSelectionMode.Single;
            }
        }
    }
}