using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ElektronikaShop.Pages
{
    public partial class ReportsPage : Page
    {
        private readonly MainWindow _mainWindow;

        public ReportsPage(
            MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
        }

        private void ExportExcelButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter =
                    "Excel файл (*.xlsx)|*.xlsx",
                FileName =
                    "Отчет_по_заказам.xlsx"
            };

            if (saveDialog.ShowDialog() != true)
            {
                return;
            }

            var orders =
                Core.Context.Orders
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            using (var workbook =
                new XLWorkbook())
            {
                var worksheet =
                    workbook.Worksheets.Add(
                        "Заказы");

                worksheet.Cell(1, 1)
                    .Value = "Код";

                worksheet.Cell(1, 2)
                    .Value = "Номер заказа";

                worksheet.Cell(1, 3)
                    .Value = "Пользователь";

                worksheet.Cell(1, 4)
                    .Value = "Статус";

                worksheet.Cell(1, 5)
                    .Value = "Адрес доставки";

                worksheet.Cell(1, 6)
                    .Value = "Сумма";

                worksheet.Cell(1, 7)
                    .Value = "Дата";

                int row = 2;

                foreach (var order in orders)
                {
                    worksheet.Cell(row, 1)
                        .Value = order.OrderId;

                    worksheet.Cell(row, 2)
                        .Value = order.OrderNumber;

                    worksheet.Cell(row, 3)
                        .Value = order.UserId;

                    worksheet.Cell(row, 4)
                        .Value = order.Status;

                    worksheet.Cell(row, 5)
                        .Value = order.DeliveryAddress;

                    worksheet.Cell(row, 6)
                        .Value = order.TotalAmount;

                    worksheet.Cell(row, 7)
                        .Value = order.CreatedAt;

                    row++;
                }

                worksheet.Columns()
                    .AdjustToContents();

                workbook.SaveAs(
                    saveDialog.FileName);
            }

            ShowMessage(
                "Excel-отчет успешно сохранен.");
        }

        private void ExportWordButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter =
                    "Word файл (*.docx)|*.docx",
                FileName =
                    "Отчет_по_заказам.docx"
            };

            if (saveDialog.ShowDialog() != true)
            {
                return;
            }

            var orders =
                Core.Context.Orders
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            using (WordprocessingDocument document =
                WordprocessingDocument.Create(
                    saveDialog.FileName,
                    WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart =
                    document.AddMainDocumentPart();

                mainPart.Document =
                    new Document(
                        new Body());

                Body body =
                    mainPart.Document.Body;

                body.Append(
                    new Paragraph(
                        new Run(
                            new Text(
                                "Отчет по заказам"))));

                body.Append(
                    new Paragraph(
                        new Run(
                            new Text(
                                "Дата формирования: " +
                                DateTime.Now.ToString(
                                    "dd.MM.yyyy HH:mm")))));

                foreach (var order in orders)
                {
                    string orderText =
                        $"Заказ {order.OrderNumber}; " +
                        $"статус: {order.Status}; " +
                        $"сумма: {order.TotalAmount:N2} ₽; " +
                        $"адрес: {order.DeliveryAddress}";

                    body.Append(
                        new Paragraph(
                            new Run(
                                new Text(orderText))));
                }

                mainPart.Document.Save();
            }

            ShowMessage(
                "Word-отчет успешно сохранен.");
        }

        private void ExportPdfButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter =
                    "PDF файл (*.pdf)|*.pdf",
                FileName =
                    "Отчет_по_заказам.pdf"
            };

            if (saveDialog.ShowDialog() != true)
            {
                return;
            }

            var orders =
                Core.Context.Orders
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            PdfDocument document =
                new PdfDocument();

            document.Info.Title =
                "Отчет по заказам";

            PdfPage page =
                document.AddPage();

            XGraphics graphics =
                XGraphics.FromPdfPage(page);

            XFont titleFont =
                new XFont(
                    "Arial",
                    18,
                    XFontStyleEx.Bold);

            XFont textFont =
                new XFont(
                    "Arial",
                    10,
                    XFontStyleEx.Regular);

            graphics.DrawString(
                "Отчет по заказам",
                titleFont,
                XBrushes.DarkBlue,
                new XPoint(40, 50));

            graphics.DrawString(
                "Дата формирования: " +
                DateTime.Now.ToString(
                    "dd.MM.yyyy HH:mm"),
                textFont,
                XBrushes.Black,
                new XPoint(40, 75));

            double y = 110;

            foreach (var order in orders)
            {
                string orderText =
                    $"Заказ: {order.OrderNumber}; " +
                    $"Статус: {order.Status}; " +
                    $"Сумма: {order.TotalAmount:N2} руб.; " +
                    $"Дата: {order.CreatedAt:dd.MM.yyyy}";

                graphics.DrawString(
                    orderText,
                    textFont,
                    XBrushes.Black,
                    new XPoint(40, y));

                y += 25;

                if (y > 760)
                {
                    page = document.AddPage();

                    graphics =
                        XGraphics.FromPdfPage(page);

                    y = 50;
                }
            }

            document.Save(saveDialog.FileName);

            ShowMessage(
                "PDF-отчет успешно сохранен.");
        }

        private void ShowMessage(string message)
        {
            MessageTextBlock.Text = message;
        }
    }
}