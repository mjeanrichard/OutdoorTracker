using System.Collections.Generic;
using System.Xml.Serialization;

namespace OutdoorTraker.Tracks.Gpx
{
	[XmlType(Namespace = "http://www.topografix.com/GPX/1/1")]
	[XmlRoot("gpx", Namespace = "http://www.topografix.com/GPX/1/1", IsNullable = false)]
	public class GpxType
	{
		[XmlElement("trk")]
		public List<TrackType> Tracks { get; set; }

		[XmlElement("wpt")]
		public List<WaypointType> Waypoints { get; set; }
	}
}