using System;
using System.Globalization;

using Windows.UI.Xaml.Data;

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
				return string.Format(CultureInfo.CurrentCulture, "{0:000000} // {1:000000}", swissGridLocation.X, swissGridLocation.Y);
			}

			Wgs84Location wgs84Location = value as Wgs84Location;
			if (wgs84Location != null)
			{
				return string.Format(CultureInfo.CurrentCulture, "{0:0.0000000}, {1:0.0000000}", wgs84Location.Latitude, wgs84Location.Longitude);
			}
			return "--";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}