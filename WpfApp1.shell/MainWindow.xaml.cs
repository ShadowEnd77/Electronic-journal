using System.Windows;
using System.Windows.Controls;
using WpfApp1.shell.View;
using WpfApp1.shell.Model;
using System.Linq;
using Prism.Ioc;
using WpfApp1.shell.ViewModel;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly IContainerProvider _containerProvider;
        private int _currentAccountId;

        public MainWindow(IContainerProvider containerProvider)
        {
            InitializeComponent();
            _containerProvider = containerProvider;
            ShowLogin();
        }

        public void ShowLogin()
        {
            var loginView = new LoginView(_containerProvider);
            loginView.ShowDialog();

            if (loginView.DataContext is LoginViewModel loginVM && loginVM.CurrentAccountId.HasValue)
            {
                SetCurrentUser(loginVM.CurrentAccountId.Value);
                NavigateTo<MainMenuPage>(); // Начальная страница после входа
            }
            else
            {
                Close(); // Закрыть приложение, если вход не выполнен
            }
        }

        public void SetCurrentUser(int accountId)
        {
            _currentAccountId = accountId;
        }

        private string GetUserRole()
        {
            using (var context = new SchoolDbContext())
            {
                var account = context.Accounts
                    .Where(a => a.IdAccount == _currentAccountId)
                    .FirstOrDefault();
                return account?.Role;
            }
        }

        private void NavigateTo<T>() where T : UserControl
        {
            UserControl view;
            if (typeof(T) == typeof(GradesPageStudent))
            {
                view = new GradesPageStudent(_currentAccountId);
            }
            else
            {
                view = _containerProvider.Resolve<T>();
            }
            MainContent.Content = view;
        }

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