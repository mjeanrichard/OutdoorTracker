using System.Xml.Serialization;

namespace OutdoorTraker.Tracks.Kml
{
	[XmlType("LineString", Namespace = "http://www.opengis.net/kml/2.2")]
	public class KmlLineString
	{
		[XmlElement("coordinates")]
		public string Coordinates { get; set; }
	}
}