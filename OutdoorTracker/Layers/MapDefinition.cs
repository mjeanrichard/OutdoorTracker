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

using Newtonsoft.Json;

using UniversalMapControl.Interfaces;
using UniversalMapControl.Tiles;

namespace OutdoorTracker.Layers
{
    public class MapDefinition
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Projection { get; set; }

        [JsonProperty(Required = Required.Always)]
        public LayerDefinition[] Layers { get; set; }
    }

    public class MapConfiguration
    {
        public MapConfiguration()
        {
            Version = 1;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Json { get; set; }
        public int Version { get; set; }

        public IProjection Projection { get; set; }
        public ILayerConfiguration LayerConfig { get; set; }
    }
}