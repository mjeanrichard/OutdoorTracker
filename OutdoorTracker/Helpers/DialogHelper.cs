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