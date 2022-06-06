using PdfSharp;
using PdfSharp.Drawing;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace TIMS.Forms
{
    public partial class ReportViewer : Form
    {
        public bool printed = false;
        Invoice inv;

        Report report;

        UPCA upc;

        BarcodeSheet barcodeSheet;

        public ReportViewer(Invoice inv)
        {
            InitializeComponent();
            CancelButton = closeButton;
            inv.invoicePages = inv.items.Count % 32 != 0 ? inv.items.Count / 32 + 1 : inv.items.Count / 32;
            if (inv.invoicePages > 1)
                nextPageBtn.Enabled = true;
            prevPageBtn.Enabled = false;

            this.inv = inv;
            inv.currentPage = 1;
            pagePreview1.Zoom = PdfSharp.Forms.Zoom.BestFit;
            pagePreview1.SetRenderFunction(inv.RenderPage);
        }

        public ReportViewer(Report reportData)
        {
            InitializeComponent();
            CancelButton = closeButton;
            report = reportData;
            report.ExecuteReport();
            reportData.totalPages = (reportData.Results.Count / reportData.ColumnCount) % 40 != 0 ? (reportData.Results.Count / reportData.ColumnCount) / 40 + 1 : (reportData.Results.Count / reportData.ColumnCount) / 40;
            if (reportData.totalPages > 1)
                nextPageBtn.Enabled = true;
            prevPageBtn.Enabled = false;

            report.currentPage = 1;
            pagePreview1.Zoom = PdfSharp.Forms.Zoom.BestFit;
            pagePreview1.SetRenderFunction(report.RenderPage);
        }

        public ReportViewer(BarcodeSheet sheet)
        {
            InitializeComponent();
            CancelButton = closeButton;
            this.barcodeSheet = sheet;

            pagePreview1.Zoom = PdfSharp.Forms.Zoom.BestFit;
            pagePreview1.SetRenderFunction(sheet.RenderBarcodePage);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.Document = new PrintDocument();
            pd.Document.PrintPage += new PrintPageEventHandler(PrintPage);
            DialogResult result = pd.ShowDialog();

            if (result == DialogResult.OK)
            {
                pd.Document.Print();
                printed = true;
            }
            else
            {
                printed = false;
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Graphics graphics = ev.Graphics;
            graphics.PageUnit = GraphicsUnit.Point;
            XGraphics gfx = XGraphics.FromGraphics(graphics, PageSizeConverter.ToSize(PageSize.A4));
            if (inv != null)
            {
                inv.RenderPage(gfx);

                if (inv.currentPage != inv.invoicePages)
                {
                    ev.HasMorePages = true;
                    inv.currentPage++;
                }
            }
            else if (report != null)
            {
                report.RenderPage(gfx);

                if (report.currentPage != report.totalPages)
                {
                    ev.HasMorePages = true;
                    report.currentPage++;
                }
            }

        }

        private void nextPageBtn_Click(object sender, EventArgs e)
        {
            if (inv != null)
            {
                inv.currentPage++;
                if (inv.currentPage == inv.invoicePages)
                    nextPageBtn.Enabled = false;
                prevPageBtn.Enabled = true;
                pagePreview1.Refresh();
            }
            else if (report != null)
            {
                report.currentPage++;
                if (report.currentPage == report.totalPages)
                    nextPageBtn.Enabled = false;
                prevPageBtn.Enabled = true;
                pagePreview1.Refresh();
            }
        }

        private void prevPageBtn_Click(object sender, EventArgs e)
        {
            if (inv != null)
            {
                inv.currentPage--;
                if (inv.currentPage == 1)
                    prevPageBtn.Enabled = false;
                nextPageBtn.Enabled = true;
                pagePreview1.Refresh();
            }
            else if (report != null)
            {
                report.currentPage--;
                if (report.currentPage == 1)
                    prevPageBtn.Enabled = false;
                nextPageBtn.Enabled = true;
                pagePreview1.Refresh();
            }
        }
    }
}
