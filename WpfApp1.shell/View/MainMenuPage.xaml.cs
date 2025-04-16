using System.Windows;


namespace WpfApp1.shell.View
{
    /// <summary>
    /// Логика взаимодействия для Shell.xaml
    /// </summary>
    public partial class MainMenuPage : Window
    {
        public MainMenuPage()
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
            GradesPage gradesPage = new GradesPage();
            gradesPage.Show();
            this.Close();
        }

        private void SchoolButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
