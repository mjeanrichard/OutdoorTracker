using System.Collections.Generic;
using System.Xml.Serialization;

namespace OutdoorTracker.Tracks.Gpx
{
	[XmlType("trkType", Namespace = "http://www.topografix.com/GPX/1/1")]
	public class TrackType
	{
		[XmlElement("name")]
		public string Name { get; set; }


		[XmlElement("cmt")]
		public string Cmt { get; set; }


		[XmlElement("desc")]
		public string Desc { get; set; }



		[XmlElement("number")]
		public int Number { get; set; }


		[XmlElement("type")]
		public string Type { get; set; }

		
		[XmlElement("trkseg")]
		public List<TrackSegmentType> Segments { get; set; }
	}
}