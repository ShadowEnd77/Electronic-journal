using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.shell.ViewModel;

namespace WpfApp1.shell.View
{
    public partial class GradesPage : UserControl
    {
        public GradesPage()
        {
            InitializeComponent();
            DataContext = new GradesPageViewModel();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) || !int.TryParse(e.Text, out int num) || num < 1 || num > 5)
            {
                e.Handled = true;
                return;
            }

            var textBox = (TextBox)sender;
            var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            if (!int.TryParse(fullText, out int result) || result < 1 || result > 5)
            {
                e.Handled = true;
            }
        }
    }
}