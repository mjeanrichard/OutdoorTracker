using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;

using OutdoorTraker.Common;
using OutdoorTraker.Services;

namespace OutdoorTraker.Views.LocationInfo
{
	public class LocationInfoViewModel : BaseViewModel
	{
		private readonly GeoLocationService _geoLocationService;
		private DispatcherTimer _timer;

		public LocationInfoViewModel()
		{
		}

		public LocationInfoViewModel(GeoLocationService geoLocationService)
			: this()
		{
			_geoLocationService = geoLocationService;
			Location = _geoLocationService.CurrentLocation;

			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromSeconds(1);
			_timer.Tick += OnTimerTick;
		}

		public LocationData Location { get; set; }

		private void OnTimerTick(object sender, object e)
		{
		}

		protected override async Task InitializeInternal()
		{
			await _geoLocationService.Initialize();
			_timer.Start();
		}
	}
}