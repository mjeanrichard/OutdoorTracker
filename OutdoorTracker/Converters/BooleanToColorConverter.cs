using System;

using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace OutdoorTracker.Converters
{
	public class BooleanToBrushConverter : IValueConverter
	{
		public Brush TrueBrush { get; set; }
		public Brush FalseBrush { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			bool? b = value as bool?;
			if (b.HasValue && b.Value)
			{
				return TrueBrush;
			}
			return FalseBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}