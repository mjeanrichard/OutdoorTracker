using Windows.UI.Xaml.Input;

using OutdoorTracker.Common;
using OutdoorTracker.Views.Map;

namespace OutdoorTracker.Views.Settings
{
	public class SettingsPageBase : AppPage<SettingsViewModel>
	{
	}

	public sealed partial class SettingsPage : SettingsPageBase
	{
		public SettingsPage()
		{
			InitializeComponent();
		}
	}
}