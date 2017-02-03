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
using Windows.UI.Popups;

using OutdoorTracker.Common;
using OutdoorTracker.Helpers;
using OutdoorTracker.Layers;
using OutdoorTracker.Logging;
using OutdoorTracker.Resources;
using OutdoorTracker.Services;

namespace OutdoorTracker.Views.Layers
{
    public class LayersViewModel : BaseViewModel
    {
        private readonly SettingsManager _settingsManager;
        private readonly MapDefinitionManager _mapDefinitionManager;
        private MapLayerModel _selectedLayer;

        public LayersViewModel()
        {
            Layers = new ObservableCollection<MapLayerModel>();
            AddMapCommand = new AsyncCommand(ImportMapDefinitionAsync, Messages.LayersViewModel.ImportingMessage, this);
            ClearAllCacheCommand = new AsyncCommand(ClearCacheAsync, this);
            LoadLayerSizeCommand = new AsyncCommand(LoadLayerSizesAsync, this);
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
        public RelayCommand ClearAllCacheCommand { get; }
        public RelayCommand LoadLayerSizeCommand { get; }

        private async Task LoadLayerSizesAsync()
        {
            LoadLayerSizeCommand.EnabledOverride = false;
            List<Task> loadTasks = new List<Task>();
            foreach (MapLayerModel layerModel in Layers)
            {
                loadTasks.Add(layerModel.LoadSizeAsync());
            }
            await Task.WhenAll(loadTasks).ConfigureAwait(false);
            LoadLayerSizeCommand.EnabledOverride = null;
        }

        private async Task ClearCacheAsync()
        {
            if (await AskDeleteCacheAsync())
            {
                IStorageFolder folder = await ApplicationData.Current.LocalCacheFolder.TryGetItemAsync("UMCCache") as IStorageFolder;
                if (folder != null)
                {
                    await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
        }

        private async Task<bool> AskDeleteCacheAsync()
        {
            MessageDialog dialog = new MessageDialog(Messages.LayersViewModel.DeleteCacheMessage, Messages.LayersViewModel.DeleteCacheMessageTitle);

            dialog.Commands.Add(new UICommand(Messages.Dialog.No) { Id = 0 });
            dialog.Commands.Add(new UICommand(Messages.LayersViewModel.YesDelete) { Id = 1 });

            dialog.DefaultCommandIndex = 1;
            dialog.CancelCommandIndex = 0;

            IUICommand result = await dialog.ShowAsync();
            return result.Id.Equals(0);
        }

        protected override async Task InitializeInternalAsync()
        {
            IEnumerable<MapConfiguration> mapConfigurations = await _mapDefinitionManager.GetMapConfigurations().ConfigureAwait(false);
            Layers = new ObservableCollection<MapLayerModel>(mapConfigurations.Select(c => new MapLayerModel(c)));
            OnPropertyChanged(nameof(Layers));
        }

        public async Task ImportMapDefinitionAsync()
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.List;
                openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
                openPicker.FileTypeFilter.Add(".json");
                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    string json = await FileIO.ReadTextAsync(file);
                    await _mapDefinitionManager.Import(json).ConfigureAwait(false);
                    await InitializeInternalAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                OutdoorTrackerEvents.Log.MapDefinitionOpenFileFailed(ex);
                await DialogHelper.ShowErrorAndReport(Messages.MapDefinitionManager.ImportError, Messages.MapDefinitionManager.ImportErrorTitle, ex).ConfigureAwait(false);
            }
        }
    }
}