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

namespace OutdoorTracker.Common
{
	public class ParameterCommand<TParam> : BaseCommand where TParam : class
	{
		private readonly Action<TParam> _execute;
		private readonly Func<TParam, bool> _canExecute;

		public ParameterCommand(Action<TParam> execute, Func<TParam, bool> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public override bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute(parameter as TParam);
		}

		public override void Execute(object parameter)
		{
			_execute(parameter as TParam);
		}
	}
}