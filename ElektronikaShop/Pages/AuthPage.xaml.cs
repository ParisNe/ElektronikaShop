using ElektronikaShop.Services;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ElektronikaShop.Services;

namespace ElektronikaShop.Pages
{
    public partial class AuthPage : Page
    {
        private readonly MainWindow _mainWindow;

        public AuthPage(MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
        }

        private void LoginButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login))
            {
                ErrorTextBlock.Text = "Введите логин.";
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorTextBlock.Text = "Введите пароль.";
                return;
            }

            var user = Core.Context.Users
    .FirstOrDefault(x =>
        x.Login == login &&
        x.IsActive == true);

            if (user == null ||
                !PasswordHasher.Verify(
                    password,
                    user.PasswordHash))
            {
                ErrorTextBlock.Text =
                    "Неверный логин или пароль.";

                return;
            }

            if (user == null)
            {
                ErrorTextBlock.Text =
                    "Неверный логин или пароль.";

                return;
            }

            ErrorTextBlock.Text = "";

            if (user.Role == "Администратор")
            {
                MessageBox.Show(
                    "Вы вошли как администратор.",
                    "Авторизация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(
                    "Вы вошли как пользователь.",
                    "Авторизация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void RegisterButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            _mainWindow.OpenPage(
                new RegisterPage(_mainWindow),
                "Регистрация");
        }
    }
}