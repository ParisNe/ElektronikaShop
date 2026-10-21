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
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly int _userId;

        public CartPage(
            MainWindow mainWindow,
            int userId)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            _userId = userId;

            LoadCart();
        }

        private void LoadCart()
        {
            var cartItems =
                (from cart in Core.Context.CartItems
                 join product in Core.Context.Products
                 on cart.ProductId equals product.ProductId
                 where cart.UserId == _userId
                 select new CartViewModel
                 {
                     CartItemId =
                         cart.CartItemId,

                     ProductId =
                         product.ProductId,

                     ProductName =
                         product.Name,

                     Price =
                         product.Price,

                     Quantity =
                         cart.Quantity,

                     Total =
                         product.Price *
                         cart.Quantity
                 })
                .ToList();

            CartDataGrid.ItemsSource =
                cartItems;

            decimal total =
                cartItems.Sum(x => x.Total);

            TotalTextBlock.Text =
                $"Итого: {total:N2} ₽";
        }

        private void DeleteButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var selectedItem =
                CartDataGrid.SelectedItem
                as CartViewModel;

            if (selectedItem == null)
            {
                MessageBox.Show(
                    "Выберите товар.",
                    "Корзина",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var cartItem =
                Core.Context.CartItems
                .FirstOrDefault(x =>
                    x.CartItemId ==
                    selectedItem.CartItemId);

            if (cartItem != null)
            {
                Core.Context.CartItems.Remove(cartItem);
                Core.Context.SaveChanges();
            }

            LoadCart();
        }

        private void RefreshButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoadCart();
        }

        private void CheckoutButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            var cartExists =
                Core.Context.CartItems.Any(x =>
                    x.UserId == _userId);

            if (!cartExists)
            {
                MessageBox.Show(
                    "Корзина пуста.",
                    "Оформление заказа",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            _mainWindow.OpenPage(
                new CheckoutPage(
                    _mainWindow,
                    _userId),
                "Оформление заказа");
        }
    }

    public class CartViewModel
    {
        public int CartItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Total { get; set; }
    }
}
