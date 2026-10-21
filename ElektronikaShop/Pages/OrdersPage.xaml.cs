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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly int _userId;

        public OrdersPage(
            MainWindow mainWindow,
            int userId)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            _userId = userId;

            LoadOrders();
        }

        private void LoadOrders()
        {
            OrdersDataGrid.ItemsSource =
                Core.Context.Orders
                .Where(x => x.UserId == _userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }
    }
}
