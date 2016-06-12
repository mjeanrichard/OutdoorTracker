using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OutdoorTraker.Converters
{
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			bool isInverted;
			if (parameter == null || !bool.TryParse(parameter.ToString(), out isInverted))
			{
				isInverted = false;
			}
			bool b = (value as bool?).GetValueOrDefault(false);

			if (isInverted)
			{
				b = !b;
			}

			return b ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			Visibility? v = value as Visibility?;
			return v == null ? (object)null : v.Value == Visibility.Visible;
		}
	}
}