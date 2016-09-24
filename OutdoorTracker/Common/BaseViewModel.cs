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

namespace OutdoorTracker.Common
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;

        protected bool IsInitialized { get; set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            protected set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected abstract Task InitializeInternal();

        public async Task Initialize()
        {
            if (IsInitialized)
            {
                return;
            }
            using (MarkBusy())
            {
                await InitializeInternal().ConfigureAwait(false);
            }
            IsInitialized = true;
        }

        protected async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            await DispatcherHelper.InvokeOnUIAsync(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }

        public IDisposable MarkBusy()
        {
            return new BusyState(this);
        }

        public virtual Task Leave()
        {
            return Task.CompletedTask;
        }

        private class BusyState : IDisposable
        {
            private static int _busyCounter = 0;

            private readonly BaseViewModel _baseViewModel;

            public BusyState(BaseViewModel baseViewModel)
            {
                Interlocked.Increment(ref _busyCounter);
                _baseViewModel = baseViewModel;
                baseViewModel.IsBusy = true;
            }

            public void Dispose()
            {
                int value = Interlocked.Decrement(ref _busyCounter);
                if (value == 0)
                {
                    _baseViewModel.IsBusy = false;
                }
            }
        }

        public virtual Task Suspending()
        {
            return Task.CompletedTask;
        }

        public virtual Task Resuming()
        {
            return Task.CompletedTask;
        }
    }
}