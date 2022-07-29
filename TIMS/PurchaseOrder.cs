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
        public decimal shippingCost;
        public decimal totalItems;

        public List<InvoiceItem> items;

        public int pageRows = 45;
        public int currentPage = 1;
        public int totalPages;

        public PurchaseOrder(string supplier)
        {
            PONumber = DatabaseHandler.SqlRetrieveNextPONumber();
            if (PONumber == 0)
                PONumber = 100001;
            this.supplier = supplier;
            items = new List<InvoiceItem>();
        }

        public void RenderPage(XGraphics gfx)
        {
            totalPages = (items.Count / pageRows + (items.Count % pageRows > 0 ? 1 : 0));
            XFont font = new XFont("Times", 8.5d);
            XFont h1 = new XFont("Times", 11, XFontStyle.Bold);

            double currentLine = font.GetHeight() + 5;

            #region Header Strings
            gfx.DrawString( DateTime.Now.ToString(), font, XBrushes.Black, 10, currentLine);
            gfx.DrawString( "Purchase Order", h1, XBrushes.Black, (gfx.PageSize.Width / 2) - gfx.MeasureString("Purchase Order", h1).Width / 2, currentLine);
            gfx.DrawString("Page " + currentPage.ToString() + "/" + totalPages.ToString(), font, XBrushes.Black,
                gfx.PageSize.Width - gfx.MeasureString("Page " + currentPage.ToString() + "/" + totalPages.ToString(), font).Width - 35,
                currentLine);

            currentLine += font.GetHeight();

            gfx.DrawString("Supplier: " + supplier, font, XBrushes.Black, 10, currentLine);
            gfx.DrawString("PO Number: " + PONumber, font, XBrushes.Black, 
                gfx.PageSize.Width - gfx.MeasureString("PO Number: " + PONumber.ToString(), font).Width - 35, 
                currentLine);

            currentLine += font.GetHeight();
            
            gfx.DrawLine(XPens.Black, 0, currentLine, gfx.PageSize.Width, currentLine);
            gfx.DrawLine(XPens.Black, (gfx.PageSize.Width - 35) * 0.33d, currentLine, (gfx.PageSize.Width - 35) * 0.33d, currentLine + 7 * font.GetHeight());
            gfx.DrawLine(XPens.Black, (gfx.PageSize.Width - 35) * 0.67d, currentLine, (gfx.PageSize.Width - 35) * 0.67d, currentLine + 7 * font.GetHeight());
            gfx.DrawLine(XPens.Black, 0, currentLine + 7 * font.GetHeight(), gfx.PageSize.Width, currentLine + 7 * font.GetHeight());
            currentLine += font.GetHeight();
            #endregion

            #region Shipping Information Strings
            double resetLine = currentLine;
            gfx.DrawString("Supplier Information", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.17d - (gfx.MeasureString("Supplier Information", h1).Width) / 2, currentLine);
            currentLine += h1.GetHeight();
            gfx.DrawString(supplier, font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.06d, currentLine);
            currentLine = resetLine;

            string[] mailingAddr = DatabaseHandler.SqlRetrievePropertyString("Mailing Address").Split(',');
            gfx.DrawString("Purchaser Information", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.5d  - (gfx.MeasureString("Purchaser Information", h1).Width) / 2, currentLine);
            currentLine += h1.GetHeight();
            gfx.DrawString(DatabaseHandler.SqlRetrievePropertyString("Store Name"), font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.40d, currentLine);
            currentLine += font.GetHeight();
            gfx.DrawString(mailingAddr[0], font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.40d, currentLine);
            currentLine += font.GetHeight();
            gfx.DrawString(mailingAddr[1].Trim() + ", " + mailingAddr[2] + ", " + mailingAddr[3] + ", " + mailingAddr[4], font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.40d, currentLine);
            currentLine += font.GetHeight();
            gfx.DrawString(DatabaseHandler.SqlRetrievePropertyString("Store Alternate Phone Number"), font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.40d, currentLine);
            currentLine = resetLine;

            string[] shippingAddr = DatabaseHandler.SqlRetrievePropertyString("Store Address").Split(',');
            gfx.DrawString("Shipping Information", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.83d - (gfx.MeasureString("Shipping Information", h1).Width) / 2, currentLine);
            currentLine += h1.GetHeight();
            gfx.DrawString(DatabaseHandler.SqlRetrievePropertyString("Store Name"), font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.73d, currentLine);
            currentLine += font.GetHeight();
            gfx.DrawString(shippingAddr[0], font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.73d, currentLine);
            currentLine += font.GetHeight();
            gfx.DrawString(shippingAddr[1].Trim() + ", " + shippingAddr[2] + ", " + shippingAddr[3] + ", " + shippingAddr[4], font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.73d, currentLine);
            currentLine += font.GetHeight();
            gfx.DrawString(DatabaseHandler.SqlRetrievePropertyString("Store Phone Number"), font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.73d, currentLine);
            currentLine += 3 * font.GetHeight();

            #endregion

            gfx.DrawString("Product Line", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.09d - (gfx.MeasureString("Product Line", h1).Width) / 2, currentLine);
            gfx.DrawString("Item Number", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.29d - (gfx.MeasureString("Item Number", h1).Width) / 2, currentLine);
            gfx.DrawString("Qty", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.49d - (gfx.MeasureString("Qty", h1).Width) / 2, currentLine);
            gfx.DrawString("Unit Price", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.69d - (gfx.MeasureString("Unit Price", h1).Width) / 2, currentLine);
            gfx.DrawString("Ext. Price", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.89d - (gfx.MeasureString("Ext. Price", h1).Width) / 2, currentLine);
            currentLine += h1.GetHeight();

            for (int i = 0; i != pageRows; i++)
            {
                int index = (currentPage - 1) * pageRows + i;
                if (index > items.Count - 1)
                    break;
                gfx.DrawString(items[index].productLine, font, XBrushes.Black,
                    (gfx.PageSize.Width - 35) * 0.09d - (gfx.MeasureString(items[index].productLine, font).Width) / 2, currentLine);
                gfx.DrawString(items[index].itemNumber, font, XBrushes.Black,
                    (gfx.PageSize.Width - 35) * 0.29d - (gfx.MeasureString(items[index].itemNumber, font).Width) / 2, currentLine);
                gfx.DrawString(items[index].quantity.ToString(), font, XBrushes.Black,
                    (gfx.PageSize.Width - 35) * 0.49d - (gfx.MeasureString(items[index].quantity.ToString(), font).Width) / 2, currentLine);
                gfx.DrawString(items[index].price.ToString("C"), font, XBrushes.Black,
                    (gfx.PageSize.Width - 35) * 0.69d - (gfx.MeasureString(items[index].price.ToString("C"), font).Width) / 2, currentLine);
                gfx.DrawString(items[index].total.ToString("C"), font, XBrushes.Black,
                    (gfx.PageSize.Width - 35) * 0.89d - (gfx.MeasureString(items[index].total.ToString("C"), font).Width) / 2, currentLine);
                currentLine += 1.2d * font.GetHeight();
            }

            if (currentPage == totalPages)
            {
                gfx.DrawLine(XPens.Black, 0, currentLine, gfx.PageSize.Width, currentLine);
                currentLine += h1.GetHeight();
                gfx.DrawString("Subtotal:", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.8d - (gfx.MeasureString("Subtotal:", h1).Width), currentLine);
                gfx.DrawString("Shipping:", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.8d - (gfx.MeasureString("Shipping:", h1).Width), currentLine + h1.GetHeight());
                gfx.DrawString("Total:", h1, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.8d - (gfx.MeasureString("Total:", h1).Width), currentLine + 2 * h1.GetHeight());
                gfx.DrawString(this.totalCost.ToString("C"), font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.84d, currentLine);
                gfx.DrawString(this.shippingCost.ToString("C"), font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.84d, currentLine + h1.GetHeight());
                gfx.DrawString((totalCost + shippingCost).ToString("C"), font, XBrushes.Black,
                (gfx.PageSize.Width - 35) * 0.84d, currentLine + 2 * h1.GetHeight());
            }


            XGraphicsState state = gfx.Save();
            gfx.Restore(state);
        }
    }
}
