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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using Windows.UI.Popups;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Helpers;
using OutdoorTracker.Layers;
using OutdoorTracker.Logging;
using OutdoorTracker.Resources;

using UniversalMapControl.Interfaces;
using UniversalMapControl.Projections;
using UniversalMapControl.Tiles;
using UniversalMapControl.Tiles.Default;
using UniversalMapControl.Tiles.SwissTopo;

namespace OutdoorTracker.Services
{
    public class MapDefinitionManager
    {
        private static readonly MapConfiguration Default = new MapConfiguration { LayerConfig = new DefaultWebLayerConfig { LayerName = "OSM" }, Projection = new Wgs84WebMercatorProjection(), Name = "Default Open Street Map" };
        private readonly IReadonlyUnitOfWork _readonlyUnitOfWork;
        private readonly UnitOfWorkFactoy _unitOfWorkFactory;
        private readonly SettingsManager _settingsManager;
        private MapConfiguration _currentConfiguration;

        public MapDefinitionManager(IReadonlyUnitOfWork readonlyUnitOfWork, UnitOfWorkFactoy unitOfWorkFactory, SettingsManager settingsManager)
        {
            _readonlyUnitOfWork = readonlyUnitOfWork;
            _unitOfWorkFactory = unitOfWorkFactory;
            _settingsManager = settingsManager;
        }

        public async Task<IEnumerable<MapConfiguration>> GetMapConfigurations()
        {
            try
            {
                using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
                {
                    return await unitOfWork.MapConfigurations.ToListAsync();
                }
            }
            catch (SqliteException sqlEx)
            {
                await DialogHelper.CheckDatabaseException(sqlEx);
                return Enumerable.Empty<MapConfiguration>();
            }
        }

        public async Task<MapConfiguration> GetCurrentConfiguration()
        {
            string mapLayerName = "";
            try
            {
                mapLayerName = _settingsManager.MapLayerName;
                if ((_currentConfiguration == null) || !_currentConfiguration.Name.Equals(mapLayerName))
                {
                    MapConfiguration mapConfiguration = await _readonlyUnitOfWork.MapConfigurations.FirstOrDefaultAsync(c => c.Name.Equals(mapLayerName, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
                    if (mapConfiguration == null)
                    {
                        return Default;
                    }
                    Deserialize(mapConfiguration);
                    _currentConfiguration = mapConfiguration;
                }
                return _currentConfiguration;
            }
            catch (SqliteException sqlEx)
            {
                await DialogHelper.CheckDatabaseException(sqlEx);
                return Default;
            }
            catch (Exception ex)
            {
                OutdoorTrackerEvents.Log.MapDefinitionGetCurrentFailed(mapLayerName, ex);
                ErrorReporter.Current.TrackException(ex, new Dictionary<string, string> { { "LayerName", mapLayerName } });
                return Default;
            }
        }

        private void Deserialize(MapConfiguration mapConfiguration)
        {
            MapDefinition mapDef = JsonConvert.DeserializeObject<MapDefinition>(mapConfiguration.Json);
            mapConfiguration.Projection = GetProjection(mapDef);
            mapConfiguration.LayerConfig = GetLayerConfig(mapDef.Layers[0]);
        }

        private IProjection GetProjection(MapDefinition mapDefinition)
        {
            switch (mapDefinition.Projection)
            {
                case "WebMercator":
                    return new Wgs84WebMercatorProjection();
                case "SwissGrid":
                    return new SwissGridProjection();
                default:
                    throw new ArgumentException(Messages.MapDefinitionManager.UnknownProjection(mapDefinition.Projection));
            }
        }

        private ILayerConfiguration GetLayerConfig(LayerDefinition layerDefinition)
        {
            ILayerConfiguration config = null;
            string value;
            switch (layerDefinition.Config)
            {
                case "Default":
                    string urlPattern = "http://{RND-a;b;c}.tile.openstreetmap.org/{z}/{x}/{y}.png";
                    if (layerDefinition.Parameters.TryGetValue("urlPattern", out value))
                    {
                        urlPattern = value;
                    }
                    config = new DefaultWebLayerConfig(layerDefinition.Name, urlPattern);
                    break;
                case "SwissTopo":
                    string licenseKey = "";
                    if (layerDefinition.Parameters.TryGetValue("lisenceKey", out value))
                    {
                        licenseKey = value;
                    }
                    config = new SwissTopoLayerConfig (layerDefinition.Name, licenseKey);
                    break;
                default:
                    throw new ArgumentException(Messages.MapDefinitionManager.UnknownConfig(layerDefinition.Config));
            }

            config.TileLoader.MaxParallelTasks = 5;
            config.TileProvider.LowerTileSetsToLoad = 2;
            return config;
        }

        public async Task Import(string json, bool forceOverwrite = false)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    await Import(json, unitOfWork, forceOverwrite);
                    await unitOfWork.SaveChangesAsync();
                }
                catch (JsonReaderException ex)
                {
                    OutdoorTrackerEvents.Log.MapDefinitionImportFailedInvalidJson(json, ex);
                    await DialogHelper.ShowError(Messages.MapDefinitionManager.ImportInvalidJson, Messages.MapDefinitionManager.ImportInvalidJsonTitle);
                }
                catch (Exception ex)
                {
                    OutdoorTrackerEvents.Log.MapDefinitionImportFailed(json, ex);
                    await DialogHelper.ShowErrorAndReport(Messages.MapDefinitionManager.ImportError, Messages.MapDefinitionManager.ImportErrorTitle, ex, new Dictionary<string, string> { { "Json", json } });
                }
            }
        }

        public async Task Import(string json, IUnitOfWork unitOfWork, bool forceOverwrite = false)
        {
            MapDefinition[] maps = JsonConvert.DeserializeObject<MapDefinition[]>(json);

            List<MapConfiguration> allConfigs = await unitOfWork.MapConfigurations.ToListAsync();

            foreach (MapDefinition map in maps)
            {
                MapConfiguration configuration = allConfigs.FirstOrDefault(c => c.Name.Equals(map.Name, StringComparison.OrdinalIgnoreCase));
                if (configuration != null)
                {
                    if (!forceOverwrite && !await AllowOverwrite(map.Name))
                    {
                        continue;
                    }
                }
                else
                {
                    configuration = new MapConfiguration();
                    unitOfWork.MapConfigurations.Add(configuration);
                }
                configuration.Name = map.Name;
                configuration.Json = JsonConvert.SerializeObject(map);
            }
        }

        private async Task<bool> AllowOverwrite(string name)
        {
            var dialog = new MessageDialog(Messages.MapDefinitionManager.LayerExists(name));

            dialog.Commands.Add(new UICommand(Messages.MapDefinitionManager.Overwrite) { Id = true });
            dialog.Commands.Add(new UICommand(Messages.MapDefinitionManager.Skip) { Id = false });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();
            return (bool)result.Id;
        }
    }
}