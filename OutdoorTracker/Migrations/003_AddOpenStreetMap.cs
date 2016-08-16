
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