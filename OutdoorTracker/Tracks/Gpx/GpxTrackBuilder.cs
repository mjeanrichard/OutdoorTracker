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
using System.Xml;
using System.Xml.Serialization;

using Windows.Storage;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Helpers;
using OutdoorTracker.Resources;

namespace OutdoorTracker.Tracks.Gpx
{
    public class GpxTrackBuilder : TrackBuilder
    {
        private readonly XmlSerializer _gpxSerializer10 = new XmlSerializer(typeof(GpxTypeV10));
        private readonly XmlSerializer _gpxSerializer11 = new XmlSerializer(typeof(GpxTypeV11));

        public GpxTrackBuilder(UnitOfWorkFactoy unitOfWorkFactory) : base(unitOfWorkFactory)
        {
        }

        protected override string FormatName
        {
            get { return Messages.GpxTrackBuilder.Format; }
        }

        public async Task<IEnumerable<Track>> Import(XmlReader xmlReader, string filename)
        {
            List<Track> importedTracks = new List<Track>();

            XmlSerializer serializer = GetSerializer(xmlReader);
            GpxType gpxFile = (GpxType)serializer.Deserialize(xmlReader);

            if (!gpxFile.Tracks.Any() && !gpxFile.Waypoints.Any())
            {
                await ImportFailed(Messages.GpxTrackBuilder.NoTracksFoundMessage, Messages.GpxTrackBuilder.NoTracksFound);
                return importedTracks;
            }

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                if (gpxFile.Tracks.Any())
                {
                    foreach (GpxTrackType gpxTrack in gpxFile.Tracks)
                    {
                        string gpxTrackName = gpxTrack.Name;
                        if (string.IsNullOrWhiteSpace(gpxTrackName))
                        {
                            gpxTrackName = filename;
                        }
                        importedTracks.Add(await CreateTrack(gpxTrackName, gpxTrack.Segments.SelectMany(s => s.Points).Select(p => p.ToPoint()), unitOfWork));
                    }
                }
                if (gpxFile.Waypoints.Any())
                {
                    importedTracks.Add(await CreateTrack(filename, gpxFile.Waypoints.Select(p => p.ToPoint()), unitOfWork));
                }
            }
            return importedTracks;
        }

        private XmlSerializer GetSerializer(XmlReader xmlReader)
        {
            if (xmlReader.NamespaceURI.Equals(GpxType.GpxV10Namespace, StringComparison.OrdinalIgnoreCase))
            {
                return _gpxSerializer10;
            }
            if (xmlReader.NamespaceURI.Equals(GpxType.GpxV11Namespace, StringComparison.OrdinalIgnoreCase))
            {
                return _gpxSerializer11;
            }
            throw new InvalidOperationException($"Unknown GPX Namespace '{xmlReader.NamespaceURI}'.");
        }

        public async Task Export(StorageFile file, int[] trackIds, IUnitOfWork unitOfWork)
        {
            List<Track> tracks = await unitOfWork.Tracks.Where(t => trackIds.Contains(t.Id)).ToListAsync();
            GpxType gpxDocument = new GpxTypeV11();
            foreach (Track track in tracks)
            {
                GpxTrackType gpxTrack = new GpxTrackType();
                TrackSegmentType segment = new TrackSegmentType();
                gpxDocument.Tracks.Add(gpxTrack);
                gpxTrack.Segments.Add(segment);
                gpxTrack.Name = track.Name;

                List<TrackPoint> trackPoints = await unitOfWork.TrackPoints.Where(p => p.TrackId == track.Id).OrderBy(t => t.Number).ToListAsync();
                foreach (TrackPoint trackPoint in trackPoints)
                {
                    WaypointType waypoint = new WaypointType();
                    waypoint.Latitude = trackPoint.Latitude;
                    waypoint.Longitude = trackPoint.Longitude;
                    waypoint.Elevation = trackPoint.Altitude;
                    waypoint.Time = trackPoint.Time;
                    segment.Points.Add(waypoint);
                }
            }
            CachedFileManager.DeferUpdates(file);
            using (Stream xmlFile = await file.OpenStreamForWriteAsync())
            {
                xmlFile.Position = 0;
                xmlFile.SetLength(0);
                _gpxSerializer11.Serialize(xmlFile, gpxDocument);
            }
            await CachedFileManager.CompleteUpdatesAsync(file);
            await DialogHelper.ShowMessage(Messages.GpxTrackBuilder.Success, Messages.GpxTrackBuilder.SuccessTitle);
        }
    }
}