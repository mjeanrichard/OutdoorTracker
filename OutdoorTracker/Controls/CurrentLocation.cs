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
using System.Numerics;

using Windows.UI;
using Windows.UI.Xaml;

using Microsoft.Graphics.Canvas;

using OutdoorTracker.Views.Map;

using UniversalMapControl.Interfaces;

namespace OutdoorTracker.Controls
{
    public class CurrentLocation : BaseCanvasItem
    {
        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(ILocation), typeof(CurrentLocation), new PropertyMetadata(default(ILocation), DependencyPropertyChanged));
        public static readonly DependencyProperty AccuracyMeterProperty = DependencyProperty.Register("AccuracyMeter", typeof(double), typeof(CurrentLocation), new PropertyMetadata(default(double), DependencyPropertyChanged));

        public double AccuracyMeter
        {
            get { return (double)GetValue(AccuracyMeterProperty); }
            set { SetValue(AccuracyMeterProperty, value); }
        }

        public static readonly DependencyProperty AccuracyTypeProperty = DependencyProperty.Register("AccuracyType", typeof(LocationAccuracy), typeof(CurrentLocation), new PropertyMetadata(LocationAccuracy.None, DependencyPropertyChanged));

        public LocationAccuracy AccuracyType
        {
            get { return (LocationAccuracy)GetValue(AccuracyTypeProperty); }
            set { SetValue(AccuracyTypeProperty, value); }
        }


        private static void DependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CurrentLocation)d).OnLayoutChanged();
        }

        public ILocation Location
        {
            get { return (ILocation)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        public static readonly DependencyProperty ShowAccuracyProperty = DependencyProperty.Register("ShowAccuracy", typeof(bool), typeof(CurrentLocation), new PropertyMetadata(false, DependencyPropertyChanged));

        public bool ShowAccuracy
        {
            get { return (bool)GetValue(ShowAccuracyProperty); }
            set { SetValue(ShowAccuracyProperty, value); }
        }

        public static readonly DependencyProperty ShowCurrentPositionProperty = DependencyProperty.Register("ShowCurrentPosition", typeof(bool), typeof(CurrentLocation), new PropertyMetadata(false, DependencyPropertyChanged));

        public bool ShowCurrentPosition
        {
            get { return (bool)GetValue(ShowCurrentPositionProperty); }
            set { SetValue(ShowCurrentPositionProperty, value); }
        }

        private static Color CreateAlphaColor(Color color, byte alpha)
        {
            return Color.FromArgb(alpha, color.R, color.G, color.B);
        }


        private bool _isGeometryValid;
        private Vector2 _locationVector;
        private float _accuracyRadius;
        private Color _innerCircleColor;
        private Color _accuracyCircleColor;
        private Color _outerCircleColor;

        public CurrentLocation()
        {
            AccuracyCircleColor = Colors.LightSkyBlue;
            OuterCircleColor = Colors.Yellow;
            HighAccuracyColor = Colors.ForestGreen;
            LowAccuracyColor = Colors.Orange;
            NoneAccuracyColor = Colors.Red;
            InnerCircleWidth = 3f;
            OuterCircleWidth = 5f;
            CircleRadius = 12;
            LocationCircleAlpha = 170;
            AccuracyCircleAlpha = 125;
        }

        public Color NoneAccuracyColor { get; set; }

        private Color HighAccuracyColor { get; set; }
        private Color LowAccuracyColor { get; set; }

        public Color AccuracyCircleColor { get; set; }
        public Color OuterCircleColor { get; set; }
        public float InnerCircleWidth { get; set; }
        public float OuterCircleWidth { get; set; }
        public float CircleRadius { get; set; }
        public byte LocationCircleAlpha { get; set; }
        public byte AccuracyCircleAlpha { get; set; }

        public override void InvalidateInternal()
        {
            _isGeometryValid = false;
        }

        public override void Draw(CanvasDrawingSession drawingSession, CanvasItemsLayer canvasItemsLayer)
        {
            if (!_isGeometryValid)
            {
                ILocation location = Location;
                if (location == null)
                {
                    return;
                }
                Recalculate(canvasItemsLayer, location);
            }
            if (ShowAccuracy)
            {
                drawingSession.FillCircle(_locationVector, canvasItemsLayer.Scale(_accuracyRadius), _accuracyCircleColor);
            }
            if (ShowCurrentPosition)
            {
                drawingSession.DrawCircle(_locationVector, CircleRadius, _outerCircleColor, OuterCircleWidth);
                drawingSession.DrawCircle(_locationVector, CircleRadius, _innerCircleColor, InnerCircleWidth);
            }
        }

        private void Recalculate(CanvasItemsLayer canvasItemsLayer, ILocation location)
        {
            _accuracyCircleColor = CreateAlphaColor(AccuracyCircleColor, AccuracyCircleAlpha);
            _outerCircleColor = CreateAlphaColor(OuterCircleColor, LocationCircleAlpha);

            switch (AccuracyType)
            {
                case LocationAccuracy.High:
                    _innerCircleColor = CreateAlphaColor(HighAccuracyColor, LocationCircleAlpha);
                    break;
                case LocationAccuracy.Low:
                    _innerCircleColor = CreateAlphaColor(LowAccuracyColor, LocationCircleAlpha);
                    break;
                case LocationAccuracy.None:
                    _innerCircleColor = CreateAlphaColor(NoneAccuracyColor, LocationCircleAlpha);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IProjection viewPortProjection = canvasItemsLayer.ParentMap.ViewPortProjection;
            _locationVector = canvasItemsLayer.Scale(viewPortProjection.ToCartesian(location));

            double accuracyScale = viewPortProjection.CartesianScaleFactor(location);
            _accuracyRadius = Convert.ToSingle(AccuracyMeter * accuracyScale);
            _isGeometryValid = true;
        }
    }
}