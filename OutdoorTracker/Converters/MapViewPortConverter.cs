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
				return ((int)value) * num;
			}
			return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}