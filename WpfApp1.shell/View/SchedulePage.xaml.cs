using System.Windows.Controls;
using WpfApp1.shell.Model;
using WpfApp1.shell.ViewModel;

namespace WpfApp1.shell.View
{
    public partial class SchedulePage : UserControl
    {
        public SchedulePage(SchoolDbContext dbContext, int teacherId)
        {
            InitializeComponent(); // Убедитесь, что файл XAML существует
            DataContext = new SchedulePageViewModel(dbContext, teacherId);
        }
    }
}