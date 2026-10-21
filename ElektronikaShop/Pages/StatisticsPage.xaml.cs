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
    /// Логика взаимодействия для StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        private readonly MainWindow _mainWindow;

        public StatisticsPage(
            MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            LoadStatistics();
        }

        private void LoadStatistics()
        {
            UsersCountTextBlock.Text =
                Core.Context.Users.Count(
                    x => x.IsActive == true)
                .ToString();

            ProductsCountTextBlock.Text =
                Core.Context.Products.Count(
                    x => x.IsActive == true)
                .ToString();

            OrdersCountTextBlock.Text =
                Core.Context.Orders.Count()
                .ToString();

            decimal revenue =
                Core.Context.Orders
                .Where(x =>
                    x.Status != "Отменен")
                .Sum(x => (decimal?)x.TotalAmount)
                ?? 0;

            RevenueTextBlock.Text =
                $"{revenue:N2} ₽";

            LoadCategoryStatistics();
            LoadRecentOrders();
        }

        private void LoadCategoryStatistics()
        {
            var categoryStatistics =
                (from item in Core.Context.OrderItems
                 join order in Core.Context.Orders
                 on item.OrderId equals order.OrderId
                 join product in Core.Context.Products
                 on item.ProductId equals product.ProductId
                 join category in Core.Context.Categories
                 on product.CategoryId equals
                    category.CategoryId
                 where order.Status != "Отменен"
                 group new
                 {
                     item.Quantity,
                     Total =
                         item.Quantity *
                         item.UnitPrice
                 }
                 by category.Name
                 into categoryGroup
                 select new CategoryStatisticsViewModel
                 {
                     CategoryName =
                         categoryGroup.Key,

                     Quantity =
                         categoryGroup.Sum(
                             x => x.Quantity),

                     Amount =
                         categoryGroup.Sum(
                             x => x.Total)
                 })
                .OrderByDescending(x => x.Amount)
                .ToList();

            CategoriesDataGrid.ItemsSource =
                categoryStatistics;
        }

        private void LoadRecentOrders()
        {
            OrdersDataGrid.ItemsSource =
                Core.Context.Orders
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .ToList();
        }
    }

    public class CategoryStatisticsViewModel
    {
        public string CategoryName { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }
    }
}
