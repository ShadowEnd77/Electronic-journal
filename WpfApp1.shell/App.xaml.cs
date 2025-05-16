using System.Windows;
using Microsoft.EntityFrameworkCore;
using Unity.Injection;
using Prism.Ioc;
using Prism.Unity;
using WpfApp1.shell.Model;
using WpfApp1.shell.View;
using WpfApp1.shell.ViewModel;

namespace WpfApp1.shell
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Регистрация страниц и ViewModel
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<MainMenuPage, MainMenuPageViewModel>();
            containerRegistry.RegisterForNavigation<SchedulePage, SchedulePageViewModel>();
            containerRegistry.RegisterForNavigation<SchedulePageStudent, SchedulePageStudentViewModel>();
            containerRegistry.RegisterForNavigation<GradesPage, GradesPageViewModel>();
            containerRegistry.Register<GradesPageStudent>(); // Не регистрируем для навигации, так как создаем вручную

            // Регистрация SchoolDbContext
            containerRegistry.GetContainer().RegisterType<SchoolDbContext>(new InjectionFactory(c =>
                new SchoolDbContext(new DbContextOptionsBuilder<SchoolDbContext>()
                    .UseNpgsql("Host=localhost;Port=5432;Database=EJ;Username=postgres;Password=1111")
                    .Options)));
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>(); // Убрано mainWindow.ShowLogin()
        }
    }
}