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

using Windows.UI.Core;
using Windows.UI.Xaml;

namespace OutdoorTracker.Common
{
    public static class DispatcherHelper
    {
        public static void Init()
        {
            _uiDispatcher = Window.Current.Dispatcher;
        }

        public static void InvokeOnUI(Action action)
        {
            if (action == null)
            {
                return;
            }

            if ((_uiDispatcher == null) || _uiDispatcher.HasThreadAccess)
            {
                action();
            }
            else
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private static CoreDispatcher _uiDispatcher;
    }
}