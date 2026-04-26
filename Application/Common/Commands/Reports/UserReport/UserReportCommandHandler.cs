using Application.Common.Dtos.Reports.Users;
using Application.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text;

namespace Application.Common.Commands.Reports.UserReport
{
    public class UserReportCommandHandler(ICoursesDbContext context) : IRequestHandler<UserReportCommand, UserReportReusltDto>
    {
        public async Task<UserReportReusltDto> Handle(UserReportCommand request, CancellationToken cancellationToken)
        {
            switch (request.Format?.ToLower())
            {
                case ".xlsx":
                case "excel":
                case "xlsx":
                    return await GenerateExcelReport(request, cancellationToken);
                case ".pdf":
                case "pdf":
                    return await GeneratePdfReport(request, cancellationToken);
                default:
                    throw new FormatException("Unsupported format. Please use .xlsx or .pdf");
            }
        }


        public async Task<UserReportReusltDto> GenerateExcelReport(UserReportCommand request, CancellationToken ct = default)
        {
            // 1. Формируем запрос к БД (без изменений)
            var query = context.Users
                .Include(u => u.Role)
                .Where(u => u.IsActive == true)
                .AsQueryable();

            if (!request.IsAllTime)
            {
                query = query.Where(u => u.CreatedAt >= request.StartDate && u.CreatedAt <= request.EndDate);
            }

            var allUsers = await query.ToListAsync(ct);

            IEnumerable<UserReportDto> filteredUsers = allUsers.Select(u => new UserReportDto
            {
                NameUser = u.NameUser ?? string.Empty,
                Login = u.Login ?? string.Empty,
                Email = u.Email ?? string.Empty,
                HashPassword = u.HashPassword ?? string.Empty,
                RoleName = u.Role?.Name ?? "Без роли",
                CreatedAt = u.CreatedAt,
                PhoneNumber = u.PhoneNumber ?? string.Empty,
                IsActive = u.IsActive
            });

            if (request.RoleId.HasValue && request.RoleId.Value != Guid.Empty)
            {
                var roleName = await context.Roles
                    .Where(r => r.Id == request.RoleId)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync(ct);
                filteredUsers = filteredUsers.Where(u => u.RoleName == roleName);
            }

            var usersList = filteredUsers.ToList();

            var groupedUsers = usersList
                .GroupBy(u => u.RoleName)
                .OrderBy(g => g.Key)
                .ToList();

            // 2. Генерация Excel с помощью EPPlus
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Пользователи");

                // Настройка шрифта
                worksheet.Cells.Style.Font.Name = "Segoe UI";
                worksheet.Cells.Style.Font.Size = 11;

                int currentRow = 1;
                int totalUsers = 0;

                // 3. Заголовок отчета
                string reportTitle = string.IsNullOrEmpty(request.Name) ? "Отчет по пользователям" : request.Name;
                worksheet.Cells[currentRow, 1, currentRow, 5].Merge = true;
                worksheet.Cells[currentRow, 1].Value = reportTitle;
                worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                worksheet.Cells[currentRow, 1].Style.Font.Size = 16;
                worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[currentRow, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                currentRow++;

                // 4. Дата формирования
                worksheet.Cells[currentRow, 1].Value = $"Дата формирования отчета: {DateTime.Now:dd.MM.yyyy}";
                worksheet.Cells[currentRow, 1].Style.Font.Size = 10;
                worksheet.Cells[currentRow, 1].Style.Font.Italic = true;
                currentRow++;

                // 5. Период отчета
                if (!request.IsAllTime)
                {
                    worksheet.Cells[currentRow, 1].Value = $"Зарегистрированные пользователи: с {request.StartDate:dd.MM.yyyy} по {request.EndDate:dd.MM.yyyy}";
                }
                else
                {
                    worksheet.Cells[currentRow, 1].Value = "Зарегистрированные пользователи: за все время";
                }
                worksheet.Cells[currentRow, 1].Style.Font.Size = 10;
                worksheet.Cells[currentRow, 1].Style.Font.Italic = true;
                currentRow++;
                currentRow++;

                // 6. Заголовки таблицы
                string[] headers = { "№", "ФИО", "Логин", "Email", "Телефон", "Дата регистрации", "Роль" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[currentRow, i + 1].Value = headers[i];
                    worksheet.Cells[currentRow, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[currentRow, i + 1].Style.Font.Size = 12;
                    worksheet.Cells[currentRow, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[currentRow, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    worksheet.Cells[currentRow, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                currentRow++;

                // 7. Заполнение данных с группировкой
                int rowNumber = 1;

                foreach (var group in groupedUsers)
                {
                    // Заголовок группы
                    worksheet.Cells[currentRow, 1, currentRow, headers.Length].Merge = true;
                    worksheet.Cells[currentRow, 1].Value = group.Key;
                    worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                    worksheet.Cells[currentRow, 1].Style.Font.Size = 12;
                    worksheet.Cells[currentRow, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[currentRow, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 241, 217));
                    worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    currentRow++;

                    // Данные пользователей
                    foreach (var user in group)
                    {
                        worksheet.Cells[currentRow, 1].Value = rowNumber++;
                        worksheet.Cells[currentRow, 2].Value = user.NameUser;
                        worksheet.Cells[currentRow, 3].Value = user.Login;
                        worksheet.Cells[currentRow, 4].Value = user.Email;
                        worksheet.Cells[currentRow, 5].Value = string.IsNullOrEmpty(user.PhoneNumber) ? "—" : user.PhoneNumber;
                        worksheet.Cells[currentRow, 6].Value = user.CreatedAt.ToString("dd.MM.yyyy");
                        worksheet.Cells[currentRow, 7].Value = user.RoleName;
                        currentRow++;
                        totalUsers++;
                    }

                    currentRow++; // Пустая строка между группами
                }

                // 8. Итоговая строка
                worksheet.Cells[currentRow, 1, currentRow, headers.Length].Merge = true;
                worksheet.Cells[currentRow, 1].Value = $"ИТОГО: {totalUsers} пользователей";
                worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                worksheet.Cells[currentRow, 1].Style.Font.Size = 12;
                worksheet.Cells[currentRow, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[currentRow, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 204));
                worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // 9. Автоматическая ширина колонок
                worksheet.Cells.AutoFitColumns();

                // 10. Добавляем рамки для таблицы
                var dataRange = worksheet.Cells[4, 1, currentRow, headers.Length];
                dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                // 11. Возврат результата
                return new UserReportReusltDto
                {
                    FileContent = package.GetAsByteArray(),
                    FileName = request.Name,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };
            }
        }


        public async Task<UserReportReusltDto> GeneratePdfReport(UserReportCommand request, CancellationToken ct = default)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            DateTime startDateUtc = request.StartDate.ToUniversalTime();
            DateTime endDateUtc = request.EndDate.ToUniversalTime();

            // 1. Формируем запрос к БД
            var query = context.Users
                .Include(u => u.Role)
                .Where(u => u.IsActive == true)
                .AsQueryable();

            // 2. Фильтрация по датам (если не "за все время")
            if (!request.IsAllTime)
            {
                query = query.Where(u => u.CreatedAt >= startDateUtc && u.CreatedAt <= endDateUtc);
            }


            // 3. Получаем всех пользователей
            var allUsers = await query.ToListAsync(ct);

            // 4. Фильтрация по роли (если указана)
            IEnumerable<UserReportDto> filteredUsers = allUsers.Select(u => new UserReportDto
            {
                NameUser = u.NameUser ?? string.Empty,
                Login = u.Login ?? string.Empty,
                Email = u.Email ?? string.Empty,
                HashPassword = u.HashPassword ?? string.Empty,
                RoleName = u.Role?.Name ?? "Без роли",
                CreatedAt = u.CreatedAt,
                PhoneNumber = u.PhoneNumber ?? string.Empty,
                IsActive = u.IsActive
            });

            if (request.RoleId.HasValue && request.RoleId.Value != Guid.Empty)
            {
                var roleName = await context.Roles
                    .Where(r => r.Id == request.RoleId)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync(ct);

                filteredUsers = filteredUsers.Where(u => u.RoleName == roleName);
            }

            var usersList = filteredUsers.ToList();

            // 5. Группировка по ролям
            var groupedUsers = usersList
                .GroupBy(u => u.RoleName)
                .OrderBy(g => g.Key)
                .ToList();

            // 6. Создание PDF документа
            string reportTitle = string.IsNullOrEmpty(request.Name) ? "Отчет по пользователям" : request.Name;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Создаем документ A4 с полями
                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // 7. Добавляем шрифты
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

                if (!File.Exists(fontPath))
                {
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                }

                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont baseFontBold = BaseFont.CreateFont(
                    fontPath.Replace(".ttf", "bd.ttf"),
                    BaseFont.IDENTITY_H,
                    BaseFont.EMBEDDED);

                Font titleFont = new Font(baseFontBold, 18, Font.BOLD);
                Font headerFont = new Font(baseFontBold, 12, Font.BOLD);
                Font normalFont = new Font(baseFont, 10, Font.NORMAL);
                Font groupFont = new Font(baseFontBold, 13, Font.BOLD);
                Font dateFont = new Font(baseFont, 9, Font.ITALIC);
                Font totalFont = new Font(baseFontBold, 11, Font.BOLD);


                // 8. Заголовок отчета
                Paragraph title = new Paragraph(reportTitle, titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20;
                document.Add(title);

                // 9. Информация о формировании
                Paragraph info = new Paragraph($"Дата формирования отчета: {DateTime.Now:dd.MM.yyyy}", dateFont);
                info.SpacingAfter = 5;
                document.Add(info);

                // 10. Период отчета
                Paragraph period;
                if (!request.IsAllTime)
                {
                    period = new Paragraph($"Зарегистрированные пользователи: с {request.StartDate:dd.MM.yyyy} по {request.EndDate:dd.MM.yyyy}", dateFont);
                }
                else
                {
                    period = new Paragraph("Зарегистрированные пользователи: за все время", dateFont);
                }
                period.SpacingAfter = 20;
                document.Add(period);

                // 11. Создание таблицы (7 колонок)
                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                float[] columnWidths = { 5f, 15f, 12f, 18f, 15f, 12f, 13f };
                table.SetWidths(columnWidths);

                // 12. Заголовки таблицы
                string[] headers = { "№", "ФИО", "Логин", "Email", "Телефон", "Дата регистрации", "Роль" };
                foreach (string header in headers)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, headerFont));
                    headerCell.BackgroundColor = new BaseColor(220, 220, 220);
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    headerCell.Padding = 8;
                    table.AddCell(headerCell);
                }

                // 13. Заполнение данных с группировкой
                int rowNumber = 1;
                int totalUsers = 0;

                foreach (var group in groupedUsers)
                {
                    // Добавляем ячейку для названия группы (объединяем на 7 колонок)
                    PdfPCell groupCell = new PdfPCell(new Phrase(group.Key, groupFont));
                    groupCell.Colspan = 7;
                    groupCell.BackgroundColor = new BaseColor(217, 241, 217);
                    groupCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    groupCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    groupCell.Padding = 8;
                    table.AddCell(groupCell);

                    // Данные пользователей в группе
                    foreach (var user in group)
                    {
                        // №
                        table.AddCell(CreateCell(rowNumber.ToString(), normalFont, Element.ALIGN_CENTER));
                        // ФИО
                        table.AddCell(CreateCell(user.NameUser, normalFont, Element.ALIGN_LEFT));
                        // Логин
                        table.AddCell(CreateCell(user.Login, normalFont, Element.ALIGN_LEFT));
                        // Email
                        table.AddCell(CreateCell(user.Email, normalFont, Element.ALIGN_LEFT));
                        // Телефон
                        string phone = string.IsNullOrEmpty(user.PhoneNumber) ? "—" : user.PhoneNumber;
                        table.AddCell(CreateCell(phone, normalFont, Element.ALIGN_LEFT));
                        // Дата регистрации
                        table.AddCell(CreateCell(user.CreatedAt.ToString("dd.MM.yyyy"), normalFont, Element.ALIGN_CENTER));
                        // Роль
                        table.AddCell(CreateCell(user.RoleName, normalFont, Element.ALIGN_LEFT));

                        rowNumber++;
                        totalUsers++;
                    }

                    // Пустая строка между группами (добавляем ячейку-разделитель)
                    PdfPCell emptyCell = new PdfPCell(new Phrase(" "));
                    emptyCell.Colspan = 7;
                    emptyCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    emptyCell.Padding = 3;
                    table.AddCell(emptyCell);
                }

                // 14. Итоговая строка
                PdfPCell totalCell = new PdfPCell(new Phrase($"ИТОГО: {totalUsers} пользователей", totalFont));
                totalCell.Colspan = 7;
                totalCell.BackgroundColor = new BaseColor(255, 255, 204);
                totalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                totalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                totalCell.Padding = 8;
                table.AddCell(totalCell);

                document.Add(table);
                document.Close();

                // 15. Возврат результата
                return new UserReportReusltDto
                {
                    FileContent = memoryStream.ToArray(),
                    FileName = request.Name,
                    ContentType = "application/pdf"
                };
            }
        }

        // Вспомогательный метод для создания ячейки таблицы
        private PdfPCell CreateCell(string text, Font font, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 6;
            return cell;
        }
    }
}
