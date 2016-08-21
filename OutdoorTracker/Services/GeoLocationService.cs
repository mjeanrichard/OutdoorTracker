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

        public GeoLocationService()
        {
            CurrentLocation = new LocationData();
        }

        public LocationData CurrentLocation { get; }

        public bool HasCompass { get; private set; }

        public event EventHandler PositionChanged;

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

            if ((CurrentLocation.State == PositionStatus.Ready) || (CurrentLocation.State == PositionStatus.Initializing))
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