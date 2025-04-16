using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


public abstract class BaseNotifier : INotifyPropertyChanged
{
    
    public event PropertyChangedEventHandler PropertyChanged;

    
    protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(field, value))
            return;

        field = value;
        NotifyPropertyChanged(propertyName);
    }

    
    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
    protected void NotifyPropertyChanged(Expression<Func<object>> propertyExpression)
    {
        var propertyName = GetPropertyName(propertyExpression);
        NotifyPropertyChanged(propertyName);
    }

    
    private string GetPropertyName(Expression<Func<object>> propertyExpression)
    {
        var lambda = (MemberExpression)propertyExpression.Body;
        return lambda.Member.Name;
    }
}