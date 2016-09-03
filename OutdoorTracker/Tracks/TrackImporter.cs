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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Helpers;
using OutdoorTracker.Logging;
using OutdoorTracker.Resources;
using OutdoorTracker.Tracks.Gpx;
using OutdoorTracker.Tracks.Kml;

namespace OutdoorTracker.Tracks
{
    public class TrackImporter
    {
        private readonly GpxTrackBuilder _gpxTrackBuilder;
        private readonly KmlTrackBuilder _kmlTrackBuilder;
        private readonly UnitOfWorkFactoy _unitOfWorkFactoy;

        public TrackImporter(GpxTrackBuilder gpxTrackBuilder, KmlTrackBuilder kmlTrackBuilder, UnitOfWorkFactoy unitOfWorkFactoy)
        {
            _gpxTrackBuilder = gpxTrackBuilder;
            _kmlTrackBuilder = kmlTrackBuilder;
            _unitOfWorkFactoy = unitOfWorkFactoy;
        }

        public async Task<IEnumerable<Track>> ImportTracks()
        {
            string fileExtension = "";
            try
            {
                List<Track> createdTracks = new List<Track>();

                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.List;
                openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
                openPicker.FileTypeFilter.Add(".gpx");
                openPicker.FileTypeFilter.Add(".kml");
                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    IRandomAccessStreamWithContentType xmlStream = await file.OpenReadAsync();
                    IEnumerable<Track> tracks;
                    fileExtension = Path.GetExtension(file.Name);
                    if (fileExtension.Equals(".kml", StringComparison.OrdinalIgnoreCase))
                    {
                        tracks = await _kmlTrackBuilder.Import(xmlStream.AsStreamForRead(), file.Name);
                    }
                    else
                    {
                        tracks = await _gpxTrackBuilder.Import(xmlStream.AsStreamForRead(), file.Name);
                    }
                    foreach (Track track in tracks)
                    {
                        createdTracks.Add(track);
                    }
                }

                return createdTracks;
            }
            catch (Exception ex)
            {
                OutdoorTrackerEvents.Log.TrackImportError(fileExtension, ex);
                await DialogHelper.ShowErrorAndReport(Messages.TrackImporter.ImportError, Messages.TrackImporter.ImportErrorTitle, ex, new Dictionary<string, string> { { "FileExtension", fileExtension } });
                return Enumerable.Empty<Track>();
            }
        }

        public async Task ExportTracks(int[] trackIds)
        {
            string fileExtension = "";
            try
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add(Messages.TrackImporter.GpxFile, new[] { ".gpx" });
                savePicker.FileTypeChoices.Add(Messages.TrackImporter.KmlFile, new[] { ".kml" });
                savePicker.DefaultFileExtension = ".gpx";
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    using (IUnitOfWork unitOfWork = _unitOfWorkFactoy.Create())
                    {
                        fileExtension = Path.GetExtension(file.Name);
                        if (fileExtension.Equals(".kml", StringComparison.OrdinalIgnoreCase))
                        {
                            await _kmlTrackBuilder.Export(file, trackIds, unitOfWork);
                        }
                        else
                        {
                            await _gpxTrackBuilder.Export(file, trackIds, unitOfWork);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OutdoorTrackerEvents.Log.TrackExportError(fileExtension, ex);
                await DialogHelper.ShowErrorAndReport(Messages.TrackImporter.ExportError, Messages.TrackImporter.ExportErrorTitle, ex, new Dictionary<string, string> { { "FileExtension", fileExtension } });
            }
        }
    }
}