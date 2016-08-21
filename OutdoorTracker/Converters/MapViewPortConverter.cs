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

using Windows.UI.Xaml.Data;

using UniversalMapControl;

namespace OutdoorTracker.Converters
{
    public class MapViewPortConverter : IValueConverter
    {
        public Map Map { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int)
            {
                double num = Map.ViewPortProjection.GetZoomFactor(Map.ZoomLevel);
                return (int)value * num;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}