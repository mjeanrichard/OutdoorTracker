using System.Xml.Serialization;

namespace OutdoorTraker.Tracks.Kml
{
	[XmlType("Placemark", Namespace = "http://www.opengis.net/kml/2.2")]
	public class KmlPlacemark
	{
		[XmlElement("LineString")]
		public KmlLineString LineString { get; set; }
	}
}