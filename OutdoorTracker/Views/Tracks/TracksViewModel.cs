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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.UI.Popups;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Resources;
using OutdoorTracker.Services;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Views.Tracks
{
    public class TracksViewModel : BaseViewModel
    {
        private readonly TrackRecorder _trackRecorder;
        private readonly TrackFixer _trackFixer;
        private readonly UnitOfWorkFactoy _unitOfWorkFactoy;
        private readonly TrackImporter _trackImporter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly NavigationService _navigationService;
        private ObservableCollection<TrackViewModel> _tracks;
        private IList<TrackViewModel> _selectedTracks;

        public TracksViewModel()
        {
            EditTrackCommand = new AsyncCommand(EditTrack, () => SelectedTracks != null && SelectedTracks.Count == 1, this);

            Func<bool> hasSelectedTracks = () => SelectedTracks != null && SelectedTracks.Count > 0;
            RebuildTrackCommand = new AsyncCommand(RebuildTracks, Messages.TracksPage.RebuildText, this);
            DeleteTrackCommand = new AsyncCommand(DeleteTrack, Messages.TracksPage.DeletingText, hasSelectedTracks, this);
            ExportTracksCommand = new AsyncCommand(ExportTracks, Messages.TracksPage.ExportingText, hasSelectedTracks, this);

            StartTrackingCommand = new AsyncCommand(StartTracking, this);
            ImportGpxTrackCommand = new AsyncCommand(ImportGpxTrack, Messages.TracksPage.ImportingText, this);

            _selectedTracks = new List<TrackViewModel>();
        }

        public TracksViewModel(TrackImporter trackImporter, IUnitOfWork unitOfWork, NavigationService navigationService, TrackRecorder trackRecorder, TrackFixer trackFixer, UnitOfWorkFactoy unitOfWorkFactoy)
            : this()
        {
            _trackRecorder = trackRecorder;
            _trackFixer = trackFixer;
            _unitOfWorkFactoy = unitOfWorkFactoy;
            _trackImporter = trackImporter;
            _unitOfWork = unitOfWork;
            _navigationService = navigationService;
        }



        public ObservableCollection<TrackViewModel> Tracks
        {
            get { return _tracks; }
            private set
            {
                _tracks = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ExportTracksCommand { get; set; }
        public RelayCommand StartTrackingCommand { get; }
        public RelayCommand ImportGpxTrackCommand { get; }
        public RelayCommand EditTrackCommand { get; }
        public RelayCommand RebuildTrackCommand { get; }
        public RelayCommand DeleteTrackCommand { get; }

        public IList<TrackViewModel> SelectedTracks
        {
            get { return _selectedTracks; }
            set
            {
                _selectedTracks = value;
                EditTrackCommand.RaiseCanExecuteChanged();
                DeleteTrackCommand.RaiseCanExecuteChanged();
                ExportTracksCommand.RaiseCanExecuteChanged();
                RebuildTrackCommand.RaiseCanExecuteChanged();
            }
        }

        private async Task ExportTracks()
        {
            if (SelectedTracks.Any())
            {
                await _trackImporter.ExportTracks(SelectedTracks.Select(t => t.Track.Id).ToArray());
            }
        }

        private async Task StartTracking()
        {
            if (_trackRecorder.IsTracking)
            {
                if (!await AskStopTracking())
                {
                    return;
                }
            }
            await _navigationService.NavigateToNewTrack();
        }

        private async Task DeleteTrack()
        {
            IList<TrackViewModel> selectedTracks = SelectedTracks;
            if (selectedTracks.Count == 0)
            {
                return;
            }

            string message;
            string title;
            if (selectedTracks.Count == 1)
            {
                message = Messages.TracksPage.DeleteSingleMessage(selectedTracks[0].Name);
                title = Messages.TracksPage.DeleteSingleTitle;
            }
            else
            {
                message = Messages.TracksPage.DeleteMessage;
                title = Messages.TracksPage.DeleteTitle;
            }
            if (await AskDelete(message, title))
            {
                await Task.Run(async () =>
                {
                    using (MarkBusy())
                    {
                        foreach (TrackViewModel selectedTrack in selectedTracks)
                        {
                            int trackId = selectedTrack.Track.Id;
                            if (_trackRecorder.IsTracking && _trackRecorder.RecordingTrack.Id == trackId)
                            {
                                _trackRecorder.StopTracking();
                            }
                            _unitOfWork.TrackPoints.RemoveRange(await _unitOfWork.TrackPoints.Where(p => p.TrackId == trackId).ToArrayAsync());
                            _unitOfWork.Tracks.Remove(await _unitOfWork.Tracks.SingleAsync(t => t.Id == trackId));
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                });
                foreach (TrackViewModel selectedTrack in selectedTracks)
                {
                    Tracks.Remove(selectedTrack);
                }
            }
        }

        private async Task RebuildTracks()
        {
            IList<TrackViewModel> trackViewModels;
            if (SelectedTracks != null && SelectedTracks.Any())
            {
                trackViewModels = SelectedTracks.ToArray();
            }
            else
            {
                trackViewModels = Tracks.ToArray();
            }
            foreach (TrackViewModel track in trackViewModels)
            {
                await _trackFixer.UpdateTrack(track.Track, _unitOfWork).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                track.Refresh();
            }
        }

        private async Task<bool> AskDelete(string message, string title)
        {
            var dialog = new MessageDialog(message, title);

            dialog.Commands.Add(new UICommand(Messages.TracksPage.YesDelete) { Id = true });
            dialog.Commands.Add(new UICommand(Messages.Dialog.Cancel) { Id = false });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();
            return (bool)result.Id;
        }

        private async Task<bool> AskStopTracking()
        {
            var dialog = new MessageDialog("You are already recoring a track. Do you want to stop recording and create a new track?");

            dialog.Commands.Add(new UICommand("Yes") { Id = true });
            dialog.Commands.Add(new UICommand("Cancel") { Id = false });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();
            return (bool)result.Id;
        }


        private async Task EditTrack()
        {
            if (SelectedTracks == null || SelectedTracks.Count != 1)
            {
                return;
            }
            await _navigationService.NavigateToEditTrack(SelectedTracks[0].Track.Id);
        }

        private async Task ImportGpxTrack()
        {
            IEnumerable<Track> importedTracks = await _trackImporter.ImportTracks();
            foreach (Track importedTrack in importedTracks)
            {
                Tracks.Add(new TrackViewModel(importedTrack, _unitOfWorkFactoy));
            }
        }

        protected override async Task LoadData()
        {
            List<Track> tracks = await _unitOfWork.Tracks.ToListAsync();
            Tracks = new ObservableCollection<TrackViewModel>(tracks.Select(t => new TrackViewModel(t, _unitOfWorkFactoy)));
        }
    }
}