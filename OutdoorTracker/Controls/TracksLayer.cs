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
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml;

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

using OutdoorTracker.Tracks;

using UniversalMapControl.Interfaces;

namespace OutdoorTracker.Controls
{
    public class TracksLayer : BaseCanvasItem
    {
        public static readonly DependencyProperty TracksProperty = DependencyProperty.Register("Tracks", typeof(object), typeof(TracksLayer), new PropertyMetadata(default(object), TracksChanged));

        private static void TracksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TracksLayer tracksLayer = d as TracksLayer;
            if (tracksLayer != null)
            {
                tracksLayer.OnTracksChanged((ObservableCollection<Track>)e.OldValue, (ObservableCollection<Track>)e.NewValue);
            }
        }

        public ObservableCollection<Track> Tracks
        {
            get { return (ObservableCollection<Track>)GetValue(TracksProperty); }
            set { SetValue(TracksProperty, value); }
        }

        private volatile bool _isPathValid = false;

        private List<TrackContainer> _trackPaths;

        private async void OnTracksChanged(ObservableCollection<Track> oldValue, ObservableCollection<Track> newValue)
        {
            if (oldValue != null)
            {
                foreach (Track oldTrack in oldValue)
                {
                    oldTrack.Points.CollectionChanged -= TrackPointsCollectionChanged;
                }
                oldValue.CollectionChanged -= TracksCollectionChanged;
            }
            if (newValue != null)
            {
                newValue.CollectionChanged += TracksCollectionChanged;
                foreach (Track newTrack in newValue)
                {
                    newTrack.Points.CollectionChanged += TrackPointsCollectionChanged;
                }
            }
            await InvalidateTrack();
        }

        private async void TracksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Track oldTrack in e.OldItems)
                {
                    oldTrack.Points.CollectionChanged -= TrackPointsCollectionChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (Track newTrack in e.NewItems)
                {
                    newTrack.Points.CollectionChanged += TrackPointsCollectionChanged;
                }
            }
            await InvalidateTrack();
        }

        private async void TrackPointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            await InvalidateTrack();
        }

        public async Task InvalidateTrack()
        {
            _isPathValid = false;
            await OnLayoutChanged();
        }

        private void BuildPathGeometry(CanvasDevice device, CanvasItemsLayer canvasItemsLayer)
        {
            List<TrackContainer> oldList = _trackPaths;
            _trackPaths = new List<TrackContainer>();
            if (oldList != null)
            {
                foreach (TrackContainer trackPath in oldList)
                {
                    trackPath.Dispose();
                }
            }

            IProjection projection = canvasItemsLayer.ParentMap.ViewPortProjection;

            foreach (Track track in Tracks)
            {
                if (track.Points.Any())
                {
                    CanvasGeometry trackPath = BuildPathFromTrack(projection, track, device, canvasItemsLayer);
                    _trackPaths.Add(new TrackContainer(trackPath, track));
                }
            }
            _isPathValid = true;
        }

        private CanvasGeometry BuildPathFromTrack(IProjection projection, Track first, CanvasDevice device, CanvasItemsLayer canvasItemsLayer)
        {
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(device);
            TrackPoint[] trackPoints = first.Points.OrderBy(p => p.Number).ToArray();

            Vector2 vector = canvasItemsLayer.Scale(projection.ToCartesian(trackPoints[0].Location));
            pathBuilder.BeginFigure(vector);
            for (int i = 1; i < trackPoints.Length; i++)
            {
                TrackPoint trackPoint = trackPoints[i];
                vector = canvasItemsLayer.Scale(projection.ToCartesian(trackPoint.Location));
                pathBuilder.AddLine(vector);
            }
            pathBuilder.EndFigure(CanvasFigureLoop.Open);
            return CanvasGeometry.CreatePath(pathBuilder);
        }

        public override void Draw(CanvasDrawingSession drawingSession, CanvasItemsLayer canvasItemsLayer)
        {
            if (Tracks == null)
            {
                return;
            }
            if (!_isPathValid)
            {
                BuildPathGeometry(drawingSession.Device, canvasItemsLayer);
            }
            foreach (TrackContainer trackPath in _trackPaths)
            {
                trackPath.Draw(drawingSession);
            }
        }

        public override void InvalidateInternal()
        {
            _isPathValid = false;
        }

        private class TrackContainer : IDisposable
        {
            private readonly Track _track;

            public TrackContainer(CanvasGeometry canvasGeometry, Track track)
            {
                _track = track;
                Geometry = canvasGeometry;
            }

            public CanvasGeometry Geometry { get; set; }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (Geometry != null)
                    {
                        Geometry.Dispose();
                    }
                }
            }

            public void Draw(CanvasDrawingSession drawingSession)
            {
                drawingSession.DrawGeometry(Geometry, _track.Color, _track.Width);
            }
        }
    }
}