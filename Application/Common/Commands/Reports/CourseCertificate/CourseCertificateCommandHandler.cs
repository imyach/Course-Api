using Application.Common.Dtos.Reports.Course;
using MediatR;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Reflection;

namespace Application.Common.Commands.Reports.CourseCertificate
{
    public class CourseCertificateCommandHandler : IRequestHandler<CourseCertificateCommand, CourseCertificateResponceDto>
    {
        public async Task<CourseCertificateResponceDto> Handle(CourseCertificateCommand request, CancellationToken cancellationToken)
        {
            // Получаем путь к иконке относительно текущей директории
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var iconPath = Path.Combine(currentDirectory, "Icons", "Skill-Forge-icon.png");

            // Преобразуем изображение в base64 для вставки в HTML
            string imageBase64 = string.Empty;
            if (File.Exists(iconPath))
            {
                var imageBytes = File.ReadAllBytes(iconPath);
                imageBase64 = Convert.ToBase64String(imageBytes);
            }

            // HTML-шаблон сертификата с отступами от краёв
            var htmlTemplate = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Сертификат SkillForge</title>
                <style>
                    * {{
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }}
                    
                    body {{
                        font-family: 'Roboto', 'Segoe UI', Arial, sans-serif;
                        background: white;
                        width: 210mm;
                        height: 297mm;
                        margin: 0;
                        padding: 15mm;
                    }}
                    
                    .certificate {{
                        width: 100%;
                        height: 100%;
                        background: white;
                        position: relative;
                        border: 3px solid #4CAF50;
                        box-sizing: border-box;
                        border-radius: 10px;
                    }}
                    
                    /* Декоративная зеленая полоса сверху */
                    .certificate::before {{
                        content: '';
                        position: absolute;
                        top: -3px;
                        left: 0;
                        right: 0;
                        height: 8px;
                        background: linear-gradient(90deg, #2E7D32 0%, #4CAF50 50%, #81C784 100%);
                        border-radius: 10px 10px 0 0;
                    }}
                    
                    .certificate-inner {{
                        padding: 25px 35px;
                        height: 100%;
                        display: flex;
                        flex-direction: column;
                        justify-content: space-between;
                    }}
                    
                    /* Декоративный орнамент по углам */
                    .corner {{
                        position: absolute;
                        width: 40px;
                        height: 40px;
                        border-color: #4CAF50;
                        border-style: solid;
                        z-index: 1;
                    }}
                    
                    .corner-tl {{ top: 15px; left: 15px; border-width: 2px 0 0 2px; }}
                    .corner-tr {{ top: 15px; right: 15px; border-width: 2px 2px 0 0; }}
                    .corner-bl {{ bottom: 15px; left: 15px; border-width: 0 0 2px 2px; }}
                    .corner-br {{ bottom: 15px; right: 15px; border-width: 0 2px 2px 0; }}
                    
                    .logo {{
                        text-align: center;
                        margin-bottom: 10px;
                    }}
                    
                    .logo-image {{
                        width: 70px;
                        height: 70px;
                        object-fit: contain;
                        display: inline-block;
                    }}
                    
                    .logo-text {{
                        font-size: 18px;
                        font-weight: 700;
                        color: #2E7D32;
                        letter-spacing: 2px;
                        margin-top: 5px;
                    }}
                    
                    .badge {{
                        text-align: center;
                        margin: 5px 0;
                    }}
                    
                    .badge-icon {{
                        font-size: 40px;
                        color: #FFD700;
                        text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
                    }}
                    
                    .title {{
                        font-size: 36px;
                        font-weight: 700;
                        text-align: center;
                        color: #2E7D32;
                        letter-spacing: 6px;
                        margin: 15px 0 8px;
                        text-transform: uppercase;
                    }}
                    
                    .subtitle {{
                        text-align: center;
                        color: #666;
                        font-size: 14px;
                        margin-bottom: 20px;
                        padding-bottom: 10px;
                        border-bottom: 1px solid #e0e0e0;
                    }}
                    
                    .confirmation {{
                        text-align: center;
                        font-size: 16px;
                        color: #555;
                        margin: 15px 0 20px;
                    }}
                    
                    .recipient-name {{
                        text-align: center;
                        font-size: 32px;
                        font-weight: 700;
                        color: #1a1a1a;
                        padding: 12px 25px;
                        margin: 8px 0;
                        background: linear-gradient(135deg, #f9f9f9, #f0f0f0);
                        border-radius: 50px;
                        letter-spacing: 1px;
                    }}
                    
                    .course-completion {{
                        text-align: center;
                        font-size: 16px;
                        color: #555;
                        margin: 15px 0 8px;
                    }}
                    
                    .course-name {{
                        text-align: center;
                        font-size: 22px;
                        font-weight: 600;
                        color: #4CAF50;
                        margin: 8px 0 25px;
                        padding: 8px 18px;
                        background: #e8f5e9;
                        border-radius: 35px;
                        display: inline-block;
                        width: auto;
                        margin-left: auto;
                        margin-right: auto;
                    }}
                    
                    .details {{
                        display: flex;
                        justify-content: space-between;
                        margin: 25px 0 15px;
                        padding-top: 15px;
                        border-top: 1px solid #e0e0e0;
                    }}
                    
                    .detail-item {{
                        text-align: center;
                        flex: 1;
                    }}
                    
                    .detail-label {{
                        font-size: 11px;
                        color: #999;
                        text-transform: uppercase;
                        letter-spacing: 1px;
                    }}
                    
                    .detail-value {{
                        font-size: 13px;
                        color: #333;
                        font-weight: 500;
                        margin-top: 4px;
                    }}
                    
                    .signature {{
                        text-align: center;
                        margin: 15px 0 12px;
                        padding-top: 12px;
                    }}
                    
                    .signature-line {{
                        display: inline-block;
                        width: 180px;
                        border-bottom: 1px solid #333;
                        margin: 0 15px;
                    }}
                    
                    .signature-text {{
                        font-size: 11px;
                        color: #666;
                        margin-top: 4px;
                    }}
                    
                    .footer {{
                        text-align: center;
                        margin-top: 10px;
                        padding-top: 8px;
                        border-top: 1px solid #e0e0e0;
                    }}
                    
                    .footer-text {{
                        font-size: 8px;
                        color: #aaa;
                    }}
                    
                    .seal {{
                        text-align: center;
                        margin-top: 8px;
                    }}
                    
                    .seal-icon {{
                        font-size: 30px;
                        color: #4CAF50;
                        opacity: 0.5;
                    }}
                    
                    @media print {{
                        body {{
                            margin: 0;
                            padding: 0;
                        }}
                        .certificate {{
                            border: 3px solid #4CAF50;
                        }}
                    }}
                </style>
            </head>
            <body>
                <div class='certificate'>
                    <div class='certificate-inner'>
                        <div class='corner corner-tl'></div>
                        <div class='corner corner-tr'></div>
                        <div class='corner corner-bl'></div>
                        <div class='corner corner-br'></div>
                        
                        <div>
                            <div class='logo'>
                                {(string.IsNullOrEmpty(imageBase64) ?
                                    "<span class='logo-icon' style='font-size:50px'>🎓</span>" :
                                    $"<img src='data:image/png;base64,{imageBase64}' class='logo-image' alt='SkillForge'>")}
                                <div class='logo-text'>SKILLFORGE</div>
                            </div>
                            
                            <div class='badge'>
                                <div class='badge-icon'>★</div>
                            </div>
                            
                            <div class='title'>СЕРТИФИКАТ</div>
                            <div class='subtitle'>о прохождении обучения</div>
                            
                            <div class='confirmation'>Настоящим подтверждается, что</div>
                            <div class='recipient-name'>{request.Name}</div>
                            <div class='course-completion'>успешно завершил(а) курс</div>
                            <div class='course-name'>«{request.CourseTitle}»</div>
                            
                            <div class='details'>
                                <div class='detail-item'>
                                    <div class='detail-label'>Дата выдачи</div>
                                    <div class='detail-value'>{request.PassedDate:dd.MM.yyyy}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Номер сертификата</div>
                                    <div class='detail-value'>SF-{request.PassedDate:yyyyMMdd}-{Math.Abs(request.Name.GetHashCode()) % 10000}</div>
                                </div>
                            </div>
                        </div>
                        
                        <div>
                            <div class='signature'>
                                <span class='signature-line'></span>
                                <div class='signature-text'>Руководитель платформы SkillForge</div>
                            </div>
                            
                            <div class='footer'>
                                <div class='footer-text'>Документ не требует печати. Подлинность можно проверить на сайте skillforge.com</div>
                            </div>
                            
                            <div class='seal'>
                                <div class='seal-icon'>❦</div>
                            </div>
                        </div>
                    </div>
                </div>
            </body>
            </html>";

            // Скачивание браузера (только при первом запуске)
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            // Запуск браузера
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new[] { "--no-sandbox" }
            });

            await using var page = await browser.NewPageAsync();

            // Устанавливаем размер страницы под A4
            await page.SetViewportAsync(new ViewPortOptions { Width = 800, Height = 1131 });
            await page.SetContentAsync(htmlTemplate);

            // Генерация PDF на весь лист A4 с отступами
            var pdfBytes = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                Landscape = false,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "5mm",
                    Bottom = "5mm",
                    Left = "5mm",
                    Right = "5mm"
                }
            });

            var fileName = $"Certificate_{request.Name}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            return new CourseCertificateResponceDto
            {
                FileContent = pdfBytes,
                FileName = fileName
            };
        }
    }
}