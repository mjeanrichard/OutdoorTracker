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
using System.Globalization;

using Windows.Foundation.Collections;
using Windows.Storage;

using UniversalMapControl.Interfaces;
using UniversalMapControl.Projections;

namespace OutdoorTracker.Services
{
    public enum HeadingMode
    {
        NorthUp,
        Manual,
        Compass
    }

    public class SettingsManager
    {
        private const string LastMapCenterSettingName = "LastMapCenter";
        private const string ShowLocationSettingName = "ShowLocation";
        private const string ShowAccuracySettingName = "ShowAccuracy";
        private const string MapLayerNameSettingName = "MapLayerName";
        private const string ZoomFactorSettingName = "ZoomFactor";
        private const string RotationEnabledSettingName = "RotationEnabled";
        private const string CenterOnPositionSettingName = "CenterOnPosition";
        private const string CurrentTrackingIdSettingName = "CurrentTrackingId";
        private const string HeadingModeSettingName = "HeadingMode";
        private const string ShowCompassSettingName = "ShowCompass";
        private const string LastHeadingSettingName = "LastHeading";

        private IPropertySet Values
        {
            get { return ApplicationData.Current.LocalSettings.Values; }
        }

        public bool CenterOnPosition
        {
            get { return GetValue<bool?>(CenterOnPositionSettingName) ?? true; }
            set { Values[CenterOnPositionSettingName] = value; }
        }

        public bool ShowLocation
        {
            get { return GetValue<bool?>(ShowLocationSettingName) ?? true; }
            set { Values[ShowLocationSettingName] = value; }
        }

        public bool ShowAccuracy
        {
            get { return GetValue<bool?>(ShowAccuracySettingName) ?? true; }
            set { Values[ShowAccuracySettingName] = value; }
        }

        public HeadingMode HeadingMode
        {
            get
            {
                string stringValue = GetValue<string>(HeadingModeSettingName);
                HeadingMode value;
                if (!Enum.TryParse(stringValue, true, out value))
                {
                    return HeadingMode.NorthUp;
                }
                return value;
            }
            set { Values[HeadingModeSettingName] = value.ToString("G"); }
        }

        public string MapLayerName
        {
            get { return GetValue<string>(MapLayerNameSettingName); }
            set { Values[MapLayerNameSettingName] = value; }
        }

        public double LastHeading
        {
            get { return GetValue<double?>(LastHeadingSettingName) ?? 0; }
            private set { Values[LastHeadingSettingName] = value; }
        }

        public double ZoomFactor
        {
            get { return GetValue<double?>(ZoomFactorSettingName) ?? 5; }
            private set { Values[ZoomFactorSettingName] = value; }
        }

        public bool RotationEnabled
        {
            get { return GetValue<bool?>(RotationEnabledSettingName) ?? false; }
            set { Values[RotationEnabledSettingName] = value; }
        }

        public int? CurrentTrackingId
        {
            get { return GetValue<int?>(CurrentTrackingIdSettingName); }
            set { Values[CurrentTrackingIdSettingName] = value; }
        }

        public ILocation GetLastMapCenter()
        {
            string locationString = GetValue<string>(LastMapCenterSettingName);
            if (!string.IsNullOrWhiteSpace(locationString))
            {
                string[] locationParts = locationString.Split(',');
                if (locationParts.Length == 2)
                {
                    double latitude;
                    if (double.TryParse(locationParts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out latitude))
                    {
                        double longitude;
                        if (double.TryParse(locationParts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out longitude))
                        {
                            return new Wgs84Location(latitude, longitude);
                        }
                    }
                }
            }
            return new SwissGridLocation(600000, 200000);
        }

        public void SetLastPosition(ILocation location, double zoomFactor, double heading)
        {
            if (location == null)
            {
                return;
            }
            Values[LastMapCenterSettingName] = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", location.Latitude, location.Longitude);
            ZoomFactor = zoomFactor;
            LastHeading = heading;
        }

        private TValue GetValue<TValue>(string name)
        {
            try
            {
                object value = Values[name];
                return (TValue)value;
            }
            catch (Exception)
            {
                // TODO: LOG ERROR!
                return default(TValue);
            }
        }
    }
}