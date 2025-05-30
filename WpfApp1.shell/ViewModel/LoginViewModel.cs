using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using WpfApp1.shell.Model;
using WpfApp1.shell.Model.Entities;

namespace WpfApp1.shell.ViewModel
{
    public class LoginViewModel : BindableBase
    {
        private readonly SchoolDbContext _dbContext;
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public int? CurrentAccountId { get; private set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand RegisterCommand { get; private set; }

        public event Action NavigateToMainMenu;

        public LoginViewModel(SchoolDbContext dbContext)
        {
            _dbContext = dbContext;
            LoginCommand = new DelegateCommand(ExecuteLogin);
            RegisterCommand = new DelegateCommand(RegisterUser);
        }

        private void ExecuteLogin()
        {
            try
            {
                var account = _dbContext.Accounts
                    .FirstOrDefault(a => a.Login == Username);

                if (account != null)
                {
                    // Хешируем введенный пароль
                    string hashedInputPassword = ComputeSha256Hash(Password);

                    // Сравниваем хеши
                    if (string.Equals(hashedInputPassword, account.Password, StringComparison.Ordinal))
                    {
                        CurrentAccountId = account.IdAccount;
                        NavigateToMainMenu?.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}