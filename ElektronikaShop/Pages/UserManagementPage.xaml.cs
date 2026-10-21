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
    /// Логика взаимодействия для UserManagementPage.xaml
    /// </summary>
    public partial class UserManagementPage : Page
    {
        private readonly MainWindow _mainWindow;

        private List<Users> _allUsers =
            new List<Users>();

        public UserManagementPage(
            MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            LoadUsers();
        }

        private void LoadUsers()
        {
            _allUsers = Core.Context.Users
                .OrderBy(x => x.Login)
                .ToList();

            UsersDataGrid.ItemsSource = _allUsers;
        }

        private void SearchButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string search =
                SearchTextBox.Text
                .Trim()
                .ToLower();

            if (string.IsNullOrWhiteSpace(search))
            {
                UsersDataGrid.ItemsSource =
                    _allUsers;

                return;
            }

            var result = _allUsers
                .Where(x =>
                    x.Login.ToLower()
                        .Contains(search)
                    ||
                    x.Fio.ToLower()
                        .Contains(search)
                    ||
                    x.Email.ToLower()
                        .Contains(search))
                .ToList();

            UsersDataGrid.ItemsSource = result;
        }

        private void ResetButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SearchTextBox.Clear();

            UsersDataGrid.ItemsSource =
                _allUsers;
        }

        private void ChangeStatusButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var selectedUser =
                UsersDataGrid.SelectedItem
                as Users;

            if (selectedUser == null)
            {
                ShowMessage(
                    "Выберите пользователя.");

                return;
            }

            if (selectedUser.Role ==
                "Администратор")
            {
                ShowMessage(
                    "Нельзя заблокировать администратора.");

                return;
            }

            string action =
                selectedUser.IsActive == true
                    ? "заблокировать"
                    : "разблокировать";

            MessageBoxResult result =
                MessageBox.Show(
                    "Вы действительно хотите " +
                    action +
                    " пользователя?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            selectedUser.IsActive =
                !selectedUser.IsActive;

            Core.Context.SaveChanges();

            LoadUsers();

            ShowMessage(
                "Статус пользователя изменен.");
        }

        private void ShowMessage(string message)
        {
            MessageTextBlock.Text = message;
        }
    }
}
