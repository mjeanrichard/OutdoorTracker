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

using Windows.UI.Core;
using Windows.UI.Xaml;

namespace OutdoorTracker.Common
{
    public static class DispatcherHelper
    {
        public static void Init()
        {
            _uiDispatcher = Window.Current.Dispatcher;
        }

        public static async Task InvokeOnUiAsync(Action action)
        {
            if (action == null)
            {
                return;
            }

            if ((_uiDispatcher == null) || _uiDispatcher.HasThreadAccess)
            {
                action();
            }
            else
            {
                await _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).AsTask().ConfigureAwait(false);
            }
        }

        public static async Task InvokeOnUiAsync(Func<Task> action)
        {
            await InvokeOnUiAsync(async () =>
            {
                await action();
                return true;
            });
        }

        public static async Task<TResult> InvokeOnUiAsync<TResult>(Func<Task<TResult>> action)
        {
            if ((_uiDispatcher == null) || _uiDispatcher.HasThreadAccess)
            {
                return await action().ConfigureAwait(false);
            }

            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            await _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    taskCompletionSource.SetResult(await action());
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            });
            return await taskCompletionSource.Task;
        }

        private static CoreDispatcher _uiDispatcher;
    }
}