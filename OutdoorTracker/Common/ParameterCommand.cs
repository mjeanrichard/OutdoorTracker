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