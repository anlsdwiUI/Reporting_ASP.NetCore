using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Reporting.Models;
using Rotativa.AspNetCore;

namespace Reporting.Controllers
{
    public class ReportingController : Controller
    {
        private List<Person> GetSampleData()
        {
            return new List<Person>
            {
                new Person
                {
                    Name = "Alice",
                    Age = 25,
                    BirthDay = new DateTime(1998, 5, 10),
                    Job = "Software Engineer",
                    Education = "Bachelor's Degree"
                },
                new Person
                {
                    Name = "Bob",
                    Age = 30,
                    BirthDay = new DateTime(1993, 3, 20),
                    Job = "Product Manager",
                    Education = "Master's Degree"
                }
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DownloadPdf()
        {
            try
            {
                var data = GetSampleData();
                return new ViewAsPdf("PdfTemplate", data)
                {
                    FileName = "Report.pdf"
                };
            }
            catch (Exception ex)
            {
                return Content("Error generating PDF: " + ex.Message);
            }
        }

        public IActionResult ViewHtml()
        {
            try
            {
                var data = GetSampleData();
                return View("PdfTemplate", data);
            }
            catch (Exception ex)
            {
                return Content("Error loading view: " + ex.Message);
            }
        }

        public IActionResult DownloadExcel()
        {
            try
            {
                var data = GetSampleData();

                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Data");

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
                    row.CreateCell(2).SetCellValue(data[i].BirthDay.ToString("dd MMM yyyy"));
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