using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

public class LoginViewModel : BindableBase
{
    private string _username;
    private string _password;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public DelegateCommand LoginCommand { get; private set; }

    // Событие для уведомления о необходимости навигации
    public event Action NavigateToMainMenu;

    public LoginViewModel()
    {
        LoginCommand = new DelegateCommand(ExecuteLogin);
    }

    private void ExecuteLogin()
    {
        //Здесь должна быть ваша логика проверки логина и пароля.
        //Для примера просто проверим на "admin" и "password".
        if (Username == "123" && Password == "123")
        {
            // Вызываем событие для навигации на главную страницу
            NavigateToMainMenu?.Invoke();
        }
        else
        {
            // Здесь можно добавить обработку ошибок, например, показать сообщение об ошибке.
            MessageBox.Show("Неверный логин или пароль.");
        }
    }
}