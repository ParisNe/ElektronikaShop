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
    /// Логика взаимодействия для CategoryManagementPage.xaml
    /// </summary>
    public partial class CategoryManagementPage : Page
    {
        private readonly MainWindow _mainWindow;

        public CategoryManagementPage(
            MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            LoadCategories();
        }

        private void LoadCategories()
        {
            CategoriesDataGrid.ItemsSource =
                Core.Context.Categories
                .Where(x => x.IsActive == true)
                .OrderBy(x => x.Name)
                .ToList();
        }

        private void AddButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string name =
                NameTextBox.Text.Trim();

            string description =
                DescriptionTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                ShowMessage(
                    "Введите название категории.");

                return;
            }

            bool categoryExists =
                Core.Context.Categories.Any(x =>
                    x.Name == name &&
                    x.IsActive == true);

            if (categoryExists)
            {
                ShowMessage(
                    "Такая категория уже существует.");

                return;
            }

            var category = new Categories
            {
                Name = name,
                Description = description,
                IsActive = true
            };

            Core.Context.Categories.Add(category);
            Core.Context.SaveChanges();

            ClearFields();
            LoadCategories();

            ShowMessage("Категория добавлена.");
        }

        private void DeleteButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var selectedCategory =
                CategoriesDataGrid.SelectedItem
                as Categories;

            if (selectedCategory == null)
            {
                ShowMessage(
                    "Выберите категорию для удаления.");

                return;
            }

            MessageBoxResult result =
                MessageBox.Show(
                    "Вы действительно хотите удалить категорию?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            selectedCategory.IsActive = false;

            Core.Context.SaveChanges();

            LoadCategories();

            ShowMessage("Категория удалена.");
        }

        private void RefreshButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoadCategories();

            ShowMessage("Данные обновлены.");
        }

        private void ClearFields()
        {
            NameTextBox.Clear();
            DescriptionTextBox.Clear();
        }

        private void ShowMessage(string message)
        {
            MessageTextBlock.Text = message;
        }
    }
}
