using System.Xml.Serialization;

namespace OutdoorTracker.Tracks.Kml
{
	[XmlType("Placemark", Namespace = "http://www.opengis.net/kml/2.2")]
	public class KmlPlacemark
	{
		[XmlElement("LineString")]
		public KmlLineString LineString { get; set; }
	}
}