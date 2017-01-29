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

using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

using OutdoorTracker.Common;

namespace OutdoorTracker.Views.EditTrack
{
    public class EditTrackPageBase : AppPage<EditTrackViewModel>
    {
    }

    public sealed partial class EditTrackPage : EditTrackPageBase
    {
        public EditTrackPage()
        {
            InitializeComponent();
        }

        private void ColorSelectorTapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void ColorSelected(object sender, EventArgs e)
        {
            FlyoutBase flyout = FlyoutBase.GetAttachedFlyout(ColorButton);
            flyout?.Hide();
        }
    }
}