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
    /// Логика взаимодействия для ProductManagementPage.xaml
    /// </summary>
    public partial class ProductManagementPage : Page
    {
        private readonly MainWindow _mainWindow;

        public ProductManagementPage(
            MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            LoadCategories();
            LoadProducts();
        }

        private void LoadCategories()
        {
            CategoryComboBox.ItemsSource =
                Core.Context.Categories
                .Where(x => x.IsActive == true)
                .OrderBy(x => x.Name)
                .ToList();
        }

        private void LoadProducts()
        {
            ProductsDataGrid.ItemsSource =
                Core.Context.Products
                .Where(x => x.IsActive == true)
                .OrderBy(x => x.Name)
                .ToList();
        }

        private void AddButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                    NameTextBox.Text))
            {
                ShowMessage("Введите название товара.");
                return;
            }

            if (string.IsNullOrWhiteSpace(
                    BrandTextBox.Text))
            {
                ShowMessage("Введите бренд.");
                return;
            }

            if (!decimal.TryParse(
                    PriceTextBox.Text,
                    out decimal price))
            {
                ShowMessage("Введите корректную цену.");
                return;
            }

            if (!int.TryParse(
                    StockTextBox.Text,
                    out int stock))
            {
                ShowMessage(
                    "Введите корректное количество.");
                return;
            }

            var selectedCategory =
                CategoryComboBox.SelectedItem
                as Categories;

            if (selectedCategory == null)
            {
                ShowMessage("Выберите категорию.");
                return;
            }

            var product = new Products
            {
                Name = NameTextBox.Text.Trim(),
                Brand = BrandTextBox.Text.Trim(),
                Price = price,
                StockQuantity = stock,
                CategoryId =
                    selectedCategory.CategoryId,
                Sku = "SKU-" +
                      DateTime.Now.Ticks,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            Core.Context.Products.Add(product);
            Core.Context.SaveChanges();

            ShowMessage("Товар добавлен.");

            ClearFields();
            LoadProducts();
        }

        private void DeleteButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var selectedProduct =
                ProductsDataGrid.SelectedItem
                as Products;

            if (selectedProduct == null)
            {
                ShowMessage(
                    "Выберите товар для удаления.");
                return;
            }

            MessageBoxResult result =
                MessageBox.Show(
                    "Вы действительно хотите удалить товар?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            selectedProduct.IsActive = false;

            Core.Context.SaveChanges();

            ShowMessage("Товар удален.");

            LoadProducts();
        }

        private void RefreshButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoadCategories();
            LoadProducts();

            ShowMessage("Данные обновлены.");
        }

        private void ClearFields()
        {
            NameTextBox.Clear();
            BrandTextBox.Clear();
            PriceTextBox.Clear();
            StockTextBox.Clear();
            CategoryComboBox.SelectedIndex = -1;
        }

        private void ShowMessage(string message)
        {
            MessageTextBlock.Text = message;
        }
    }
}
