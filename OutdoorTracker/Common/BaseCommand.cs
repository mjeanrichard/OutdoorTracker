using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OutdoorTracker.Common
{
	public abstract class BaseCommand : ICommand, INotifyPropertyChanged
	{
		/// <summary>
		///     Method used to raise the <see cref="CanExecuteChanged" /> event
		///     to indicate that the return value of the <see cref="CanExecute" />
		///     method has changed.
		/// </summary>
		public virtual void RaiseCanExecuteChanged()
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

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}