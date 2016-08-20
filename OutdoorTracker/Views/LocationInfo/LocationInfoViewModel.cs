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

using System.ComponentModel;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;

using OutdoorTracker.Common;
using OutdoorTracker.Services;

namespace OutdoorTracker.Views.LocationInfo
{
	public class LocationInfoViewModel : BaseViewModel
	{
		private readonly GeoLocationService _geoLocationService;

		public LocationInfoViewModel()
		{
		}

		public LocationInfoViewModel(GeoLocationService geoLocationService)
			: this()
		{
			_geoLocationService = geoLocationService;
			Location = _geoLocationService.CurrentLocation;

			Location.PropertyChanged += LocationOnPropertyChanged;
		}

		public bool ShowLocationSettingsInfo
		{
			get { return _geoLocationService.CurrentLocation.State == PositionStatus.NotAvailable; }
		}

		public LocationData Location { get; set; }

		private void LocationOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			OnPropertyChanged(nameof(ShowLocationSettingsInfo));
		}

		protected override async Task InitializeInternal()
		{
			await _geoLocationService.Initialize();
		}
	}
}