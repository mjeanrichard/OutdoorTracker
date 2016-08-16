using System.Collections.Generic;
using System.Xml.Serialization;

namespace OutdoorTracker.Tracks.Gpx
{
	[XmlType("trksegType", Namespace = "http://www.topografix.com/GPX/1/1")]
	public class TrackSegmentType
	{
		[XmlElement("trkpt")]
		public List<WaypointType> Points { get; set; }
	}
}