﻿// 
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

using OutdoorTracker.Common;
using OutdoorTracker.Helpers;
using OutdoorTracker.Logging;

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

        public event EventHandler PositionChanged;

        public async Task Initialize()
        {
            if (!CurrentLocation.HasCompass)
            {
                _compass = Compass.GetDefault();
                if (_compass != null)
                {
                    try
                    {
                        _compass.ReportInterval = _compass.MinimumReportInterval > 16 ? _compass.MinimumReportInterval : 16;
                        _compass.ReadingChanged += CompassReadingChanged;
                        CurrentLocation.HasCompass = true;
                        OutdoorTrackerEvents.Log.CompassFound();
                    }
                    catch (Exception ex)
                    {
                        ErrorReporter.Current.TrackException(ex, new Dictionary<string, string> { { "Event", "CompassDisabled" } });
                        OutdoorTrackerEvents.Log.CompassAccessException(ex);
                        CurrentLocation.HasCompass = false;
                    }
                }
                else
                {
                    ErrorReporter.Current.TrackEvent(TrackEvents.NoCompass);
                    OutdoorTrackerEvents.Log.CompassNotFound();
                }
            }

            if ((CurrentLocation.State == PositionStatus.Ready) || (CurrentLocation.State == PositionStatus.Initializing))
            {
                return;
            }

            OutdoorTrackerEvents.Log.LocationInitializing();
            GeolocationAccessStatus accessStatus = GeolocationAccessStatus.Unspecified;
            await DispatcherHelper.InvokeOnUiAsync(async () =>
            {
                accessStatus = await Geolocator.RequestAccessAsync();
            });

            OutdoorTrackerEvents.Log.LocationAccessState(accessStatus.ToString("G"));
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    _geolocator = new Geolocator();
                    _geolocator.DesiredAccuracyInMeters = 1;
                    _geolocator.MovementThreshold = 1;
                    _geolocator.ReportInterval = 1000;
                    _geolocator.StatusChanged += OnGeoStatusChanged;
                    _geolocator.PositionChanged += PositionChangedHandler;
                    await CurrentLocation.UpdateState(PositionStatus.Initializing).ConfigureAwait(false);
                    break;

                case GeolocationAccessStatus.Denied:
                    ErrorReporter.Current.TrackEvent(TrackEvents.GeoLocationDenied);
                    await CurrentLocation.UpdateState(PositionStatus.NotAvailable).ConfigureAwait(false);
                    break;
                case GeolocationAccessStatus.Unspecified:
                    ErrorReporter.Current.TrackEvent(TrackEvents.LocationStateUnspecified);
                    await CurrentLocation.UpdateState(PositionStatus.NotAvailable).ConfigureAwait(false);
                    break;
            }
        }

        private void CompassReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            CurrentLocation.UpdateCompass(args.Reading);
        }

        private async void OnGeoStatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            await CurrentLocation.UpdateState(args.Status).ConfigureAwait(false);
        }

        private async void PositionChangedHandler(Geolocator sender, PositionChangedEventArgs args)
        {
            await CurrentLocation.UpdatePosition(args.Position).ConfigureAwait(false);
            OnPositionChanged();
        }

        protected virtual void OnPositionChanged()
        {
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}