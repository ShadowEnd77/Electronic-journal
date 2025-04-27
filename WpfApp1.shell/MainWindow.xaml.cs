using System.Windows;
using System.Windows.Controls;
using WpfApp1.shell.View;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly IContainerProvider _containerProvider;

        public MainWindow(IContainerProvider containerProvider)
        {
            InitializeComponent();
            _containerProvider = containerProvider;

            // Загружаем начальную страницу (Login)
            //NavigateTo<LoginView>();
        }
        public void ShowLogin()
        {
            var loginView = new LoginView(_containerProvider);
            loginView.ShowDialog(); // Открываем как модальное окно
        }

        private void NavigateTo<T>() where T : UserControl // Убедитесь, что это UserControl
        {
            var view = _containerProvider.Resolve<T>();
            MainContent.Content = view;
        }

        private void OnNavigateToMainMenu()
        {
            NavigateTo<MainMenuPage>();
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo<SchedulePage>();
        }

        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo<GradesPage>();
        }

        private void SchoolButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo<MainMenuPage>();
        }
    }
}