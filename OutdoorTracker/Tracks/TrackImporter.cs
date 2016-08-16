using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Windows.Foundation;
using Windows.UI.Popups;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Tracks.Gpx;
using OutdoorTracker.Tracks.Kml;

namespace OutdoorTracker.Tracks
{
	public class TrackImporter
	{
		private readonly UnitOfWorkFactoy _unitOfWorkFactory;

		public TrackImporter(UnitOfWorkFactoy unitOfWorkFactory)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
		}

		public async Task<IEnumerable<Track>> ImportKml(Stream xml, string filename)
		{
			List<Track> importedTracks = new List<Track>();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(KmlType));
			KmlType kmlFile = (KmlType)xmlSerializer.Deserialize(xml);

			if (string.IsNullOrWhiteSpace(kmlFile.Document?.Placemark?.LineString?.Coordinates))
			{
				await ImportFailed("There are not Tracks in this File.");
				return importedTracks;
			}

			using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
			{
				string[] coordinates = kmlFile.Document.Placemark.LineString.Coordinates.Split(' ');
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
			return importedTracks;
		}

		public async Task<IEnumerable<Track>> ImportGpx(Stream xml, string filename)
		{
			List<Track> importedTracks = new List<Track>();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(GpxType));
			GpxType gpxFile = (GpxType)xmlSerializer.Deserialize(xml);

			if (!gpxFile.Tracks.Any() && !gpxFile.Waypoints.Any())
			{
				await ImportFailed("There are not Tracks in this File.");
				return importedTracks;
			}

			using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
			{
				if (gpxFile.Tracks.Any())
				{
					foreach (TrackType gpxTrack in gpxFile.Tracks)
					{
						importedTracks.Add(await CreateTrack(gpxTrack.Name, gpxTrack.Segments.SelectMany(s => s.Points).Select(p => p.ToPoint()), unitOfWork));
					}
				}
				if (gpxFile.Waypoints.Any())
				{
					importedTracks.Add(await CreateTrack(filename, gpxFile.Waypoints.Select(p => p.ToPoint()), unitOfWork));
				}
			}
			return importedTracks;
		}

		private async Task<Track> CreateTrack(string name, IEnumerable<Point> points, IUnitOfWork unitOfWork)
		{
			Track track = new Track();
			track.Name = name;
			int pointNumber = 0;
			foreach (Point point in points)
			{
				TrackPoint trackPoint = new TrackPoint();
				trackPoint.Track = track;
				trackPoint.Latitude = point.X;
				trackPoint.Longitude = point.Y;
				trackPoint.Time = DateTime.Now;
				trackPoint.Number = pointNumber++;
				unitOfWork.TrackPoints.Add(trackPoint);
			}
			unitOfWork.Tracks.Add(track);
			await unitOfWork.SaveChangesAsync();
			return track;
		}

		private async Task ImportFailed(string message)
		{
			var dialog = new MessageDialog(message, "GPX Import failed");

			dialog.Commands.Add(new UICommand("OK") { Id = true });

			dialog.DefaultCommandIndex = 0;
			dialog.CancelCommandIndex = 0;

			await dialog.ShowAsync();
		}
	}
}