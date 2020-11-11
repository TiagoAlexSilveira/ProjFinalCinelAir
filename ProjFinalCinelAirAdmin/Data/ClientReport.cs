using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Data
{
    public class ClientReport
    {
        private IWebHostEnvironment _webHostEnvironment;

        public ClientReport(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        #region Declaration
        int _maxColumn = 4;
        Document _document;
        Font _fontStyle;
        PdfPTable _pdfTable = new PdfPTable(4);
        PdfPCell _pdfCell;
        MemoryStream _memoryStream = new MemoryStream();

        List<Client> _clients = new List<Client>();

        #endregion

        public byte[] Report(List<Client> clients) 
        {
            _clients = clients;

            _document = new Document();
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(5f, 5f, 20f, 5f);

            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter docWriter = PdfWriter.GetInstance(_document, _memoryStream);

            _document.Open();

            float[] sizes = new float[_maxColumn];

            for (var i = 0; i < _maxColumn; i++)
            {
                if (i == 0)
                {
                    sizes[i] = 20;
                }

                else
                {
                    sizes[i] = 100;
                }
            }

            _pdfTable.SetWidths(sizes);

            this.ReportHeader();
            this.EmptyRow(2);
            this.ReportBody();



            _pdfTable.HeaderRows = 2;

            _document.Add(_pdfTable);
            _document.Close();

            return _memoryStream.ToArray();
        
        }

        private void ReportHeader() 
        {
            _pdfCell = new PdfPCell(this.AddLogo());
            _pdfCell.Colspan = 1;
            _pdfCell.Border = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(this.SetPageTitle());
            _pdfCell.Colspan = _maxColumn - 1;
            _pdfCell.Border = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();
        
        }

        private PdfPTable AddLogo() 
        {
            int maxColumn = 1;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);

            string path = _webHostEnvironment.WebRootPath + "/img";

            string imgCombine = Path.Combine(path, "Logo.jpg");
            Image img = Image.GetInstance(imgCombine);

            _pdfCell = new PdfPCell(img);
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();

            return pdfPTable;
        }

        private PdfPTable SetPageTitle() 
        {
            int maxColumn = 4;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);

            _fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _pdfCell = new PdfPCell(new Phrase("Client Information", _fontStyle));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);
            pdfPTable.CompleteRow();


            return pdfPTable;


        }

        private void EmptyRow(int nCount) 
        {
            for (int i = 0; i <= nCount; i++)
            { 
                _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
                _pdfCell.Colspan = _maxColumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.Border = 0;
                _pdfCell.ExtraParagraphSpace = 10;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
            }
        
        }

        private void ReportBody() 
        {
            var fontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            #region Detail Table Header

            _pdfCell = new PdfPCell(new Phrase("Name", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LightGray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Client Number", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LightGray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Tax Number", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LightGray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Identification", fontStyleBold));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LightGray;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();
            #endregion

            #region Detail table body

            foreach (var client in _clients)
            {
                _pdfCell = new PdfPCell(new Phrase(client.FirstName, fontStyleBold));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LightGray;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Convert.ToString(client.Client_Number), fontStyleBold));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LightGray;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Convert.ToString(client.TaxNumber), fontStyleBold));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LightGray;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(Convert.ToString(client.Identification), fontStyleBold));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.LightGray;
                _pdfTable.AddCell(_pdfCell);

                _pdfTable.CompleteRow();

            }

            #endregion
        }


    }
}
