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

using System.Globalization;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.Globalization;

using OutdoorTracker.Common;
using OutdoorTracker.Helpers;
using OutdoorTracker.Services;

namespace OutdoorTracker.Views.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly SettingsManager _settingsManager;

        public SettingsViewModel()
        {
            Package package = Package.Current;
            PackageVersion packageVersion = package.Id.Version;
            Version = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }

        public SettingsViewModel(SettingsManager settingsManager)
            : this()
        {
            _settingsManager = settingsManager;
        }

        public string Version { get; set; }

        public bool ShowLocation
        {
            get { return _settingsManager.ShowLocation; }
            set { _settingsManager.ShowLocation = value; }
        }

        public bool EnableTrackSmoothing
        {
            get { return _settingsManager.EnableTrackSmoothing; }
            set
            {
                _settingsManager.EnableTrackSmoothing = value;
                OnPropertyChanged();
            }
        }

        public bool UseHighAccuracyOnly
        {
            get { return _settingsManager.UseHighAccuracyOnly; }
            set
            {
                _settingsManager.UseHighAccuracyOnly = value;
                OnPropertyChanged();
            }
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


        public string TrackMinDistanceMeters
        {
            get { return _settingsManager.TrackMinDistanceMeters.ToString(); }
            set
            {
                int intValue;
                if (int.TryParse(value, NumberStyles.AllowTrailingWhite | NumberStyles.AllowTrailingWhite, CultureHelper.CurrentCulture, out intValue))
                {
                    _settingsManager.TrackMinDistanceMeters = intValue;
                }
                OnPropertyChanged();
            }
        }

        public string SelectedLanguage
        {
            get { return ApplicationLanguages.PrimaryLanguageOverride; }
            set
            {
                if (ApplicationLanguages.PrimaryLanguageOverride != value)
                {
                    ApplicationLanguages.PrimaryLanguageOverride = value;
                    CultureHelper.Reload();
                    OnPropertyChanged();
                }
            }
        }

        protected override Task InitializeInternalAsync()
        {
            return Task.CompletedTask;
        }
    }
}