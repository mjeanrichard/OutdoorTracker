// 
// Outdoor Tracker - Copyright(C) 2016 Meinard Jean-Richard
//  
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

using OutdoorTracker.Common;

using UniversalMapControl.Behaviors;

namespace OutdoorTracker.Views.Map
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
            map.ManipulationMode = ManipulationModes.Rotate | ManipulationModes.Scale | ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.TranslateInertia;
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