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

using System;
using System.Threading.Tasks;

using Windows.Storage;

using OutdoorTracker.Database;
using OutdoorTracker.Services;

namespace OutdoorTracker.Migrations
{
    [DbScript("003_AddOpenStreetMap")]
    public class AddOpenStreetMap : IDbScript
    {
        private readonly MapDefinitionManager _mapDefinitionManager;

        public AddOpenStreetMap(MapDefinitionManager mapDefinitionManager)
        {
            _mapDefinitionManager = mapDefinitionManager;
        }

        public async Task Execute(OutdoorTrackerContext context)
        {
            StorageFile initialMaps = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/InitialMapLayers.json"));
            string json = await FileIO.ReadTextAsync(initialMaps);

            await _mapDefinitionManager.Import(json, context, true);
        }
    }
}