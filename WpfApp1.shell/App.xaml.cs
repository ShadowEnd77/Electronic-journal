using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using Prism.Unity;
using WpfApp1.shell.View;
using WpfApp1.shell.ViewModel;
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
            containerRegistry.RegisterForNavigation<MainMenuPage, MainMenuPageViewModel>();
            containerRegistry.RegisterForNavigation<SchedulePage, SchedulePageViewModel>();
            containerRegistry.RegisterForNavigation<GradesPage, GradesPageViewModel>();

            containerRegistry.RegisterForNavigation<LoginView>();
            containerRegistry.RegisterForNavigation<MainMenuPage>();
            containerRegistry.RegisterForNavigation<SchedulePage>();
            containerRegistry.RegisterForNavigation<GradesPage>();
        }

        protected override Window CreateShell()
        {
            var mainWindow = Container.Resolve<MainWindow>();
            mainWindow.ShowLogin(); // Показываем окно входа при запуске приложения
            return mainWindow; // Возвращаем главное окно
        }
    }
}
