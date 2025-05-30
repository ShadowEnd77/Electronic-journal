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

        // Метод для регистрации нового пользователя
        private void RegisterUser()
        {
            try
            {
                var existingUser = _dbContext.Accounts.FirstOrDefault(a => a.Login == Username);
                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string hashedPassword = ComputeSha256Hash(Password);

                var newAccount = new Account
                {
                    Login = Username,
                    Password = hashedPassword
                };

                _dbContext.Accounts.Add(newAccount);
                _dbContext.SaveChanges();

                MessageBox.Show("Пользователь успешно зарегистрирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для вычисления SHA-256 хеша строки
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Преобразуем строку в байты
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Конвертируем байты в строку в шестнадцатеричном формате
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // 'x2' — для нижнего регистра
                }
                Console.WriteLine(builder.ToString());
                return builder.ToString();
            }
        }
    }
}