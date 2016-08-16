using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;

using Microsoft.HockeyApp;

namespace OutdoorTracker.Services
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
			if (!HasCompass)
			{
				_compass = Compass.GetDefault();
				if (_compass != null)
				{
					try
					{
						_compass.ReportInterval = _compass.MinimumReportInterval > 16 ? _compass.MinimumReportInterval : 16;
						_compass.ReadingChanged += CompassReadinChanged;
						HasCompass = true;
					}
					catch (Exception ex)
					{
						HockeyClient.Current.TrackException(ex, new Dictionary<string, string> { { "Event", "CompassDisabled" } });
						HasCompass = false;
					}
				}
				else
				{
					HockeyClient.Current.TrackEvent("No Compass");
				}
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
					HockeyClient.Current.TrackEvent("Geo Location Denied.");
					CurrentLocation.UpdateState(PositionStatus.NotAvailable);
					break;
				case GeolocationAccessStatus.Unspecified:
					HockeyClient.Current.TrackEvent("Geo Location state unspecified.");
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