using System;
using System.Windows.Input;

namespace OutdoorTraker.Common
{
	public abstract class BaseCommand : ICommand
	{
		/// <summary>
		///     Method used to raise the <see cref="CanExecuteChanged" /> event
		///     to indicate that the return value of the <see cref="CanExecute" />
		///     method has changed.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public abstract bool CanExecute(object parameter);
		public abstract void Execute(object parameter);

		/// <summary>
		///     Raised when RaiseCanExecuteChanged is called.
		/// </summary>
		public event EventHandler CanExecuteChanged;
	}
}