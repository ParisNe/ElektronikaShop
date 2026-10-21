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

        private readonly CaptchaService _captchaService =
    new CaptchaService();

        private int _failedAttempts = 0;

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

            if (_failedAttempts >= 3)
            {
                bool captchaCorrect =
                    _captchaService.Check(
                        CaptchaTextBox.Text);

                if (!captchaCorrect)
                {
                    ErrorTextBlock.Text =
                        "Неверно введена капча.";

                    _captchaService.Generate();

                    CaptchaCodeTextBlock.Text =
                        _captchaService.CurrentCode;

                    CaptchaTextBox.Clear();

                    return;
                }
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
                _failedAttempts++;

                ErrorTextBlock.Text =
                    "Неверный логин или пароль.";

                if (_failedAttempts >= 3)
                {
                    CaptchaPanel.Visibility =
                        Visibility.Visible;

                    _captchaService.Generate();

                    CaptchaCodeTextBlock.Text =
                        _captchaService.CurrentCode;

                    CaptchaTextBox.Clear();
                }

                return;
            }

            _failedAttempts = 0;

            CaptchaPanel.Visibility =
                Visibility.Collapsed;

            ErrorTextBlock.Text = "";

            if (user.Role == "Пользователь")
            {
                _mainWindow.OpenPage(
                new CatalogPage(
                    _mainWindow,
                    user.UserId),
                "Каталог товаров");

                return;
            }

            if (user.Role == "Администратор")
            {
                _mainWindow.OpenPage(
                    new AdminDashboardPage(_mainWindow),
                    "Панель администратора");

                return;
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