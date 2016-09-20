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
        private string _busyText;

        public LayersViewModel()
        {
            Layers = new ObservableCollection<MapLayerModel>();
            AddMapCommand = new RelayCommand(async () => await ImportMapDefinition());
            ClearAllCacheCommand = new RelayCommand(async () => await ClearCache());
            LoadLayerSizeCommand = new RelayCommand(async () => await LoadLayerSizes());
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

        public string BusyText
        {
            get { return _busyText; }
            set
            {
                _busyText = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadLayerSizes()
        {
            LoadLayerSizeCommand.EnabledOverride = false;
            List<Task> loadTasks = new List<Task>();
            foreach (MapLayerModel layerModel in Layers)
            {
                loadTasks.Add(layerModel.LoadSize());
            }
            await Task.WhenAll(loadTasks);
            LoadLayerSizeCommand.EnabledOverride = null;
        }

        private async Task ClearCache()
        {
            using (MarkBusy())
            {
                if (await AskDeleteCache())
                {
                    IStorageFolder folder = await ApplicationData.Current.LocalCacheFolder.TryGetItemAsync("UMCCache") as IStorageFolder;
                    if (folder != null)
                    {
                        await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    }
                }
            }
        }

        private async Task<bool> AskDeleteCache()
        {
            MessageDialog dialog = new MessageDialog(Messages.LayersViewModel.DeleteCacheMessage, Messages.LayersViewModel.DeleteCacheMessageTitle);

            dialog.Commands.Add(new UICommand(Messages.Dialog.No) { Id = 0 });
            dialog.Commands.Add(new UICommand(Messages.LayersViewModel.YesDelete) { Id = 1 });

            dialog.DefaultCommandIndex = 1;
            dialog.CancelCommandIndex = 0;

            IUICommand result = await dialog.ShowAsync();
            return result.Id.Equals(0);
        }

        protected override async Task InitializeInternal()
        {
            IEnumerable<MapConfiguration> mapConfigurations = await _mapDefinitionManager.GetMapConfigurations().ConfigureAwait(false);
            Layers = new ObservableCollection<MapLayerModel>(mapConfigurations.Select(c => new MapLayerModel(c)));
            OnPropertyChanged(nameof(Layers));
        }

        public async Task ImportMapDefinition()
        {
            BusyText = Messages.LayersViewModel.ImportingMessage;
            using (MarkBusy())
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
                        await _mapDefinitionManager.Import(json);
                        await InitializeInternal();
                    }
                }
                catch (Exception ex)
                {
                    OutdoorTrackerEvents.Log.MapDefinitionOpenFileFailed(ex);
                    await DialogHelper.ShowErrorAndReport(Messages.MapDefinitionManager.ImportError, Messages.MapDefinitionManager.ImportErrorTitle, ex);
                }
            }
        }
    }
}