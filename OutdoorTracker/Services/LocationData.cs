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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;

using Microsoft.Toolkit.Uwp;

using OutdoorTracker.Views.Map;

using UniversalMapControl.Interfaces;
using UniversalMapControl.Projections;

namespace OutdoorTracker.Services
{
    public class LocationData : INotifyPropertyChanged
    {
        public LocationData()
        {
            Location = new Wgs84Location();
            State = PositionStatus.NotInitialized;
            LocationAccuracy = LocationAccuracy.None;
        }

        public double Altitude { get; private set; }
        public double AltitudeAccuracy { get; private set; }
        public double Accuracy { get; private set; }
        public ILocation Location { get; private set; }
        public PositionStatus State { get; private set; }
        public DateTime LastUpdate { get; private set; }
        public bool IsAltitudeValid { get; private set; }
        public bool IsLocationValid { get; private set; }
        public PositionSource PositionSource { get; private set; }
        public LocationAccuracy LocationAccuracy { get; private set; }
        public double? Heading { get; private set; }
        public double? Compass { get; private set; }
        public double? Speed { get; private set; }
        public bool HasCompass { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SendPropertyChangeNotifications()
        {
            OnPropertyChanged(nameof(Altitude));
            OnPropertyChanged(nameof(AltitudeAccuracy));
            OnPropertyChanged(nameof(Accuracy));
            OnPropertyChanged(nameof(HasCompass));
            OnPropertyChanged(nameof(Heading));
            OnPropertyChanged(nameof(IsAltitudeValid));
            OnPropertyChanged(nameof(IsLocationValid));
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(LocationAccuracy));
            OnPropertyChanged(nameof(LastUpdate));
            OnPropertyChanged(nameof(PositionSource));
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(Speed));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task UpdateState(PositionStatus state)
        {
            State = state;
            if (State != PositionStatus.Ready)
            {
                IsLocationValid = false;
                LocationAccuracy = LocationAccuracy.None;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => SendPropertyChangeNotifications()).ConfigureAwait(false);
        }

        public async Task UpdatePosition(Geoposition positionData)
        {
            Geocoordinate coordinate = positionData?.Coordinate;
            if (coordinate == null)
            {
                return;
            }

            BasicGeoposition pos = coordinate.Point.Position;
            Location = new Wgs84Location(pos.Latitude, pos.Longitude);
            Accuracy = coordinate.Accuracy;
            IsLocationValid = true;

            LastUpdate = coordinate.Timestamp.LocalDateTime;
            PositionSource = coordinate.PositionSource;
            Heading = !coordinate.Heading.HasValue || double.IsNaN(coordinate.Heading.Value) ? (double?)null : coordinate.Heading.Value;
            Speed = !coordinate.Speed.HasValue || double.IsNaN(coordinate.Speed.Value) ? (double?)null : coordinate.Speed.Value;

            switch (PositionSource)
            {
                case PositionSource.Satellite:
                    LocationAccuracy = LocationAccuracy.High;
                    break;
                case PositionSource.Cellular:
                case PositionSource.WiFi:
                case PositionSource.IPAddress:
                    LocationAccuracy = LocationAccuracy.Low;
                    break;
                case PositionSource.Unknown:
                default:
                    LocationAccuracy = LocationAccuracy.None;
                    break;
            }

            if (coordinate.AltitudeAccuracy.HasValue)
            {
                AltitudeAccuracy = coordinate.AltitudeAccuracy.Value;
                Altitude = pos.Altitude;
                IsAltitudeValid = true;
            }
            else
            {
                IsAltitudeValid = false;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => SendPropertyChangeNotifications());
        }

        public void UpdateCompass(CompassReading compassReading)
        {
            Compass = compassReading.HeadingMagneticNorth;
            OnPropertyChanged(nameof(Compass));
        }
    }
}