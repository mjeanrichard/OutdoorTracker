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

using Windows.UI.Xaml.Data;

using Microsoft.Toolkit.Uwp;

using OutdoorTracker.Helpers;

using UniversalMapControl.Projections;

namespace OutdoorTracker.Converters
{
    public class LocationToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            SwissGridLocation swissGridLocation = value as SwissGridLocation;
            if (swissGridLocation != null)
            {
                return string.Format(CultureHelper.CurrentCulture, "{0:000000} // {1:000000}", swissGridLocation.X, swissGridLocation.Y);
            }

            Wgs84Location wgs84Location = value as Wgs84Location;
            if (wgs84Location != null)
            {
                return string.Format(CultureHelper.CurrentCulture, "{0:0.0000000}, {1:0.0000000}", wgs84Location.Latitude, wgs84Location.Longitude);
            }
            return "--";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}