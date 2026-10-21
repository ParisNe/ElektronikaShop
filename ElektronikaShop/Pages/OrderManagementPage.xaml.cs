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
    /// Логика взаимодействия для OrderManagementPage.xaml
    /// </summary>
    public partial class OrderManagementPage : Page
    {
        private readonly MainWindow _mainWindow;

        public OrderManagementPage(
            MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            LoadOrders();
        }

        private void LoadOrders()
        {
            OrdersDataGrid.ItemsSource =
                Core.Context.Orders
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        private void ChangeStatusButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var selectedOrder =
                OrdersDataGrid.SelectedItem
                as Orders;

            if (selectedOrder == null)
            {
                ShowMessage(
                    "Выберите заказ.");

                return;
            }

            var selectedStatus =
                StatusComboBox.SelectedItem
                as ComboBoxItem;

            if (selectedStatus == null)
            {
                ShowMessage(
                    "Выберите новый статус.");

                return;
            }

            string status =
                selectedStatus.Content.ToString();

            selectedOrder.Status = status;

            Core.Context.SaveChanges();

            LoadOrders();

            ShowMessage(
                "Статус заказа изменен.");
        }

        private void RefreshButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoadOrders();

            ShowMessage("Данные обновлены.");
        }

        private void ShowMessage(string message)
        {
            MessageTextBlock.Text = message;
        }
    }
}
