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

using System.Threading.Tasks;

using OutdoorTracker.Common;
using OutdoorTracker.Services;

namespace OutdoorTracker.Views.Settings
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