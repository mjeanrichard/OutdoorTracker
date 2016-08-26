// 
// Outdoor Tracker - Copyright(C) 2016 Meinard Jean-Richard
//  
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Xml.Serialization;

namespace OutdoorTracker.Tracks.Gpx
{
    [XmlType("trkType", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class GpxTrackType
    {
        public GpxTrackType()
        {
            Segments = new List<TrackSegmentType>();
        }

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