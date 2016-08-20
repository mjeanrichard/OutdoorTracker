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
using System.Threading.Tasks;

using Windows.ApplicationModel.ExtendedExecution;
using Windows.UI.Popups;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Tracks;

using UniversalMapControl.Interfaces;
using UniversalMapControl.Projections;

namespace OutdoorTracker.Services
{
	public class TrackRecorder
	{
		private readonly UnitOfWorkFactoy _unitOfWorkFactory;
		private readonly SettingsManager _settingsManager;
		private readonly GeoLocationService _geoLocationService;

		private int _nextTrackPointNumber;
		private ExtendedExecutionSession _extendedExecutionSession;

		private double _sumLatitude;
		private double _sumLongitude;
		private double? _sumAltitude;
		private int _avgCount;
		private int _avgAltCount;
		private ILocation _lastLocation;

		public TrackRecorder(UnitOfWorkFactoy unitOfWorkFactory, SettingsManager settingsManager, GeoLocationService geoLocationService)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
			_settingsManager = settingsManager;
			_geoLocationService = geoLocationService;
			_geoLocationService.PositionChanged += PositionChanged;
		}

		public bool IsTracking { get; private set; }

		public Track RecordingTrack { get; private set; }

		public event EventHandler TrackUpdated;

		private async void PositionChanged(object sender, EventArgs e)
		{
			if (IsTracking && _geoLocationService.CurrentLocation.IsLocationValid)
			{
				ILocation location = _geoLocationService.CurrentLocation.Location;
				if (_lastLocation != null)
				{
					_sumLatitude += location.Latitude;
					_sumLongitude += location.Longitude;
					_avgCount++;
					if (_geoLocationService.CurrentLocation.IsAltitudeValid)
					{
						_sumAltitude += _geoLocationService.CurrentLocation.Altitude;
						_avgAltCount++;
					}

					double lat = _sumLatitude / _avgCount;
					double lng = _sumLongitude / _avgCount;
					double? alt = _sumAltitude / _avgAltCount;

					Wgs84Location avgLocation = new Wgs84Location(lat, lng);
					double distance = _lastLocation.DistanceTo(avgLocation);
					if (distance > 10)
					{
						_sumLongitude = 0;
						_sumLatitude = 0;
						_sumAltitude = null;
						_avgCount = 0;
						_avgAltCount = 0;
						await UpdateTrack(lat, lng, alt);
						_lastLocation = avgLocation;
					}
				}
				else
				{
					double? alt = _geoLocationService.CurrentLocation.IsAltitudeValid ? _geoLocationService.CurrentLocation.Altitude : (double?)null;
					await UpdateTrack(location.Latitude, location.Longitude, alt);
					_lastLocation = _geoLocationService.CurrentLocation.Location;
				}
			}
		}

		private async Task UpdateTrack(double latitude, double longitude, double? altitude)
		{
			using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
			{
				TrackPoint point = new TrackPoint();
				point.Number = _nextTrackPointNumber++;
				point.Time = DateTime.UtcNow;
				point.TrackId = RecordingTrack.Id;
				point.Latitude = latitude;
				point.Longitude = longitude;
				if (altitude.HasValue)
				{
					point.Altitude = altitude.Value;
				}
				unitOfWork.TrackPoints.Add(point);

				// this is only to update the current Map that might be displayed.
				RecordingTrack.Points.Add(point);
				await unitOfWork.SaveChangesAsync();
				OnTrackUpdated();
			}
		}

		public async Task StartTracking(Track track, int startNr = 0)
		{
			_settingsManager.CurrentTrackingId = track.Id;
			IsTracking = true;
			RecordingTrack = track;
			await StartLocationExtensionSession();
			OnTrackUpdated();
		}

		public void StopTracking()
		{
			IsTracking = false;
			_settingsManager.CurrentTrackingId = null;
			RecordingTrack = null;
			_extendedExecutionSession.Dispose();
			OnTrackUpdated();
		}

		protected virtual void OnTrackUpdated()
		{
			TrackUpdated?.Invoke(this, EventArgs.Empty);
		}

		private async Task StartLocationExtensionSession()
		{
			_extendedExecutionSession = new ExtendedExecutionSession();
			_extendedExecutionSession.Description = "Location Tracker";
			_extendedExecutionSession.Reason = ExtendedExecutionReason.LocationTracking;
			_extendedExecutionSession.Revoked += ExtendedExecutionSessionRevoked;
			ExtendedExecutionResult result = await _extendedExecutionSession.RequestExtensionAsync();
			if (result == ExtendedExecutionResult.Denied)
			{
				//TODO: handle denied
			}
		}

		private void ExtendedExecutionSessionRevoked(object sender, ExtendedExecutionRevokedEventArgs args)
		{
		}

		public async Task CheckExistingSession()
		{
			if (_settingsManager.CurrentTrackingId.HasValue)
			{
				if (await AskToContinueTracking())
				{
					using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
					{
						int trackId = _settingsManager.CurrentTrackingId.Value;
						Track track = await unitOfWork.Tracks.SingleOrDefaultAsync(t => t.Id == trackId);
						int trackPointCount = await unitOfWork.TrackPoints.CountAsync(p => p.TrackId == trackId);
						await StartTracking(track, trackPointCount);
					}
				}
				else
				{
					_settingsManager.CurrentTrackingId = null;
				}
			}
		}


		private async Task<bool> AskToContinueTracking()
		{
			var dialog = new MessageDialog("Would you like to continue the last Tracking session?", "Tracking session found");

			dialog.Commands.Add(new UICommand("Yes") { Id = true });
			dialog.Commands.Add(new UICommand("No") { Id = false });

			dialog.DefaultCommandIndex = 1;
			dialog.CancelCommandIndex = 0;

			IUICommand result = await dialog.ShowAsync();
			return (bool)result.Id;
		}
	}
}