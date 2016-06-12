using System;

using Windows.UI.Xaml;

using Microsoft.Graphics.Canvas;

using UniversalMapControl;

namespace OutdoorTraker.Controls
{
	public abstract class BaseCanvasItem : DependencyObject, ICanvasItem
	{
		public event EventHandler LayoutChanged;

		public abstract void Draw(CanvasDrawingSession drawingSession, Map parentMap, float scale);

		public void ParentInvalidated()
		{
			InvalidateInternal();
		}


		public abstract void InvalidateInternal();

		public void Invalidate()
		{
			InvalidateInternal();
			OnLayoutChanged();
		}

		protected virtual void OnLayoutChanged()
		{
			InvalidateInternal();
			LayoutChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}