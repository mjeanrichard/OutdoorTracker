using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.System;
using Windows.UI.Popups;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Resources;

namespace OutdoorTracker.Tracks
{
    public abstract class TrackBuilder
    {
        private static readonly Uri GithubTrackerUri = new Uri("https://github.com/mjeanrichard/OutdoorTracker/issues/new");

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

        protected async Task ImportFailed(string message, string title = null)
        {
            if (title == null)
            {
                title = Messages.TrackBuilder.ImportFailedTitle(FormatName);
            }
            var dialog = new MessageDialog(message, title);

            dialog.Commands.Add(new UICommand(Messages.Dialog.Ok) { Id = 1 });
            dialog.Commands.Add(new UICommand(Messages.Dialog.GotoGithub) { Id = 2 });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;

            IUICommand result = await dialog.ShowAsync();
            if (result.Id.Equals(2))
            {
                await Launcher.LaunchUriAsync(GithubTrackerUri);
            }
        }
    }
}