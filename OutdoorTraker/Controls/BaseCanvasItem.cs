using System;
using System.Numerics;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

using Microsoft.Graphics.Canvas;

using OutdoorTraker.Common;

namespace OutdoorTraker.Controls
{
	public abstract class BaseCanvasItem : DependencyObject, ICanvasItem
	{
		public event EventHandler LayoutChanged;

		public abstract void Draw(CanvasDrawingSession drawingSession, CanvasItemsLayer canvasItemsLayer);

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
			DispatcherHelper.InvokeOnUI(() => LayoutChanged?.Invoke(this, EventArgs.Empty));
		}
	}
}