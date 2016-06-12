using Newtonsoft.Json;

using UniversalMapControl.Interfaces;
using UniversalMapControl.Tiles;

namespace OutdoorTraker.Layers
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