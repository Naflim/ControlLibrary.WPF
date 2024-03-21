using System.Windows.Input;

namespace Naflim.ControlLibrary.WPF.Core
{
    public class SimpleCommand<T> : ICommand
    {
        public SimpleCommand(Func<T?, bool>? canExecute = null, Action<T?>? execute = null)
        {
            CanExecuteDelegate = canExecute;
            ExecuteDelegate = execute;
        }

        public Func<T?, bool>? CanExecuteDelegate { get; }

        public Action<T?>? ExecuteDelegate { get; }

        public bool CanExecute(object? parameter)
        {
            var canExecute = CanExecuteDelegate;
            return canExecute is null || canExecute(parameter is T t ? t : default);
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object? parameter)
        {
            ExecuteDelegate?.Invoke(parameter is T t ? t : default);
        }
    }
}
