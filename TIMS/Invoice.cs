using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace TIMS
{
    public class Invoice
    {
        public int invoiceNumber;
        public Customer customer;
        public Employee employee;

        public List<InvoiceItem> items;
        public float subtotal;
        public float taxableTotal;
        public float taxRate;
        public float taxAmount;
        public float total;
        public List<Payment> payments;
        public float totalPayments;

        public bool containsAgeRestrictedItem;
        public DateTime customerBirthdate;
        public string attentionLine;
        public string PONumber;
        public string invoiceMessage;

        public bool savedInvoice;
        public DateTime savedInvoiceTime;

        public Invoice()
        {
            items = new List<InvoiceItem>();
        }

        public void CreateDocument(XGraphics gfx)
        {
            XRect rect;
            XPen pen = new XPen(XColors.Black, 1);
            double x = 50, y = 100;
            XFont fontH1 = new XFont("Times", 18, XFontStyle.Bold);
            XFont fontH2 = new XFont("Times", 12);
            XFont font = new XFont("Times", 8);
            XFont fontItalic = new XFont("Times", 12, XFontStyle.BoldItalic);
            double ls = fontH2.GetHeight();

            #region Invoice Header
            rect = new XRect(440, 10, 140, 30);
            gfx.DrawRectangle(pen, XBrushes.LightGray, rect);
            gfx.DrawString("Invoice", fontH1, XBrushes.Black, 483, 30);
            #endregion

            #region Employee and Store Information Area
            rect = new XRect(130, 10, 300, 80);
            gfx.DrawRectangle(pen, XBrushes.White, rect);
            rect = new XRect(130, 10, 300, 16);
            gfx.DrawRectangle(pen, XBrushes.DarkSlateGray, rect);
            gfx.DrawLine(pen, new Point(280, 10), new Point(280, 90));
            gfx.DrawString("Employee Information", fontH2, XBrushes.White, 150, 22);
            gfx.DrawString("Employee Name: ", font, XBrushes.Black, 134, 37);
            gfx.DrawString("Employee Number: ", font, XBrushes.Black, 134, 37 + font.GetHeight());
            gfx.DrawString("Employee Position: ", font, XBrushes.Black, 134, 37 + 2 * font.GetHeight());
            gfx.DrawString("Store Information", fontH2, XBrushes.White, 315, 22);
            gfx.DrawString("Store Name: ", font, XBrushes.Black, 284, 37);
            gfx.DrawString("Store Number: ", font, XBrushes.Black, 284, 37 + font.GetHeight());
            gfx.DrawString("Phone Number: ", font, XBrushes.Black, 284, 37 + 2 * font.GetHeight());
            #endregion

            #region Invoice Information Area
            rect = new XRect(440, 50, 140, 40);
            gfx.DrawRectangle(pen, XBrushes.White, rect);
            gfx.DrawString("Time: ", font, XBrushes.Black, 444, 60);
            gfx.DrawString("Date: ", font, XBrushes.Black, 500, 60);
            gfx.DrawString("Page: 1/1", font, XBrushes.Black, 530, 85);
            #endregion

            #region Customer Information Area
            PointF customerInfoOrigin = new PointF(20, 100);
            int customerInfoHeight = 60;
            rect = new XRect(customerInfoOrigin, new SizeF(560, customerInfoHeight));
            gfx.DrawRectangle(pen, XBrushes.White, rect);
            gfx.DrawString(customer.customerNumber, font, XBrushes.Black, new PointF(customerInfoOrigin.X + 4, customerInfoOrigin.Y + 10));
            gfx.DrawString(customer.customerName, font, XBrushes.Black, new PointF(customerInfoOrigin.X + 4, customerInfoOrigin.Y + 10 + (float)font.GetHeight()));
            gfx.DrawString(customer.mailingAddress.Split(',')[0], font, XBrushes.Black, new PointF(customerInfoOrigin.X + 4, customerInfoOrigin.Y + 10 + 2 * (float)font.GetHeight()));
            gfx.DrawString(
                customer.mailingAddress.Split(',')[1].Trim() + "," + 
                customer.mailingAddress.Split(',')[2] + 
                customer.mailingAddress.Split(',')[3] + "," +
                customer.mailingAddress.Split(',')[4], font, XBrushes.Black, new PointF(customerInfoOrigin.X + 4, customerInfoOrigin.Y + 10 + 3 * (float)font.GetHeight()));
            gfx.DrawString("Anticipated Delivery:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 224.5f, customerInfoOrigin.Y + 10));
            gfx.DrawString("Attention:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 260, customerInfoOrigin.Y + 10 + (float)font.GetHeight()));
            gfx.DrawString("Tax Exemption:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 241.5f, customerInfoOrigin.Y + 10 + 2*(float)font.GetHeight()));
            gfx.DrawString("PO#:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 275, customerInfoOrigin.Y + 10 + 3*(float)font.GetHeight()));
            gfx.DrawString("Terms:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 269.5f, customerInfoOrigin.Y + 10 + 4*(float)font.GetHeight()));


            #endregion

            #region Item Information Area
            PointF itemInfoOrigin = new PointF(20, 170);
            float itemInfoheight = 320;

            rect = new XRect(itemInfoOrigin, new SizeF(560, itemInfoheight));
            gfx.DrawRectangle(pen, XBrushes.White, rect);
            rect = new XRect(itemInfoOrigin, new SizeF(560, 16));
            gfx.DrawRectangle(pen, XBrushes.DarkSlateGray, rect);
            gfx.DrawLine(pen, new PointF(itemInfoOrigin.X + 110, itemInfoOrigin.Y), new PointF(itemInfoOrigin.X + 110, itemInfoOrigin.Y + itemInfoheight));
            gfx.DrawLine(pen, new PointF(itemInfoOrigin.X + 160, itemInfoOrigin.Y), new PointF(itemInfoOrigin.X + 160, itemInfoOrigin.Y + itemInfoheight));
            gfx.DrawLine(pen, new PointF(itemInfoOrigin.X + 340, itemInfoOrigin.Y), new PointF(itemInfoOrigin.X + 340, itemInfoOrigin.Y + itemInfoheight));
            gfx.DrawLine(pen, new PointF(itemInfoOrigin.X + 380, itemInfoOrigin.Y), new PointF(itemInfoOrigin.X + 380, itemInfoOrigin.Y + itemInfoheight));
            gfx.DrawLine(pen, new PointF(itemInfoOrigin.X + 440, itemInfoOrigin.Y), new PointF(itemInfoOrigin.X + 440, itemInfoOrigin.Y + itemInfoheight));
            gfx.DrawLine(pen, new PointF(itemInfoOrigin.X + 500, itemInfoOrigin.Y), new PointF(itemInfoOrigin.X + 500, itemInfoOrigin.Y + itemInfoheight));
            gfx.DrawString("Item Number",   fontH2, XBrushes.White, itemInfoOrigin.X + 22,  itemInfoOrigin.Y + 12);
            gfx.DrawString("Line",          fontH2, XBrushes.White, itemInfoOrigin.X + 124, itemInfoOrigin.Y + 12);
            gfx.DrawString("Description",   fontH2, XBrushes.White, itemInfoOrigin.X + 222, itemInfoOrigin.Y + 12);
            gfx.DrawString("Qty",           fontH2, XBrushes.White, itemInfoOrigin.X + 351, itemInfoOrigin.Y + 12);
            gfx.DrawString("List",          fontH2, XBrushes.White, itemInfoOrigin.X + 400, itemInfoOrigin.Y + 12);
            gfx.DrawString("Price",         fontH2, XBrushes.White, itemInfoOrigin.X + 459, itemInfoOrigin.Y + 12);
            gfx.DrawString("Total",         fontH2, XBrushes.White, itemInfoOrigin.X + 518, itemInfoOrigin.Y + 12);

            int row = 0;
            foreach (InvoiceItem item in items)
            {
                gfx.DrawString(item.itemNumber, font, XBrushes.Black, itemInfoOrigin.X + 4, itemInfoOrigin.Y + 27 + (row*font.GetHeight()));
                gfx.DrawString(item.productLine, font, XBrushes.Black, itemInfoOrigin.X + 127, itemInfoOrigin.Y + 27 + (row * font.GetHeight()));
                gfx.DrawString(item.itemName, font, XBrushes.Black, itemInfoOrigin.X + 165, itemInfoOrigin.Y + 27 + (row * font.GetHeight()));
                gfx.DrawString(item.quantity.ToString(), font, XBrushes.Black, itemInfoOrigin.X + 355, itemInfoOrigin.Y + 27 + (row * font.GetHeight()));
                gfx.DrawString(item.listPrice.ToString("C"), font, XBrushes.Black, itemInfoOrigin.X + 390, itemInfoOrigin.Y + 27 + (row * font.GetHeight()));
                gfx.DrawString(item.price.ToString("C"), font, XBrushes.Black, itemInfoOrigin.X + 450, itemInfoOrigin.Y + 27 + (row * font.GetHeight()));
                gfx.DrawString(item.total.ToString("C"), font, XBrushes.Black, itemInfoOrigin.X + 510, itemInfoOrigin.Y + 27 + (row * font.GetHeight()));

                row++;
            }
            #endregion

            #region Payment and Total Information Area
            PointF paymentInformationOrigin = new PointF(300, itemInfoOrigin.Y + itemInfoheight + 10);
            rect = new XRect(paymentInformationOrigin, new SizeF(280, 150));
            gfx.DrawRectangle(pen, XBrushes.White, rect);
            gfx.DrawBarCode(PdfSharp.Drawing.BarCodes.BarCode.FromType(PdfSharp.Drawing.BarCodes.CodeType.Code3of9Standard, invoiceNumber.ToString(), new XSize(280, 40)), new XPoint(paymentInformationOrigin.X, paymentInformationOrigin.Y + 160));
            gfx.DrawString(invoiceNumber.ToString(), font, XBrushes.Black, new PointF(300, paymentInformationOrigin.Y + 210));
            #endregion

            y += 200;

            // Draw some text
            //gfx.DrawString("Create PDF on the fly with PDFsharp",
            //    fontH1, XBrushes.Black, x, x);
            gfx.DrawString("With PDFsharp you can use the same code to draw graphic, " +
                "text and images on different targets.", fontH2, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("The object used for drawing is the XGraphics object.",
                fontH2, XBrushes.Black, x, y);
            y += 2 * ls;

            // Draw some more text
            y += 60 + 2 * ls;
            gfx.DrawString("With XGraphics you can draw on a PDF page as well as " +
                "on any System.Drawing.Graphics object.", fontH2, XBrushes.Black, x, y);
            y += ls * 1.1;
            gfx.DrawString("Use the same code to", fontH2, XBrushes.Black, x, y);
            x += 10;
            y += ls * 1.1;
            gfx.DrawString("• draw on a newly created PDF page", fontH2, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("• draw above or beneath of the content of an existing PDF page",
                fontH2, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("• draw in a window", fontH2, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("• draw on a printer", fontH2, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("• draw in a bitmap image", fontH2, XBrushes.Black, x, y);
            x -= 10;
            y += ls * 1.1;
            gfx.DrawString("You can also import an existing PDF page and use it like " +
                "an image, e.g. draw it on another PDF page.", fontH2, XBrushes.Black, x, y);
            y += ls * 1.1 * 2;
            gfx.DrawString("Imported PDF pages are neither drawn nor printed; create a " +
                "PDF file to see or print them!", fontItalic, XBrushes.Firebrick, x, y);
            y += ls * 1.1;
            gfx.DrawString("Below this text is a PDF form that will be visible when " +
                "viewed or printed with a PDF viewer.", fontItalic, XBrushes.Firebrick, x, y);
            y += ls * 1.1;
            XGraphicsState state = gfx.Save();
            gfx.Restore(state);
        }
    }
}
