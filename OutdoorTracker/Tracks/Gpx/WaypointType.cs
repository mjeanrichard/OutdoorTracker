using System.Xml.Serialization;

using Windows.Foundation;

namespace OutdoorTracker.Tracks.Gpx
{
	[XmlType("wptType", Namespace = "http://www.topografix.com/GPX/1/1")]
	public class WaypointType
	{
		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("cmt")]
		public string Comment { get; set; }

		[XmlElement("desc")]
		public string Description { get; set; }

		[XmlElement("src")]
		public string Source { get; set; }

		[XmlElement("sym")]
		public string Symbol { get; set; }

		[XmlElement("type")]
		public string Type { get; set; }

		[XmlAttribute("lat")]
		public double Latitude { get; set; }

		[XmlAttribute("lon")]
		public double Longitude { get; set; }

		public Point ToPoint()
		{
			return new Point(Latitude, Longitude);
		}
	}
}