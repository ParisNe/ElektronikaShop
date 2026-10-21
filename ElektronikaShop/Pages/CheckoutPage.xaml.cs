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
    /// Логика взаимодействия для CheckoutPage.xaml
    /// </summary>
    public partial class CheckoutPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly int _userId;

        private decimal _totalAmount;

        public CheckoutPage(
            MainWindow mainWindow,
            int userId)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            _userId = userId;

            LoadTotal();
        }

        private void LoadTotal()
        {
            var cartItems =
                from cart in Core.Context.CartItems
                join product in Core.Context.Products
                on cart.ProductId equals product.ProductId
                where cart.UserId == _userId
                select new
                {
                    Quantity = cart.Quantity,
                    Price = product.Price
                };

            _totalAmount =
                cartItems.Sum(x =>
                    x.Quantity * x.Price);

            TotalTextBlock.Text =
                $"{_totalAmount:N2} ₽";
        }

        private void CreateOrderButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string address =
                AddressTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(address))
            {
                MessageTextBlock.Text =
                    "Введите адрес доставки.";

                return;
            }

            var cartItems =
                (from cart in Core.Context.CartItems
                 join product in Core.Context.Products
                 on cart.ProductId equals product.ProductId
                 where cart.UserId == _userId
                 select new
                 {
                     Cart = cart,
                     Product = product
                 })
                .ToList();

            if (!cartItems.Any())
            {
                MessageTextBlock.Text =
                    "Корзина пуста.";

                return;
            }

            MessageBoxResult result =
                MessageBox.Show(
                    "Подтвердить оформление заказа?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            string orderNumber =
                "ES-" +
                DateTime.Now.ToString(
                    "yyyyMMddHHmmss");

            var order = new Orders
            {
                UserId = _userId,
                OrderNumber = orderNumber,
                Status = "Новый",
                DeliveryAddress = address,
                TotalAmount = _totalAmount,
                CreatedAt = DateTime.Now
            };

            Core.Context.Orders.Add(order);
            Core.Context.SaveChanges();

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItems
                {
                    OrderId = order.OrderId,
                    ProductId =
                        item.Product.ProductId,
                    Quantity = item.Cart.Quantity,
                    UnitPrice = item.Product.Price
                };

                Core.Context.OrderItems.Add(orderItem);

                item.Product.StockQuantity -=
                    item.Cart.Quantity;

                Core.Context.CartItems.Remove(
                    item.Cart);
            }

            Core.Context.SaveChanges();

            MessageBox.Show(
                "Заказ успешно создан.",
                "Оформление заказа",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            _mainWindow.OpenPage(
                new OrdersPage(
                    _mainWindow,
                    _userId),
                "Мои заказы");
        }
    }
}
