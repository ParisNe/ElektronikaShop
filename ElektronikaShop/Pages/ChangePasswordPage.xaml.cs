using ElektronikaShop.Services;
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
    /// Логика взаимодействия для ChangePasswordPage.xaml
    /// </summary>
    public partial class ChangePasswordPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly int _userId;

        public ChangePasswordPage(
            MainWindow mainWindow,
            int userId)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            _userId = userId;
        }

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string oldPassword =
                OldPasswordBox.Password;

            string newPassword =
                NewPasswordBox.Password;

            string repeatPassword =
                RepeatPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(
                    oldPassword)
                ||
                string.IsNullOrWhiteSpace(
                    newPassword)
                ||
                string.IsNullOrWhiteSpace(
                    repeatPassword))
            {
                ShowError(
                    "Заполните все поля.");

                return;
            }

            if (newPassword.Length < 8)
            {
                ShowError(
                    "Новый пароль должен содержать " +
                    "минимум 8 символов.");

                return;
            }

            if (newPassword != repeatPassword)
            {
                ShowError(
                    "Новые пароли не совпадают.");

                return;
            }

            var user = Core.Context.Users
                .FirstOrDefault(x =>
                    x.UserId == _userId);

            if (user == null)
            {
                ShowError(
                    "Пользователь не найден.");

                return;
            }

            bool oldPasswordCorrect =
                PasswordHasher.Verify(
                    oldPassword,
                    user.PasswordHash);

            if (!oldPasswordCorrect)
            {
                ShowError(
                    "Неверно введен текущий пароль.");

                return;
            }

            user.PasswordHash =
                PasswordHasher.Hash(newPassword);

            Core.Context.SaveChanges();

            MessageBox.Show(
                "Пароль успешно изменен.",
                "Смена пароля",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            OldPasswordBox.Clear();
            NewPasswordBox.Clear();
            RepeatPasswordBox.Clear();
            MessageTextBlock.Text = "";
        }

        private void ShowError(string message)
        {
            MessageTextBlock.Text = message;
        }
    }
}
