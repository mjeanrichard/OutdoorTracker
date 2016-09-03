using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Popups;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Resources;

namespace OutdoorTracker.Tracks
{
    public abstract class TrackBuilder
    {
        protected UnitOfWorkFactoy UnitOfWorkFactory { get; }

        public TrackBuilder(UnitOfWorkFactoy unitOfWorkFactory)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
        }
        protected abstract string FormatName { get; }

        protected async Task<Track> CreateTrack(string name, IEnumerable<Point> points, IUnitOfWork unitOfWork)
        {
            Track track = new Track();
            track.Name = name;
            int pointNumber = 0;
            foreach (Point point in points)
            {
                TrackPoint trackPoint = new TrackPoint();
                trackPoint.Track = track;
                trackPoint.Latitude = point.X;
                trackPoint.Longitude = point.Y;
                trackPoint.Time = DateTime.Now;
                trackPoint.Number = pointNumber++;
                unitOfWork.TrackPoints.Add(trackPoint);
            }
            unitOfWork.Tracks.Add(track);
            await unitOfWork.SaveChangesAsync();
            return track;
        }

        protected async Task ImportFailed(string message)
        {
            var dialog = new MessageDialog(message, Messages.TrackBuilder.ImportFailedTitle(FormatName));

            dialog.Commands.Add(new UICommand(Messages.Dialog.Ok) { Id = true });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;

            await dialog.ShowAsync();
        }
    }
}