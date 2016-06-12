using OutdoorTraker.Common;

namespace OutdoorTraker.Views.EditTrack
{
	public class EditTrackPageBase : AppPage<EditTrackViewModel>
	{
	}

	public sealed partial class EditTrackPage : EditTrackPageBase
	{
		public EditTrackPage()
		{
			InitializeComponent();
		}
	}
}