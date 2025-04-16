using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.shell.View
{
    /// <summary>
    /// Логика взаимодействия для GradesPage.xaml
    /// </summary>
    public partial class GradesPage : Window
    {
        public GradesPage()
        {
            InitializeComponent();
        }
        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            SchedulePage schedulePage = new SchedulePage();
            schedulePage.Show();
            this.Close();
        }

        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SchoolButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuPage mainmenuPage = new MainMenuPage();
            mainmenuPage.Show();
            this.Close();
        }
    }
}
