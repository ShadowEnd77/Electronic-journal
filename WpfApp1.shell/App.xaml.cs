using System.Configuration;
using System.Data;
using System.Windows;
using WpfApp1.shell.View;

namespace WpfApp1.shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell() => Container.Resolve<View.Shell>();
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }

}
