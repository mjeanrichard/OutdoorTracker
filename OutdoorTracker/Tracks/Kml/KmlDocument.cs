using System.Xml.Serialization;

namespace OutdoorTracker.Tracks.Kml
{
	[XmlType("Document", Namespace = "http://www.opengis.net/kml/2.2")]
	public class KmlDocument
	{
		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("Placemark")]
		public KmlPlacemark Placemark { get; set; }
	}
}