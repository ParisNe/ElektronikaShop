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
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly int _userId;

        private List<ProductViewModel> _allProducts =
            new List<ProductViewModel>();

        public CatalogPage(
    MainWindow mainWindow,
    int userId)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            _userId = userId;

            LoadCategories();
            LoadProducts();
        }

        private void LoadCategories()
        {
            var categories = Core.Context.Categories
                .Where(x => x.IsActive == true)
                .OrderBy(x => x.Name)
                .ToList();

            CategoryComboBox.ItemsSource = categories;
        }

        private void LoadProducts()
        {
            _allProducts =
                (from product in Core.Context.Products
                 join category in Core.Context.Categories
                 on product.CategoryId equals category.CategoryId
                 where product.IsActive == true
                 select new ProductViewModel
                 {
                     ProductId = product.ProductId,
                     Name = product.Name,
                     Brand = product.Brand,
                     CategoryName = category.Name,
                     Price = product.Price,
                     StockQuantity = product.StockQuantity
                 })
                .ToList();

            ProductsDataGrid.ItemsSource =
                _allProducts;
        }

        private void SearchButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string searchText =
                SearchTextBox.Text.Trim()
                .ToLower();

            var selectedCategory =
                CategoryComboBox.SelectedItem
                as Categories;

            IEnumerable<ProductViewModel> result =
                _allProducts;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                result = result.Where(x =>
                    x.Name.ToLower()
                        .Contains(searchText)
                    ||
                    x.Brand.ToLower()
                        .Contains(searchText));
            }

            if (selectedCategory != null)
            {
                result = result.Where(x =>
                    x.CategoryName ==
                    selectedCategory.Name);
            }

            if (SortComboBox.SelectedIndex == 1)
            {
                result = result.OrderBy(x => x.Price);
            }
            else if (SortComboBox.SelectedIndex == 2)
            {
                result = result.OrderByDescending(
                    x => x.Price);
            }
            else if (SortComboBox.SelectedIndex == 3)
            {
                result = result.OrderBy(x => x.Name);
            }

            ProductsDataGrid.ItemsSource =
                result.ToList();
        }

        private void ResetButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SearchTextBox.Clear();

            CategoryComboBox.SelectedIndex = -1;
            SortComboBox.SelectedIndex = 0;

            ProductsDataGrid.ItemsSource =
                _allProducts;
        }

        private void ChangePasswordButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            _mainWindow.OpenPage(
                new ChangePasswordPage(
                    _mainWindow,
                    _userId),
                "Смена пароля");
        }

        private void OrdersButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            _mainWindow.OpenPage(
                new OrdersPage(
                    _mainWindow,
                    _userId),
                "Мои заказы");
        }
    }



    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string CategoryName { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
    }
}
