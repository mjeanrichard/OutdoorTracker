using System;

using Windows.UI.Xaml;

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
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			_execute = execute;
			_canExecute = canExecute;
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
			return _canExecute == null ? true : _canExecute();
		}

		public Visibility Visibility
		{
			get { return CanExecute() ? Visibility.Visible : Visibility.Collapsed; }
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
			OnPropertyChanged(nameof(Visibility));
		}
	}
}