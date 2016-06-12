using Windows.UI.Xaml;

using OutdoorTraker.Common;

using UniversalMapControl.Behaviors;

namespace OutdoorTraker.Views.Map
{
	public class MapPageBase : AppPage<MapViewModel>
	{
	}

	public sealed partial class MapPage : MapPageBase
	{
		public MapPage()
		{
			InitializeComponent();
			map.Visibility = Visibility.Collapsed;
		}

		protected override void InitializeCompleted()
		{
			map.ViewPortProjection = ViewModel.MapConfiguration.Projection;
			tileLayer.LayerConfiguration = ViewModel.MapConfiguration.LayerConfig;
		}

		private void TouchMapBehavior_OnUpdate(object sender, TouchMapEventArgs e)
		{
			map.ViewPortCenter = e.ViewPortCenter;
			map.ZoomLevel = e.ZoomLevel;
			ViewModel.UpdateTouchInput(e);
		}
	}
}