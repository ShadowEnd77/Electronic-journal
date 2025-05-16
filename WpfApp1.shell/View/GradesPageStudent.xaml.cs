using System.Windows;
using System.Windows.Controls;
using WpfApp1.shell.Model.Entities;
using WpfApp1.shell.ViewModel;

namespace WpfApp1.shell.View
{
    public partial class GradesPageStudent : UserControl
    {
        public GradesPageStudent(int accountId)
        {
            InitializeComponent();
            DataContext = new GradesPageStudentViewModel(accountId);
        }

    }
}