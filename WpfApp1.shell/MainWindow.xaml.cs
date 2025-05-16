using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using WpfApp1.shell.Model;
using WpfApp1.shell.View;
using WpfApp1.shell.ViewModel;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly IContainerProvider _containerProvider;
        private int _currentAccountId;
        private readonly SchoolDbContext _dbContext;

        public MainWindow(IContainerProvider containerProvider, SchoolDbContext dbContext)
        {
            InitializeComponent();
            _containerProvider = containerProvider;
            _dbContext = dbContext;
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
            UpdateUserInfo();
        }

        private void UpdateUserInfo()
        {
            using (var context = new SchoolDbContext())
            {
                var account = context.Accounts
                    .Include(a => a.Teacher)
                    .Include(a => a.Student)
                    .FirstOrDefault(a => a.IdAccount == _currentAccountId);

                if (account != null)
                {
                    string fullName = string.Empty;
                    string roleDisplay = account.Role switch
                    {
                        "Учитель" => "👨‍🏫 Учитель",
                        "Ученик" => "👨‍🎓 Ученик",
                        _ => account.Role
                    };

                    if (account.Role == "Учитель" && account.Teacher != null)
                    {
                        fullName = $"{account.Teacher.LastName} {account.Teacher.FirstName[0]}. {account.Teacher.Patronymic[0]}.";
                    }
                    else if (account.Role == "Ученик" && account.Student != null)
                    {
                        fullName = $"{account.Student.LastName} {account.Student.FirstName[0]}. {account.Student.Patronymic[0]}.";
                    }

                    UserInfoTextBlock.Text = $"{fullName} | {roleDisplay}";
                }
            }
        }

        private void NavigateTo<T>() where T : UserControl
        {
            UserControl view;
            if (typeof(T) == typeof(GradesPageStudent))
            {
                view = new GradesPageStudent(_currentAccountId);
            }
            else if (typeof(T) == typeof(SchedulePageStudent))
            {
                // Получаем ID студента из аккаунта
                var studentId = _dbContext.Students
                    .FirstOrDefault(s => s.IdAccount == _currentAccountId)?.IdStudent;

                if (studentId.HasValue)
                {
                    view = new SchedulePageStudent(_dbContext, studentId.Value);
                }
                else
                {
                    // Обработка случая, когда студент не найден
                    MessageBox.Show("Студент не найден");
                    return;
                }
            }
            else
            {
                view = _containerProvider.Resolve<T>();
            }
            MainContent.Content = view;
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
        private void
ScheduleButton_Click(object sender, RoutedEventArgs e)
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