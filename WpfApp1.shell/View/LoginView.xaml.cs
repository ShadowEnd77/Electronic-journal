using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.shell.View
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            var viewModel = new LoginViewModel();
            viewModel.NavigateToMainMenu += OnNavigateToMainMenu;
            DataContext = viewModel;
        }

        private void OnNavigateToMainMenu()
        {
            //// Проверяем, открыто ли уже окно MainMenuPage
            //var mainMenuPage = Application.Current.Windows.OfType<MainMenuPage>().FirstOrDefault();
            //if (mainMenuPage == null)
            //{
            //    mainMenuPage = new MainMenuPage();
            //    mainMenuPage.Show(); // Открываем новое окно
            //}
            var mainMenuPage = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainMenuPage == null)
            {
                mainMenuPage = new MainWindow();
                mainMenuPage.Show(); // Открываем новое окно
            }

            this.Close(); // Закрываем текущее окно (если необходимо)
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LoginViewModel;
            if (viewModel != null)
            {
                // Получаем пароль из PasswordBox и устанавливаем его в ViewModel
                viewModel.Password = PasswordBox.Password;

                // Вызываем команду входа без аргументов
                //viewModel.LoginCommand.Execute();
            }
        }
    }
}
