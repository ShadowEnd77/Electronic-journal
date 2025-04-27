using System.Windows;
using Prism.Ioc;
using System.Linq;
using System.Windows.Controls;

namespace WpfApp1.shell.View
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private readonly IContainerProvider _containerProvider;

        public LoginView(IContainerProvider containerProvider)
        {
            InitializeComponent();
            _containerProvider = containerProvider;

            var viewModel = new LoginViewModel();
            viewModel.NavigateToMainMenu += OnNavigateToMainMenu;
            DataContext = viewModel;
        }

        private void OnNavigateToMainMenu()
        {
            this.Close();

            var mainMenuPage = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainMenuPage == null)
            {
                mainMenuPage = _containerProvider.Resolve<MainWindow>();
                mainMenuPage.Show(); // Открываем новое окно
            }

            //this.Close(); // Закрываем текущее окно (если необходимо)
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