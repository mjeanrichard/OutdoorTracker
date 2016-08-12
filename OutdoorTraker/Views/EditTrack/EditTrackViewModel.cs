using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Navigation;

using Microsoft.EntityFrameworkCore;

using OutdoorTraker.Common;
using OutdoorTraker.Database;
using OutdoorTraker.Services;
using OutdoorTraker.Tracks;

namespace OutdoorTraker.Views.EditTrack
{
	public class EditTrackViewModel : BaseViewModel
	{
		private readonly NavigationService _navigationService;
		private readonly TrackRecorder _trackRecorder;
		private readonly IUnitOfWork _unitOfWork;
		private int? _trackId;
		private Track _track;

		public EditTrackViewModel()
		{
		}

		public EditTrackViewModel(NavigationEventArgs parameter, IUnitOfWork unitOfWork, NavigationService navigationService, TrackRecorder trackRecorder)
			: this()
		{
			_unitOfWork = unitOfWork;
			_navigationService = navigationService;
			_trackRecorder = trackRecorder;

			CancelCommand = new RelayCommand(() => _navigationService.GoBack());
			SaveCommand = new RelayCommand(async () => await SaveTrack());
			CreateCommand = new RelayCommand(async () => await CreateNewTrack());

			_trackId = parameter.Parameter as int?;
		}

		public RelayCommand SaveCommand { get; private set; }
		public RelayCommand CreateCommand { get; private set; }
		public RelayCommand CancelCommand { get; private set; }

		public bool IsNew => !_trackId.HasValue;

		public Track Track
		{
			get { return _track; }
			set
			{
				_track = value;
				OnPropertyChanged();
			}
		}

		private async Task CreateNewTrack()
		{
			if (_trackRecorder.IsTracking)
			{
				_trackRecorder.StopTracking();
			}
			_unitOfWork.Tracks.Add(Track);
			await _unitOfWork.SaveChangesAsync();
			await _trackRecorder.StartTracking(Track);
			_navigationService.NavigateToMap();
		}

		private async Task SaveTrack()
		{
			await _unitOfWork.SaveChangesAsync();
			_navigationService.GoBack();
		}

		protected override async Task InitializeInternal()
		{
			if (_trackId.HasValue)
			{
				int trackId = _trackId.Value;
				Track track = await _unitOfWork.Tracks.SingleOrDefaultAsync(t => t.Id == trackId);
				if (track == null)
				{
					_navigationService.GoBack();
				}
				Track = track;
			}
			else
			{
				Track = new Track();
				Track.Name = $"Track {DateTime.Now.ToString("s")}";
				Track.ShowOnMap = true;
			}
		}
	}
}