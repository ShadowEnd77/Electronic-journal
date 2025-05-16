using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
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
            UserControl view = null;
            string role = GetUserRole();

            try
            {
                if (typeof(T) == typeof(GradesPageStudent))
                {
                    view = new GradesPageStudent(_currentAccountId);
                }
                else if (typeof(T) == typeof(SchedulePageStudent))
                {
                    var studentId = _dbContext.Students
                        .FirstOrDefault(s => s.IdAccount == _currentAccountId)?.IdStudent;

                    if (studentId.HasValue)
                    {
                        view = new SchedulePageStudent(_dbContext, studentId.Value);
                    }
                    else
                    {
                        MessageBox.Show("Студент не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else if (typeof(T) == typeof(SchedulePage))
                {
                    var teacherId = _dbContext.Teachers
                        .FirstOrDefault(t => t.IdAccount == _currentAccountId)?.IdTeacher;

                    if (teacherId.HasValue)
                    {
                        view = new SchedulePage(_dbContext, teacherId.Value);
                    }
                    else
                    {
                        MessageBox.Show("Преподаватель не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    view = _containerProvider.Resolve<T>();
                }

                if (view != null)
                {
                    MainContent.Content = view;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переходе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            string role = GetUserRole();
            try
            {
                if (role == "Учитель")
                {
                    var teacherId = _dbContext.Teachers
                        .FirstOrDefault(t => t.IdAccount == _currentAccountId)?.IdTeacher;

                    if (teacherId.HasValue)
                    {
                        MainContent.Content = new SchedulePage(_dbContext, teacherId.Value);
                    }
                    else
                    {
                        MessageBox.Show("Преподаватель не найден");
                    }
                }
                else if (role == "Ученик")
                {
                    var studentId = _dbContext.Students
                        .FirstOrDefault(s => s.IdAccount == _currentAccountId)?.IdStudent;

                    if (studentId.HasValue)
                    {
                        MainContent.Content = new SchedulePageStudent(_dbContext, studentId.Value);
                    }
                    else
                    {
                        MessageBox.Show("Студент не найден");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
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