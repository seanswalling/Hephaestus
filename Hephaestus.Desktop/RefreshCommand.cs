using System;
using System.Windows.Input;

namespace Hephaestus.Desktop
{
    public class RefreshCommand : ICommand
    {
        private readonly Action[] _sideEffects;

        public RefreshCommand(params Action[] sideEffects)
        {
            _sideEffects = sideEffects;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            foreach (var sideEffect in _sideEffects)
                sideEffect();
        }

        public event EventHandler? CanExecuteChanged;
    }
}