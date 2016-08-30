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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OutdoorTracker.Controls
{
    public sealed class PageViewControl : ContentControl
    {
        public static readonly DependencyProperty BusyMessageProperty = DependencyProperty.Register(
            nameof(BusyMessage), typeof(string), typeof(PageViewControl), new PropertyMetadata(default(string)));

        public string BusyMessage
        {
            get { return (string)GetValue(BusyMessageProperty); }
            set { SetValue(BusyMessageProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            nameof(IsBusy), typeof(bool), typeof(PageViewControl), new PropertyMetadata(default(bool)));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public PageViewControl()
        {
            DefaultStyleKey = typeof(PageViewControl);
        }
    }
}