using OutdoorTracker.Common;

namespace OutdoorTracker.Views.LocationInfo
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