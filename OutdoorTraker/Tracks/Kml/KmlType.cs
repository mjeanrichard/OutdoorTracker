using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using OutdoorTraker.Tracks.Gpx;

namespace OutdoorTraker.Tracks.Kml
{
	[XmlType(Namespace = "http://www.opengis.net/kml/2.2")]
	[XmlRoot("kml", Namespace = "http://www.opengis.net/kml/2.2", IsNullable = false)]
	public class KmlType
	{
		[XmlElement("Document")]
		public KmlDocument Document { get; set; }
	}
}
