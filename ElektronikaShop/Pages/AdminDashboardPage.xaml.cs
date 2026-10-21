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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElektronikaShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminDashboardPage.xaml
    /// </summary>
    public partial class AdminDashboardPage : Page
    {
        private readonly MainWindow _mainWindow;

        public AdminDashboardPage(
            MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
        }

        private void ProductsButton_Click(
            object sender,
    RoutedEventArgs e)
        {
            _mainWindow.OpenPage(
                new ProductManagementPage(_mainWindow),
                "Управление товарами");
        }

        private void CategoriesButton_Click(
            object sender,
    RoutedEventArgs e)
        {
            _mainWindow.OpenPage(
                new CategoryManagementPage(_mainWindow),
                "Управление категориями");
        }

        private void OrdersButton_Click(
            object sender,
    RoutedEventArgs e)
        {
            _mainWindow.OpenPage(
                new OrderManagementPage(_mainWindow),
                "Управление заказами");
        }

        private void UsersButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageTextBlock.Text =
                "Раздел управления пользователями будет добавлен далее.";
        }

        private void StatisticsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageTextBlock.Text =
                "Раздел графиков будет добавлен далее.";
        }

        private void ReportsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageTextBlock.Text =
                "Раздел отчетов будет добавлен далее.";
        }
    }
}
