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
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.UI.Popups;

using Microsoft.HockeyApp;

namespace OutdoorTracker.Helpers
{
    public static class DialogHelper
    {
        public static async Task ShowError(string errorMessage, string title)
        {
            var dialog = new MessageDialog(errorMessage, title);

            dialog.Commands.Add(new UICommand("Ok") { Id = 0 });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;
            await dialog.ShowAsync();
        }

        public static async Task ShowMessage(string message, string title)
        {
            var dialog = new MessageDialog(message, title);

            dialog.Commands.Add(new UICommand("Ok") { Id = 0 });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;
            await dialog.ShowAsync();
        }

        public static async Task ShowErrorAndReport(string errorMessage, string title, Exception ex, Dictionary<string, string> properties = null)
        {
            var dialog = new MessageDialog(errorMessage + Environment.NewLine + "Would you like to report this Error?", title);

            dialog.Commands.Add(new UICommand("No") { Id = 0 });
            dialog.Commands.Add(new UICommand("Yes, send report") { Id = 1 });

            dialog.DefaultCommandIndex = 1;
            dialog.CancelCommandIndex = 0;

            IUICommand result = await dialog.ShowAsync();
            if ((int)result.Id == 1)
            {
                HockeyClient.Current.TrackException(ex, properties);
            }
        }
    }
}