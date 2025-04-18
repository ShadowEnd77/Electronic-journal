using System.Windows;
using WpfApp1.shell.View;

namespace WpfApp1
{
    public partial class MainWindow :Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Загружаем начальную страницу
            //MainContent.Content = new MainMenuPage();
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SchedulePage();
        }

        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new GradesPage();
        }

        private void SchoolButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new MainMenuPage();
        }
    }
}