using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media;

using Newtonsoft.Json;

namespace OutdoorTraker.Layers
{
	public class LayerDefinition
	{
		public LayerDefinition()
		{
			Parameters = new Dictionary<string, string>();
		}

		[JsonProperty(Required = Required.Always)]
		public string Name { get; set; }

		[JsonProperty(Required = Required.Always)]
		public string Config { get; set; }

		public Dictionary<string, string> Parameters { get; set; }
	}
}
