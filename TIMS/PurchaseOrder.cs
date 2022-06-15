using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfSharp.Drawing;

namespace TIMS
{
    public class PurchaseOrder
    {
        public int PONumber;
        public string supplier;
        public bool finalized;
        public int assignedCheckin;
        public decimal totalCost;
        public decimal totalItems;

        public int pageRows = 60;

        public List<InvoiceItem> items;

        public int currentPage = 1;
        public int totalPages;

        public PurchaseOrder(string supplier)
        {
            PONumber = DatabaseHandler.SqlRetrieveNextPONumber();
            if (PONumber == 0)
                PONumber = 100001;
            this.supplier = supplier;
        }

        public void RenderPage(XGraphics gfx)
        {
            totalPages = (items.Count / pageRows + (items.Count % pageRows > 0 ? 1 : 0));
            XFont font = new XFont("Times", 8.5d);

            double currentLine = font.GetHeight() + 5;

            #region Header Strings
            gfx.DrawString(
                DateTime.Now.ToString(), 
                font, 
                XBrushes.Black, 
                10, 
                currentLine);

            gfx.DrawString(
                "Purchase Order for Supplier: " + supplier, 
                font, 
                XBrushes.Black, 
                (gfx.PageSize.Width / 2) - gfx.MeasureString("Purchase Order for Supplier: " + supplier, font).Width / 2, 
                currentLine);

            gfx.DrawString(
                "Page " + currentPage.ToString() + "/" + totalPages.ToString(), 
                font, 
                XBrushes.Black,
                gfx.PageSize.Width - gfx.MeasureString("Page " + currentPage.ToString() + "/" + totalPages.ToString(), font).Width - 15,
                currentLine);
            currentLine += font.GetHeight();

            gfx.DrawString(
                PONumber.ToString(), 
                font, 
                XBrushes.Black, 
                gfx.PageSize.Width - gfx.MeasureString(PONumber.ToString(), font).Width - 15, 
                currentLine);
            currentLine += font.GetHeight();
            
            gfx.DrawLine(XPens.Black, 0, currentLine, gfx.PageSize.Width, currentLine);
            currentLine += font.GetHeight();
            #endregion



            XGraphicsState state = gfx.Save();
            gfx.Restore(state);
        }
    }
}
