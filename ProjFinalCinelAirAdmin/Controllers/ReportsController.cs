using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Data;

namespace ProjFinalCinelAirAdmin.Controllers
{
    public class ReportsController : Controller
    {

        private readonly DataContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public ReportsController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; 
        }


        public IActionResult Index()
        {
            // Lista de possíveis relatórios
            List<Client> list = _context.Client.ToList();

            return View(list);
        }

        public IActionResult GetPdf(int id)
        {
            
            Client client = _context.Client.Where(x => x.Id == id).FirstOrDefault();
            List<Client> list = new List<Client>();
            list.Add(client);

            ClientReport rpt = new ClientReport(_webHostEnvironment);

            return File(rpt.Report(list), "application/pdf");

          
        }

        public IActionResult GetExcel()
        {
            List<Client> list = _context.Client.ToList();

            byte[] fileContents;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("Report");

                #region Header Row
                workSheet.Cells[1, 1].Value = "Client Name";
                workSheet.Cells[1, 1].Style.Font.Size = 12;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[1, 1].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 2].Value = "Client Number";
                workSheet.Cells[1, 2].Style.Font.Size = 12;
                workSheet.Cells[1, 2].Style.Font.Bold = true;
                workSheet.Cells[1, 2].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 3].Value = "Client Miles";
                workSheet.Cells[1, 3].Style.Font.Size = 12;
                workSheet.Cells[1, 3].Style.Font.Bold = true;
                workSheet.Cells[1, 3].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                #endregion

                #region Body

                for (int i = 0; i < list.Count; i++)
                {
                    workSheet.Cells[i + 2, 1].Value = list[i].FirstName;
                    workSheet.Cells[i + 2, 1].Style.Font.Size = 12;
                    workSheet.Cells[i + 2, 1].Style.Border.Top.Style = ExcelBorderStyle.Hair;


                    workSheet.Cells[i + 2, 2].Value = list[i].Client_Number;
                    workSheet.Cells[i + 2, 2].Style.Font.Size = 12;
                    workSheet.Cells[i + 2, 2].Style.Border.Top.Style = ExcelBorderStyle.Hair;


                    workSheet.Cells[i + 2, 3].Value = list[i].Miles_Status;
                    workSheet.Cells[i + 2, 3].Style.Font.Size = 12;
                    workSheet.Cells[i + 2, 3].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                }
                #endregion

                fileContents = package.GetAsByteArray();
            }
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();

            }
            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "Clients.xlsx"
                );

        }


    }
}
