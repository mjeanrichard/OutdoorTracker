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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.FileProperties;

using Newtonsoft.Json;

using OutdoorTracker.Helpers;
using OutdoorTracker.Layers;
using OutdoorTracker.Resources;

namespace OutdoorTracker.Views.Layers
{
    public class MapLayerModel : INotifyPropertyChanged
    {
        private readonly MapConfiguration _mapConfiguration;
        private string _size;
        private bool _isBusy;
        private bool _isSizeVisible;

        public MapLayerModel(MapConfiguration mapConfiguration)
        {
            _mapConfiguration = mapConfiguration;
        }


        public string Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        public bool IsSizeVisible
        {
            get { return _isSizeVisible; }
            set
            {
                _isSizeVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _mapConfiguration.Name; }
        }

        public async Task LoadSize()
        {
            try
            {
                Size = Messages.LayersViewModel.LoadingSize;
                IsBusy = true;
                IsSizeVisible = true;
                MapDefinition mapDef = JsonConvert.DeserializeObject<MapDefinition>(_mapConfiguration.Json);
                ulong totalSize = 0;
                foreach (LayerDefinition layerDefinition in mapDef.Layers)
                {
                    string folderName = string.Format(CultureInfo.InvariantCulture, "UMCCache\\{0}", layerDefinition.Name);
                    IStorageFolder folder = await ApplicationData.Current.LocalCacheFolder.TryGetItemAsync(folderName) as IStorageFolder;
                    if (folder != null)
                    {
                        totalSize += await CalculateFolderSize(folder, totalSize);
                    }
                }
                totalSize = totalSize / (1024 * 1024);
                Size = Messages.LayersViewModel.SizeText(totalSize);
            }
            catch (Exception e)
            {
                DialogHelper.ReportException(e, new Dictionary<string, string>());
                Size = Messages.LayersViewModel.UnknownSize;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<ulong> CalculateFolderSize(IStorageFolder folder, ulong initialSize)
        {
            IReadOnlyList<StorageFolder> folders = await folder.GetFoldersAsync();
            foreach (StorageFolder subFolder in folders)
            {
                initialSize += await CalculateFolderSize(subFolder, initialSize);
            }
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                BasicProperties properties = await file.GetBasicPropertiesAsync();
                initialSize += properties.Size;
            }
            return initialSize;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}