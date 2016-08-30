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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;

using OutdoorTracker.Common;
using OutdoorTracker.Layers;
using OutdoorTracker.Services;

namespace OutdoorTracker.Views.Layers
{
    public class LayersViewModel : BaseViewModel
    {
        private readonly SettingsManager _settingsManager;
        private readonly MapDefinitionManager _mapDefinitionManager;
        private MapLayerModel _selectedLayer;
        private string _busyText;

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

        public string BusyText
        {
            get { return _busyText; }
            set
            {
                _busyText = value;
                OnPropertyChanged();
            }
        }

        protected override async Task InitializeInternal()
        {
            IEnumerable<MapConfiguration> mapConfigurations = await _mapDefinitionManager.GetMapConfigurations();
            Layers = new ObservableCollection<MapLayerModel>(mapConfigurations.Select(c => new MapLayerModel(c)));
            OnPropertyChanged(nameof(Layers));
        }

        public async Task ImportMapDefinition()
        {
            BusyText = "Importing map definition...";
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
                    await InitializeInternal();
                }
            }
        }
    }
}