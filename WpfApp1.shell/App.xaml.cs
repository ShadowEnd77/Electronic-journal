using System.Configuration;
using System.Data;
using System.Windows;
using Prism.Unity;
using WpfApp1.shell.View;
//using WpfApp1.shell.ViewModel;

namespace WpfApp1.shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<MainMenuPage>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<LoginView>();
        }
    }
  }
