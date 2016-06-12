using System.Threading.Tasks;

using OutdoorTraker.Common;
using OutdoorTraker.Services;

namespace OutdoorTraker.Views.Settings
{
	public class SettingsViewModel : BaseViewModel
	{
		private readonly SettingsManager _settingsManager;

		public SettingsViewModel()
		{
		}

		public SettingsViewModel(SettingsManager settingsManager)
			: this()
		{
			_settingsManager = settingsManager;
		}

		public bool ShowLocation
		{
			get { return _settingsManager.ShowLocation; }
			set { _settingsManager.ShowLocation = value; }
		}

		public bool ShowAccuracy
		{
			get { return _settingsManager.ShowAccuracy; }
			set { _settingsManager.ShowAccuracy = value; }
		}

		public bool RotationEnabled
		{
			get { return _settingsManager.RotationEnabled; }
			set { _settingsManager.RotationEnabled = value; }
		}

		protected override Task InitializeInternal()
		{
			return Task.CompletedTask;
		}
	}
}