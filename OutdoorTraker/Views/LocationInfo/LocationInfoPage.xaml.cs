using OutdoorTraker.Common;

namespace OutdoorTraker.Views.LocationInfo
{
	public class LocationInfoPageBase : AppPage<LocationInfoViewModel>
	{
	}

	public sealed partial class LocationInfoPage : LocationInfoPageBase
	{
		public LocationInfoPage()
		{
			InitializeComponent();
		}
	}
}