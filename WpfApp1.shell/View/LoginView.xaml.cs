using Prism.Ioc;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.shell.ViewModel;

namespace WpfApp1.shell.View
{
    public partial class LoginView : Window
    {
        private readonly IContainerProvider _containerProvider;

        public LoginView(IContainerProvider containerProvider)
        {
            InitializeComponent();
            _containerProvider = containerProvider;

            var viewModel = containerProvider.Resolve<LoginViewModel>();
            viewModel.NavigateToMainMenu += OnNavigateToMainMenu;
            DataContext = viewModel;
        }

        private void OnNavigateToMainMenu()
        {
            var mainMenuPage = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainMenuPage == null)
            {
                mainMenuPage = _containerProvider.Resolve<MainWindow>();
                mainMenuPage.Show();
            }
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LoginViewModel;
            if (viewModel != null)
            {
                viewModel.Password = PasswordBox.Password;
                viewModel.LoginCommand.Execute();
            }
        }
    }
}