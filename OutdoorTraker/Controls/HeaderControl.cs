using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace OutdoorTraker.Controls
{
	public sealed class HeaderControl : ContentControl
	{
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
			"Header", typeof(object), typeof(HeaderControl), new PropertyMetadata(default(object)));

		public object Header
		{
			get { return (object)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}
		public HeaderControl()
		{
			DefaultStyleKey = typeof(HeaderControl);
		}
	}
}