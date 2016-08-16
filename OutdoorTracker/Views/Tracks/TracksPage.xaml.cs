using System.Collections.ObjectModel;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

using OutdoorTracker.Common;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Views.Tracks
{
	public class TracksPageBase : AppPage<TracksViewModel>
	{
	}

	public sealed partial class TracksPage : TracksPageBase
	{
		public TracksPage()
		{
			InitializeComponent();
		}

		private void TracksView_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
		}

		private void TracksView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ViewModel.SelectedTracks = tracksView.SelectedItems.Cast<Track>().ToList();
		}
	}
}