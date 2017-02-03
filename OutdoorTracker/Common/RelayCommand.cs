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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace OutdoorTracker.Common
{
    /// <summary>
    ///     A command whose sole purpose is to relay its functionality
    ///     to other objects by invoking delegates.
    ///     The default return value for the CanExecute method is 'true'.
    ///     <see cref="RaiseCanExecuteChanged" /> needs to be called whenever
    ///     <see cref="CanExecute" /> is expected to return a different value.
    /// </summary>
    public class RelayCommand : BaseCommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;
        private bool? _enabledOverride;

        /// <summary>
        ///     Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        ///     Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool? EnabledOverride
        {
            get { return _enabledOverride; }
            set
            {
                _enabledOverride = value;
                RaiseCanExecuteChanged();
            }
        }

        public Visibility Visibility
        {
            get { return CanExecute() ? Visibility.Visible : Visibility.Collapsed; }
        }

        public bool IsEnabled
        {
            get { return CanExecute(); }
        }

        /// <summary>
        ///     Determines whether this <see cref="RelayCommand" /> can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public override bool CanExecute(object parameter = null)
        {
            if (_enabledOverride.HasValue)
            {
                return _enabledOverride.Value;
            }
            return _canExecute == null ? true : _canExecute();
        }

        /// <summary>
        ///     Executes the <see cref="RelayCommand" /> on the current command target.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public override void Execute(object parameter = null)
        {
            _execute();
        }

        public override void RaiseCanExecuteChanged()
        {
            base.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(Visibility));
        }
    }
}