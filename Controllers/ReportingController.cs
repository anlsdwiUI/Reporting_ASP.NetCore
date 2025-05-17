using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Reporting.Data;
using Reporting.Models;
using Rotativa.AspNetCore;
using NPOI.XSSF.UserModel;
using System.Globalization;
using NPOI.SS.UserModel;

namespace Reporting.Controllers
{
    public class ReportingController : Controller
    {
        private readonly IPersonRepository _personRepo;

        public ReportingController(IConfiguration config)
        {
            string connStr = config.GetConnectionString("DefaultConnection");
            _personRepo = new PersonRepository(connStr);
        }

        public IActionResult ViewHtml()
        {
            var data = _personRepo.GetAllPersons();
            return View("PdfTemplate", data);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DownloadPdf()
        {
            var data = _personRepo.GetAllPersons();
            return new ViewAsPdf("PdfTemplate", data)
            {
                FileName = "Report.pdf"
            };
        }

        public IActionResult DownloadExcel()
        {
            try
            {
                var data = _personRepo.GetAllPersons();

                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Persons");

                var headerStyle = workbook.CreateCellStyle();
                var font = workbook.CreateFont();
                font.IsBold = true;
                headerStyle.SetFont(font);
                headerStyle.FillForegroundColor = IndexedColors.LightGreen.Index;
                headerStyle.FillPattern = FillPattern.SolidForeground;

                var header = sheet.CreateRow(0);
                string[] columns = { "Name", "Age", "BirthDay", "Job", "Education" };
                for (int i = 0; i < columns.Length; i++)
                {
                    var cell = header.CreateCell(i);
                    cell.SetCellValue(columns[i]);
                    cell.CellStyle = headerStyle;
                }

                for (int i = 0; i < data.Count; i++)
                {
                    var row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(data[i].Name);
                    row.CreateCell(1).SetCellValue(data[i].Age);
                    row.CreateCell(2).SetCellValue(data[i].BirthDay.ToString("dd MMM yyyy", CultureInfo.InvariantCulture));
                    row.CreateCell(3).SetCellValue(data[i].Job);
                    row.CreateCell(4).SetCellValue(data[i].Education);
                }

                for (int i = 0; i < columns.Length; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                using var stream = new MemoryStream();
                workbook.Write(stream);
                var content = stream.ToArray();

                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Report.xlsx");
            }
            catch (Exception ex)
            {
                return Content("Error generating Excel: " + ex.Message);
            }
        }
    }
}