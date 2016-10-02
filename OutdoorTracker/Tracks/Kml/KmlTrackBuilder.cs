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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using Windows.Foundation;
using Windows.Storage;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Helpers;
using OutdoorTracker.Resources;

namespace OutdoorTracker.Tracks.Kml
{
    public class KmlTrackBuilder : TrackBuilder
    {
        private readonly XmlSerializer _kmlSerializerV21 = new XmlSerializer(typeof(KmlTypeV21));
        private readonly XmlSerializer _kmlSerializerV22 = new XmlSerializer(typeof(KmlTypeV22));

        public KmlTrackBuilder(UnitOfWorkFactoy unitOfWorkFactory) : base(unitOfWorkFactory)
        {
        }

        protected override string FormatName
        {
            get { return Messages.KmlTrackBuilder.Format; }
        }

        public async Task<IEnumerable<Track>> Import(XmlReader xmlReader, string filename)
        {
            List<Track> importedTracks = new List<Track>();
            XmlSerializer kmlSerializer = GetSerializer(xmlReader);
            KmlType kmlFile = (KmlType)kmlSerializer.Deserialize(xmlReader);

            List<KmlPlacemark> placemarks = kmlFile.Document?.Placemarks;
            if ((placemarks == null) || !placemarks.Any())
            {
                await ImportFailed(Messages.KmlTrackBuilder.NoTracksFoundMessage, Messages.KmlTrackBuilder.NoTracksFound);
                return importedTracks;
            }

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                foreach (KmlPlacemark placemark in placemarks)
                {
                    if (string.IsNullOrWhiteSpace(placemark.LineString?.Coordinates))
                    {
                        continue;
                    }

                    string[] coordinates = placemark.LineString.Coordinates.Split(' ');
                    if (coordinates.Length > 0)
                    {
                        List<Point> points = new List<Point>();
                        foreach (string coordinate in coordinates)
                        {
                            string[] coordParts = coordinate.Split(',');
                            if (coordParts.Length == 2)
                            {
                                double lat;
                                double lon;
                                if (double.TryParse(coordParts[1], out lat) && double.TryParse(coordParts[0], out lon))
                                {
                                    points.Add(new Point(lat, lon));
                                }
                            }
                        }
                        importedTracks.Add(await CreateTrack(kmlFile.Document.Name, points, unitOfWork));
                    }
                }
            }
            if (!importedTracks.Any())
            {
                await ImportFailed(Messages.KmlTrackBuilder.NoTracksFoundMessage, Messages.KmlTrackBuilder.NoTracksFound);
                return importedTracks;
            }
            return importedTracks;
        }

        public async Task Export(StorageFile file, int[] trackIds, IUnitOfWork unitOfWork)
        {
            List<Track> tracks = await unitOfWork.Tracks.Where(t => trackIds.Contains(t.Id)).ToListAsync();
            KmlType kml = new KmlTypeV22();
            kml.Document = new KmlDocument();
            foreach (Track track in tracks)
            {
                KmlPlacemark placemark = new KmlPlacemark();
                kml.Document.Placemarks.Add(placemark);
                placemark.Name = track.Name;
                placemark.LineString = new KmlLineString();

                List<TrackPoint> trackPoints = await unitOfWork.TrackPoints.Where(p => p.TrackId == track.Id).OrderBy(t => t.Number).ToListAsync();
                StringBuilder lineStringBuilder = new StringBuilder();
                foreach (TrackPoint trackPoint in trackPoints)
                {
                    lineStringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0},{1} ", trackPoint.Longitude, trackPoint.Latitude);
                }
                placemark.LineString.Coordinates = lineStringBuilder.ToString();
            }
            CachedFileManager.DeferUpdates(file);
            using (Stream xmlFile = await file.OpenStreamForWriteAsync())
            {
                xmlFile.Position = 0;
                xmlFile.SetLength(0);
                _kmlSerializerV22.Serialize(xmlFile, kml);
            }
            await CachedFileManager.CompleteUpdatesAsync(file);
            await DialogHelper.ShowMessage(Messages.KmlTrackBuilder.Success, Messages.KmlTrackBuilder.SuccessTitle);
        }

        private XmlSerializer GetSerializer(XmlReader xmlReader)
        {
            if (xmlReader.NamespaceURI.Equals(KmlType.KmlV21Namespace, StringComparison.OrdinalIgnoreCase))
            {
                return _kmlSerializerV21;
            }
            if (xmlReader.NamespaceURI.Equals(KmlType.KmlV22Namespace, StringComparison.OrdinalIgnoreCase))
            {
                return _kmlSerializerV22;
            }
            throw new InvalidOperationException($"Unknown KML Namespace '{xmlReader.NamespaceURI}'.");
        }

    }
}