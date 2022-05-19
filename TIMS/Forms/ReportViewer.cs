using System;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace TIMS.Forms
{
    public partial class ReportViewer : Form
    {
        public bool printed = false;
        Invoice inv;

        public ReportViewer(Invoice inv)
        {
            InitializeComponent();
            CancelButton = closeButton;
            if (inv.invoicePages > 1)
                nextPageBtn.Enabled = true;
            prevPageBtn.Enabled = false;

            this.inv = inv;
            inv.currentPage = 1;
            pagePreview1.Zoom = PdfSharp.Forms.Zoom.BestFit;
            pagePreview1.SetRenderFunction(inv.RenderPage);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintPage);
            pd.Print();
            printed = true;
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Graphics graphics = ev.Graphics;
            graphics.PageUnit = GraphicsUnit.Point;
            XGraphics gfx = XGraphics.FromGraphics(graphics, PageSizeConverter.ToSize(PageSize.A4));
            inv.RenderPage(gfx);

            ev.HasMorePages = false;
        }

        private void nextPageBtn_Click(object sender, EventArgs e)
        {
            inv.currentPage++;
            if (inv.currentPage == inv.invoicePages)
                nextPageBtn.Enabled = false;
            prevPageBtn.Enabled = true;
            pagePreview1.Refresh();
        }

        private void prevPageBtn_Click(object sender, EventArgs e)
        {
            inv.currentPage--;
            if (inv.currentPage == 1)
                prevPageBtn.Enabled = false;
            nextPageBtn.Enabled = true;
            pagePreview1.Refresh();
        }
    }
}
