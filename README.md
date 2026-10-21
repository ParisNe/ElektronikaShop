# ElektronikaShop
Курсовой проект WPF C# «Интернет-магазин электроники» по методическим указаниям УП.01.
Структура
`db/schema.sql` — создание БД SQL Server, первичные/внешние ключи, ограничения, каскадное удаление и тестовые данные.
`Pages/` — страницы авторизации, регистрации, каталога и администрирования.
`Services/` — подключение к БД, хэширование PBKDF2, капча, экспорт.
`Resources/Dictionary.xaml` — единый словарь стилей в цветовой палитре руководства.
Запуск
В SQL Server Management Studio выполнить `db/schema.sql`.
Открыть `ElektronikaShop.csproj` в Visual Studio на Windows с workload .NET desktop development.
При необходимости изменить строку подключения в `Data/DbConnectionFactory.cs`.
Запустить проект.
Тестовые пользователи: `admin` / `Admin123!`, `ivanov` / `User123!`, `petrova` / `User123!`.
