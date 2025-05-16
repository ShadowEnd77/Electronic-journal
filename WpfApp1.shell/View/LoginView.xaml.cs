using Prism.Ioc;
using System;
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
            _containerProvider = containerProvider ?? throw new ArgumentNullException(nameof(containerProvider));

            var viewModel = _containerProvider.Resolve<LoginViewModel>();
            viewModel.NavigateToMainMenu += OnNavigateToMainMenu;
            DataContext = viewModel;

            PasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is LoginViewModel vm)
                {
                    vm.Password = PasswordBox.Password;
                }
            };
        }

        private void OnNavigateToMainMenu()
        {
            Close(); // Закрываем окно логина, MainWindow обработает остальное
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.LoginCommand.Execute();
            }
        }
    }
}