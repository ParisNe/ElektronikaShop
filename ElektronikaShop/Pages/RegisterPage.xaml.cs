using ElektronikaShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        
            private readonly MainWindow _mainWindow;

            public RegisterPage(MainWindow mainWindow)
            {
                InitializeComponent();

                _mainWindow = mainWindow;
            }

            private void RegisterButton_Click(
                object sender,
                RoutedEventArgs e)
            {
                string login = LoginTextBox.Text.Trim();
                string password = PasswordBox.Password;
                string repeatPassword =
                    RepeatPasswordBox.Password;

                string fio = FioTextBox.Text.Trim();
                string email = EmailTextBox.Text.Trim();
                string phone = PhoneTextBox.Text.Trim();
                string address = AddressTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(login) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(fio) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(phone) ||
                    string.IsNullOrWhiteSpace(address))
                {
                    ShowError("Заполните все поля.");
                    return;
                }

                if (login.Length < 4)
                {
                    ShowError(
                        "Логин должен содержать минимум 4 символа.");
                    return;
                }

                if (password.Length < 8)
                {
                    ShowError(
                        "Пароль должен содержать минимум 8 символов.");
                    return;
                }

                if (password != repeatPassword)
                {
                    ShowError("Пароли не совпадают.");
                    return;
                }

                if (!Regex.IsMatch(
                        email,
                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ShowError("Введите корректный email.");
                    return;
                }

                var existingLogin = Core.Context.Users
                    .FirstOrDefault(x => x.Login == login);

                if (existingLogin != null)
                {
                    ShowError("Такой логин уже существует.");
                    return;
                }

                var existingEmail = Core.Context.Users
                    .FirstOrDefault(x => x.Email == email);

                if (existingEmail != null)
                {
                    ShowError("Такой email уже существует.");
                    return;
                }

                var user = new Users
                {
                    Login = login,
                    PasswordHash =
                        PasswordHasher.Hash(password),
                    Role = "Пользователь",
                    Fio = fio,
                    Email = email,
                    Phone = phone,
                    IsActive = true,
                    CreatedAt = System.DateTime.Now
                };

                Core.Context.Users.Add(user);
                Core.Context.SaveChanges();

                MessageBox.Show(
                    "Регистрация успешно завершена.",
                    "Регистрация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                _mainWindow.OpenPage(
                    new AuthPage(_mainWindow),
                    "Авторизация");
            }

            private void BackButton_Click(
                object sender,
                RoutedEventArgs e)
            {
                _mainWindow.OpenPage(
                    new AuthPage(_mainWindow),
                    "Авторизация");
            }

            private void ShowError(string message)
            {
                MessageTextBlock.Text = message;
            }
        
    }
 }
