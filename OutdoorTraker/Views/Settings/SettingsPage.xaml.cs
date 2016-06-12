using Windows.UI.Xaml.Input;

using OutdoorTraker.Common;
using OutdoorTraker.Views.Map;

namespace OutdoorTraker.Views.Settings
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