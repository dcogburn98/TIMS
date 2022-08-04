using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfSharp.Drawing;

namespace TIMSServerModel
{
    public class Invoice
    {
        public int invoiceNumber;
        public Customer customer;
        public Employee employee;

        public List<InvoiceItem> items;
        public decimal subtotal;
        public decimal taxableTotal;
        public decimal taxRate;
        public decimal taxAmount;
        public decimal total;
        public decimal cost;
        public decimal profit;
        public List<Payment> payments;
        public decimal totalPayments;

        public bool containsAgeRestrictedItem;
        public DateTime customerBirthdate;
        public string attentionLine = string.Empty;
        public string PONumber = string.Empty;
        public string invoiceMessage = string.Empty;

        public bool savedInvoice;
        public DateTime savedInvoiceTime;
        public DateTime invoiceCreationTime;
        public DateTime invoiceFinalizedTime;

        public bool finalized;
        public bool voided;

        public int invoicePages;
        public int currentPage;

        public Invoice()
        {
            items = new List<InvoiceItem>();
            payments = new List<Payment>();
        }

        public void RenderPage (XGraphics gfx)
        {
            invoicePages = (items.Count / 32 + (items.Count % 32 > 0 ? 1 : 0));
            bool lastPage = false;
            if (currentPage == invoicePages)
                lastPage = true;

            XRect rect;
            XPen pen = new XPen(XColors.Black, 1);
            XFont fontH1 = new XFont("Times", 18, XFontStyle.Bold);
            XFont fontH2 = new XFont("Times", 12);
            XFont font = new XFont("Times", 8);

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
            gfx.DrawString(employee.fullName, font, XBrushes.Black, 200, 37);
            gfx.DrawString(employee.employeeNumber.ToString(), font, XBrushes.Black, 200, 37 + font.GetHeight());
            gfx.DrawString(employee.position, font, XBrushes.Black, 200, 37 + 2 * font.GetHeight());
            //gfx.DrawString(model.RetrievePropertyString("Store Name"), font, XBrushes.Black, 340, 37);
            //gfx.DrawString(model.RetrievePropertyString("Store Number").ToString(), font, XBrushes.Black, 340, 37 + font.GetHeight());
            //gfx.DrawString(model.RetrievePropertyString("Store Phone Number"), font, XBrushes.Black, 340, 37 + 2 * font.GetHeight());
            #endregion

            #region Invoice Information Area
            rect = new XRect(440, 50, 140, 40);
            gfx.DrawRectangle(pen, XBrushes.White, rect);
            gfx.DrawString("Time: ", font, XBrushes.Black, 444, 60);
            //gfx.DrawString("Date: ", font, XBrushes.Black, 500, 60);
            gfx.DrawString(invoiceFinalizedTime.ToString("hh:mm tt"), font, XBrushes.Black, 465, 60);
            gfx.DrawString("Date: " + invoiceFinalizedTime.ToString("MM/dd/yyyy"), font, XBrushes.Black, 520, 60);
            gfx.DrawString("Page: " + currentPage.ToString() + "/" + invoicePages.ToString(), font, XBrushes.Black, 543, 85);
            gfx.DrawBarCode(PdfSharp.Drawing.BarCodes.BarCode.FromType(
                PdfSharp.Drawing.BarCodes.CodeType.Code3of9Standard,
                invoiceNumber.ToString(),
                new XSize(130, 15)),
                new XPoint(445, 63));
            gfx.DrawString("Invoice Number: " + invoiceNumber.ToString(), font, XBrushes.Black, new PointF(444, 85));
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
            gfx.DrawString("Tax Exemption:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 241.5f, customerInfoOrigin.Y + 10 + 2 * (float)font.GetHeight()));
            gfx.DrawString("PO#:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 275, customerInfoOrigin.Y + 10 + 3 * (float)font.GetHeight()));
            gfx.DrawString("Terms:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 269.5f, customerInfoOrigin.Y + 10 + 4 * (float)font.GetHeight()));

            //gfx.DrawString("Anticipated Delivery:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 224.5f, customerInfoOrigin.Y + 10));
            gfx.DrawString(attentionLine, font, XBrushes.Black, new PointF(customerInfoOrigin.X + 300, customerInfoOrigin.Y + 10 + (float)font.GetHeight()));
            gfx.DrawString(customer.taxExempt ? "True" : "False", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 300, customerInfoOrigin.Y + 10 + 2 * (float)font.GetHeight()));
            gfx.DrawString(PONumber, font, XBrushes.Black, new PointF(customerInfoOrigin.X + 300, customerInfoOrigin.Y + 10 + 3 * (float)font.GetHeight()));
            //gfx.DrawString("Terms:", font, XBrushes.Black, new PointF(customerInfoOrigin.X + 269.5f, customerInfoOrigin.Y + 10 + 4 * (float)font.GetHeight()));


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
            gfx.DrawString("Item Number", fontH2, XBrushes.White, itemInfoOrigin.X + 22, itemInfoOrigin.Y + 12);
            gfx.DrawString("Line", fontH2, XBrushes.White, itemInfoOrigin.X + 124, itemInfoOrigin.Y + 12);
            gfx.DrawString("Description", fontH2, XBrushes.White, itemInfoOrigin.X + 222, itemInfoOrigin.Y + 12);
            gfx.DrawString("Qty", fontH2, XBrushes.White, itemInfoOrigin.X + 351, itemInfoOrigin.Y + 12);
            gfx.DrawString("List", fontH2, XBrushes.White, itemInfoOrigin.X + 400, itemInfoOrigin.Y + 12);
            gfx.DrawString("Price", fontH2, XBrushes.White, itemInfoOrigin.X + 459, itemInfoOrigin.Y + 12);
            gfx.DrawString("Total", fontH2, XBrushes.White, itemInfoOrigin.X + 518, itemInfoOrigin.Y + 12);

            int row = 0;
            List<InvoiceItem> pageItems = new List<InvoiceItem>();
            for (int i = 0; i != (!lastPage ? 32 : items.Count % 32); i++)
            {
                //if ((currentPage - 1) * 32 + i > items.Count - 1)
                //    break;
                pageItems.Add(items[(currentPage - 1) * 32 + i]);
            }
            foreach (InvoiceItem item in pageItems)
            {
                gfx.DrawString(item.itemNumber, font, XBrushes.Black, itemInfoOrigin.X + 4, itemInfoOrigin.Y + 27 + (row * font.GetHeight()));
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
            rect = new XRect(paymentInformationOrigin.X, paymentInformationOrigin.Y + 60, 280, 20);
            gfx.DrawRectangle(pen, XBrushes.LightGray, rect);
            if (lastPage)
            {
                gfx.DrawString("Subtotal: " + subtotal.ToString("C"), font, XBrushes.Black, new PointF(paymentInformationOrigin.X + 130, paymentInformationOrigin.Y + 10));
                gfx.DrawString("Tax (" + taxRate.ToString("P") + "): " + taxAmount.ToString("C"), font, XBrushes.Black, new PointF(paymentInformationOrigin.X + 112, paymentInformationOrigin.Y + 10 + (float)font.GetHeight()));
                gfx.DrawString("Service Fee:", font, XBrushes.Black, new PointF(paymentInformationOrigin.X + 120, paymentInformationOrigin.Y + 10 + 2 * (float)font.GetHeight()));
                gfx.DrawString("Delivery Fee:", font, XBrushes.Black, new PointF(paymentInformationOrigin.X + 117, paymentInformationOrigin.Y + 10 + 3 * (float)font.GetHeight()));

                gfx.DrawString("TOTAL: " + total.ToString("C"), fontH1, XBrushes.Black, new PointF(paymentInformationOrigin.X + 80, paymentInformationOrigin.Y + 75));
            }
            else
            {
                gfx.DrawString("CONTINUED ON NEXT PAGE", fontH1, XBrushes.Black, new PointF(paymentInformationOrigin.X + 15, paymentInformationOrigin.Y + 75));
            }
            #endregion


            XGraphicsState state = gfx.Save();
            gfx.Restore(state);
        }
    }
}
