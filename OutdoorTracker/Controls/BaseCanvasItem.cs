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

using Windows.UI.Xaml;

using Microsoft.Graphics.Canvas;

using OutdoorTracker.Common;

namespace OutdoorTracker.Controls
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

        public async Task Invalidate()
        {
            InvalidateInternal();
            await OnLayoutChanged();
        }

        protected virtual async Task OnLayoutChanged()
        {
            InvalidateInternal();
            await DispatcherHelper.InvokeOnUiAsync(() => LayoutChanged?.Invoke(this, EventArgs.Empty));
        }
    }
}