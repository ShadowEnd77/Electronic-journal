using System.Windows;
using System.Windows.Controls;
using WpfApp1.shell.View;
using WpfApp1.shell.Model; // Для доступа к сущностям
using System.Linq;
using System.Diagnostics;
using WpfApp1.shell.ViewModel;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly IContainerProvider _containerProvider;

        // Текущий ID аккаунта пользователя
        private int _currentAccountId;

        public MainWindow(IContainerProvider containerProvider)
        {
            InitializeComponent();
            _containerProvider = containerProvider;
        }

        public void ShowLogin()
        {
            var loginView = new LoginView(_containerProvider);
            loginView.ShowDialog();

            // После закрытия окна, есть способ получить ID
            if (loginView.DataContext is LoginViewModel loginVM && loginVM.CurrentAccountId.HasValue)
            {
                SetCurrentUser(loginVM.CurrentAccountId.Value);
            }
        }

        // Метод для установки текущего пользователя по ID
        public void SetCurrentUser(int accountId)
        {
            _currentAccountId = accountId;
        }

        // Вспомогательный метод для получения роли по ID аккаунта
        private string GetUserRole()
        {
            using (var context = new SchoolDbContext())
            {
                var account = context.Accounts
                    .Where(a => a.IdAccount == _currentAccountId)
                    .FirstOrDefault();

                if (account == null)
                {
                    //MessageBox.Show($"Аккаунт с ID {_currentAccountId} не найден.", "Диагностика");
                    return null;
                }
                else
                {
                    //MessageBox.Show($"Найден аккаунт: {account.IdAccount}, роль: {account.Role}", "Диагностика");
                    return account.Role;
                }
            }
        }

        private void NavigateTo<T>() where T : UserControl
        {
            var view = _containerProvider.Resolve<T>();
            MainContent.Content = view;
        }

        private void OnNavigateToMainMenu()
        {
            NavigateTo<MainMenuPage>();
        }

        // Обновленные методы кнопок с проверкой роли
        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            string role = GetUserRole();

            if (role == "Учитель")
            {
                NavigateTo<SchedulePage>();
            }
            else if (role == "Ученик")
            {
                NavigateTo<SchedulePageStudent>();
            }
            else
            {
                MessageBox.Show("Роль пользователя не определена.", "Ошибка");
            }
        }

        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            string role = GetUserRole();

            if (role == "Учитель")
            {
                NavigateTo<GradesPage>();
            }
            else if (role == "Ученик")
            {
                NavigateTo<GradesPageStudent>();
            }
            else
            {
                MessageBox.Show("Роль пользователя не определена.", "Ошибка");
            }
        }

        private void SchoolButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo<MainMenuPage>();
        }
    }
}