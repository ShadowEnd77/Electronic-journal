using System.Windows.Controls;
using WpfApp1.shell.Model;
using WpfApp1.shell.ViewModel;

namespace WpfApp1.shell.View
{
    public partial class SchedulePageStudent : UserControl
    {
        public SchedulePageStudent(SchoolDbContext dbContext, int studentId)
        {
            InitializeComponent();
            DataContext = new SchedulePageStudentViewModel(dbContext, studentId);
        }
    }
}