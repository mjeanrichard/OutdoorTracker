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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp;

namespace OutdoorTracker.Common
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private int _busyCounter;
        private string _busyText;

        protected bool IsInitialized { get; set; }

        protected Task DataLoaderTask { get; private set; }

        public bool IsBusy => _busyCounter != 0;

        public string BusyText
        {
            get { return _busyText; }
            set
            {
                _busyText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual Task InitializeInternalAsync()
        {
            return Task.CompletedTask;
        }

        public async Task Initialize()
        {
            if (IsInitialized)
            {
                return;
            }
            await InitializeInternalAsync().ConfigureAwait(false);
            DataLoaderTask = Task.Run(async () => await RunBusy(LoadData, string.Empty));
            IsInitialized = true;
        }

        public virtual Task Refresh()
        {
            return Task.CompletedTask;
        }

        protected virtual Task LoadData()
        {
            return Task.CompletedTask;
        }

        protected async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))).ConfigureAwait(false);
        }

        public IDisposable MarkBusy(string message = null)
        {
            return new BusyState(this, message);
        }

        public async Task RunBusy(Func<Task> action, string message)
        {
            using (MarkBusy(message))
            {
                await Task.Run(async () => await action().ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        public virtual Task Leave()
        {
            return Task.CompletedTask;
        }

        public virtual Task Suspending()
        {
            return Task.CompletedTask;
        }

        public virtual Task Resuming()
        {
            return Task.CompletedTask;
        }

        private void DecrementBusyCounter()
        {
            Interlocked.Decrement(ref _busyCounter);
            OnPropertyChanged(nameof(IsBusy));
        }

        private void IncrementBusyCounter()
        {
            Interlocked.Increment(ref _busyCounter);
            OnPropertyChanged(nameof(IsBusy));
        }

        private class BusyState : IDisposable
        {
            private readonly BaseViewModel _baseViewModel;
            private readonly string _oldMessage;

            public BusyState(BaseViewModel baseViewModel, string message = null)
            {
                _baseViewModel = baseViewModel;

                _oldMessage = baseViewModel.BusyText;
                if (message != null)
                {
                    _baseViewModel.BusyText = message;
                }
                _baseViewModel.IncrementBusyCounter();
            }

            public void Dispose()
            {
                _baseViewModel.DecrementBusyCounter();
                _baseViewModel.BusyText = _oldMessage;
            }
        }
    }
}