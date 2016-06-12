using System;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;

namespace OutdoorTraker.Services
{
	public class GeoLocationService
	{
		private Geolocator _geolocator;
		private Compass _compass;
		
		public event EventHandler PositionChanged;

		public GeoLocationService()
		{
			CurrentLocation = new LocationData();
		}

		public LocationData CurrentLocation { get; }

		public bool HasCompass { get; private set; }

		public async Task Initialize()
		{
			_compass = Compass.GetDefault();
			if (_compass != null)
			{
				_compass.ReportInterval = _compass.MinimumReportInterval > 16 ? _compass.MinimumReportInterval : 16;
				_compass.ReadingChanged += CompassReadinChanged;
				HasCompass = true;
			}

			if (CurrentLocation.State == PositionStatus.Ready || CurrentLocation.State == PositionStatus.Initializing)
			{
				return;
			}

			GeolocationAccessStatus accessStatus = await Geolocator.RequestAccessAsync();
			switch (accessStatus)
			{
				case GeolocationAccessStatus.Allowed:
					_geolocator = new Geolocator();
					_geolocator.DesiredAccuracyInMeters = 1;
					_geolocator.MovementThreshold = 1;
					_geolocator.ReportInterval = 1000;
					_geolocator.StatusChanged += OnGeoStatusChanged;
					_geolocator.PositionChanged += PositionChangedHandler;
					CurrentLocation.UpdateState(PositionStatus.Initializing);
					break;

				case GeolocationAccessStatus.Denied:
				case GeolocationAccessStatus.Unspecified:
					CurrentLocation.UpdateState(PositionStatus.NotAvailable);
					break;
			}
		}

		private void CompassReadinChanged(Compass sender, CompassReadingChangedEventArgs args)
		{
			CurrentLocation.UpdateCompass(args.Reading);
		}

		private void OnGeoStatusChanged(Geolocator sender, StatusChangedEventArgs args)
		{
			CurrentLocation.UpdateState(args.Status);
		}

		private void PositionChangedHandler(Geolocator sender, PositionChangedEventArgs args)
		{
			CurrentLocation.UpdatePosition(args.Position);
			OnPositionChanged();
		}

		protected virtual void OnPositionChanged()
		{
			PositionChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}