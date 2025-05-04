using System;
using System.Windows.Input;

namespace FinancialTracker.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            if (parameter == null && default(T) == null)
                return _canExecute(default);

            if (parameter == null)
                return false;

            if (parameter is T tParam)
                return _canExecute(tParam);

            // Próbáljunk meg konvertálni
            try
            {
                var convertedParam = (T)Convert.ChangeType(parameter, typeof(T));
                return _canExecute(convertedParam);
            }
            catch
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            if (parameter is T tParam)
            {
                _execute(tParam);
                return;
            }

            // Próbáljunk meg konvertálni
            if (parameter != null)
            {
                try
                {
                    var convertedParam = (T)Convert.ChangeType(parameter, typeof(T));
                    _execute(convertedParam);
                    return;
                }
                catch
                {
                    // Nem sikerült a konverzió
                }
            }

            // Ha null vagy nem sikerült a konverzió, használjuk az alapértelmezett értéket
            _execute(default);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}