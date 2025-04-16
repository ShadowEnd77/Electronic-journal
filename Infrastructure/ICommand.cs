using System;
using System.Windows.Input;

public class DelegateCommand : ICommand
{
    private readonly Action<object> _executeMethod;
    private readonly Predicate<object> _canExecuteMethod;

    public DelegateCommand(Action<object> executeMethod)
        : this(executeMethod, null)
    {
    }

    public DelegateCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
    {
        if (executeMethod == null)
            throw new ArgumentNullException(nameof(executeMethod));

        _executeMethod = executeMethod;
        _canExecuteMethod = canExecuteMethod;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
        return _canExecuteMethod == null || _canExecuteMethod(parameter);
    }

    public void Execute(object parameter)
    {
        _executeMethod(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
