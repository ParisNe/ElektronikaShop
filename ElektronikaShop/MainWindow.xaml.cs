using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ElektronikaShop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += (s, args) =>
            {
                DateTimeNow.Text =
                    DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            };

            timer.Start();
        }

        public void OpenPage(object page, string title)
        {
            PageTitle.Text = title;
            BackButton.Visibility = Visibility.Visible;
            MainFrame.Navigate(page);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Смена темы будет подключена на следующем этапе.",
                "Информация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(
            object sender,
            CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите выйти?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
