using System;
using System.Globalization;

using Windows.UI.Xaml.Data;

namespace OutdoorTracker.Converters
{
	public class StringFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return string.Format(CultureInfo.CurrentCulture, (string)parameter, value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}