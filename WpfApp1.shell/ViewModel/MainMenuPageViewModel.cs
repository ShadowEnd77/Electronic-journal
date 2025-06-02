using Prism.Commands;
using Prism.Mvvm;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace WpfApp1.shell.ViewModel
{
    public class MainMenuPageViewModel : BindableBase
    {
        public DelegateCommand OpenMapCommand { get; private set; }
        public DelegateCommand OpenWebsiteCommand { get; private set; }

        public MainMenuPageViewModel()
        {
            OpenMapCommand = new DelegateCommand(OpenMap);
            OpenWebsiteCommand = new DelegateCommand(OpenWebsite);
        }

        private void OpenMap()
        {
            // Формируем URL для Яндекс.Карт (можно заменить на Google Maps или другой сервис)
            string address = "Вологда, улица Кирова, 35";
            string url = $"https://yandex.ru/maps/?text={System.Web.HttpUtility.UrlEncode(address)}";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Не удалось открыть карту. Проверьте подключение к интернету.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void OpenWebsite()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://lic32-vologda-r19.gosweb.gosuslugi.ru/",
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Не удалось открыть сайт. Проверьте подключение к интернету.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

    }
}