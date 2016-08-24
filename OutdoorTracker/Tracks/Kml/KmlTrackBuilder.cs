using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Windows.Foundation;
using Windows.Storage;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Helpers;
using OutdoorTracker.Tracks.Gpx;

namespace OutdoorTracker.Tracks.Kml
{
    public class KmlTrackBuilder : TrackBuilder
    {
        readonly XmlSerializer _kmlSerializer = new XmlSerializer(typeof(KmlType));

        public async Task<IEnumerable<Track>> Import(Stream xml, string filename)
        {
            List<Track> importedTracks = new List<Track>();
            KmlType kmlFile = (KmlType)_kmlSerializer.Deserialize(xml);

            List<KmlPlacemark> placemarks = kmlFile.Document?.Placemarks;
            if (placemarks == null || !placemarks.Any())
            {
                await ImportFailed("There are not Tracks in this File.");
                return importedTracks;
            }

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                foreach (KmlPlacemark placemark in kmlFile.Document.Placemarks)
                {
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
            return importedTracks;
        }

        public KmlTrackBuilder(UnitOfWorkFactoy unitOfWorkFactory) : base(unitOfWorkFactory)
        {
        }

        protected override string FormatName { get { return "KML"; } }

        public async Task Export(StorageFile file, int[] trackIds, IUnitOfWork unitOfWork)
        {
            List<Track> tracks = await unitOfWork.Tracks.Where(t => trackIds.Contains(t.Id)).ToListAsync();
            KmlType kml = new KmlType();
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
                _kmlSerializer.Serialize(xmlFile, kml);
            }
            await CachedFileManager.CompleteUpdatesAsync(file);
            await DialogHelper.ShowMessage("The selected tracks have been exported successfully.", "Export completed");
        }
    }
}