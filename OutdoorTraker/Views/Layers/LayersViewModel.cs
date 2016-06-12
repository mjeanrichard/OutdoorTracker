using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;

using OutdoorTraker.Common;
using OutdoorTraker.Layers;
using OutdoorTraker.Services;

namespace OutdoorTraker.Views.Layers
{
	public class LayersViewModel : BaseViewModel
	{
		private readonly SettingsManager _settingsManager;
		private readonly MapDefinitionManager _mapDefinitionManager;
		private MapLayerModel _selectedLayer;

		public LayersViewModel()
		{
			Layers = new ObservableCollection<MapLayerModel>();
			AddMapCommand = new RelayCommand(async () => await ImportMapDefinition());
		}

		public LayersViewModel(SettingsManager settingsManager, MapDefinitionManager mapDefinitionManager)
			: this()
		{
			_settingsManager = settingsManager;
			_mapDefinitionManager = mapDefinitionManager;
		}

		public ObservableCollection<MapLayerModel> Layers { get; private set; }

		public MapLayerModel SelectedLayer
		{
			get { return _selectedLayer; }
			set
			{
				_selectedLayer = value;
				_settingsManager.MapLayerName = value.Name;
			}
		}

		public RelayCommand AddMapCommand { get; }

		protected override async Task InitializeInternal()
		{
			IEnumerable<MapConfiguration> mapConfigurations = await _mapDefinitionManager.GetMapConfigurations();
			Layers = new ObservableCollection<MapLayerModel>(mapConfigurations.Select(c => new MapLayerModel(c)));
		}

		public async Task ImportMapDefinition()
		{
			using (MarkBusy())
			{
				FileOpenPicker openPicker = new FileOpenPicker();
				openPicker.ViewMode = PickerViewMode.List;
				openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
				openPicker.FileTypeFilter.Add(".json");
				StorageFile file = await openPicker.PickSingleFileAsync();
				if (file != null)
				{
					string json = await FileIO.ReadTextAsync(file);
					await _mapDefinitionManager.Import(json);
				}
			}
		}
	}

	public class MapLayerModel
	{
		private readonly MapConfiguration _mapConfiguration;

		public MapLayerModel(MapConfiguration mapConfiguration)
		{
			_mapConfiguration = mapConfiguration;
		}

		public string Name
		{
			get { return _mapConfiguration.Name; }
		}
	}
}