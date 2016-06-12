using System;

using UniversalMapControl.Interfaces;
using UniversalMapControl.Projections;

namespace OutdoorTraker.Tracks
{
	public class TrackPoint
	{
		public int Id { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }
		public double Altitude { get; set; }
		public int Number { get; set; }
		public DateTime Time { get; set; }
		public Track Track { get; set; }
		public int TrackId { get; set; }

		public ILocation Location
		{
			get { return new Wgs84Location(Latitude, Longitude);}
		}
	}
}