using System;
using System.Windows.Input;

namespace Hephaestus.Desktop
{
    public class ChangeContextCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Predicate<object> _canExecute;

        public ChangeContextCommand(Action execute) : this(execute, (_) => true) { }

        public ChangeContextCommand(Action execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        public event EventHandler? CanExecuteChanged;
    }
}