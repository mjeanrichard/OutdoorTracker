using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OutdoorTracker.Tracks
{
	public class Track
	{
		public Track()
		{
			Points = new ObservableCollection<TrackPoint>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool ShowOnMap { get; set; }
		public ObservableCollection<TrackPoint> Points { get; set; }
	}
}