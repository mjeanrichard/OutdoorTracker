using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

using OutdoorTraker.Common;
using OutdoorTraker.Services;

namespace OutdoorTraker.Views.LocationInfo
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

		private void LocationOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			OnPropertyChanged(nameof(ShowLocationSettingsInfo));
		}

		public bool ShowLocationSettingsInfo
		{
			get { return _geoLocationService.CurrentLocation.State == PositionStatus.NotAvailable; }
		}

		public LocationData Location { get; set; }

		protected override async Task InitializeInternal()
		{
			await _geoLocationService.Initialize();
		}
	}
}