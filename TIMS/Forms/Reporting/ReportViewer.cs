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
        public Invoice inv;

        public Report report;

        public BarcodeSheet barcodeSheet;

        public PurchaseOrder POSheet;

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
            pagePreview1.PageSize = PageSizeConverter.ToSize(PageSize.Letter);
            pagePreview1.SetRenderFunction(inv.RenderPage);
        }

        public ReportViewer(Report reportData)
        {
            InitializeComponent();
            CancelButton = closeButton;
            report = reportData;
            report.ExecuteReport();
            reportData.totalPages = (reportData.Results.Count / reportData.ColumnCount) % report.pageRows != 0 ? 
                (reportData.Results.Count / reportData.ColumnCount) / report.pageRows + 1 : 
                (reportData.Results.Count / reportData.ColumnCount) / report.pageRows;
            if (reportData.totalPages > 1)
                nextPageBtn.Enabled = true;
            prevPageBtn.Enabled = false;

            report.currentPage = 1;
            pagePreview1.Zoom = PdfSharp.Forms.Zoom.BestFit;
            pagePreview1.PageSize = PageSizeConverter.ToSize(PageSize.Letter);
            pagePreview1.SetRenderFunction(report.RenderPage);
        }

        public ReportViewer(BarcodeSheet sheet)
        {
            InitializeComponent();
            CancelButton = closeButton;
            barcodeSheet = sheet;
            if (barcodeSheet.totalPages > 1)
                nextPageBtn.Enabled = true;
            prevPageBtn.Enabled = false;

            pagePreview1.Zoom = PdfSharp.Forms.Zoom.BestFit;
            pagePreview1.PageSize = PageSizeConverter.ToSize(PageSize.Letter);
            pagePreview1.PageGraphicsUnit = XGraphicsUnit.Point;
            pagePreview1.SetRenderFunction(barcodeSheet.RenderBarcodePage);
        }

        public ReportViewer(PurchaseOrder sheet)
        {
            InitializeComponent();
            CancelButton = closeButton;
            POSheet = sheet;
            POSheet.totalPages =
                POSheet.items.Count / POSheet.pageRows + (POSheet.items.Count % POSheet.pageRows != 0 ? 1 : 0);
            if (POSheet.totalPages > 1)
                nextPageBtn.Enabled = true;
            prevPageBtn.Enabled = false;

            pagePreview1.Zoom = PdfSharp.Forms.Zoom.BestFit;
            pagePreview1.PageSize = PageSizeConverter.ToSize(PageSize.Letter);
            pagePreview1.PageGraphicsUnit = XGraphicsUnit.Point;
            pagePreview1.SetRenderFunction(POSheet.RenderPage);
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
            pd.PrinterSettings.Duplex = Duplex.Simplex;
            
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
            XGraphics gfx = XGraphics.FromGraphics(graphics, PageSizeConverter.ToSize(PageSize.Letter));
            
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
            else if (barcodeSheet != null)
            {
                barcodeSheet.RenderBarcodePage(gfx);

                if (barcodeSheet.currentPage != barcodeSheet.totalPages)
                {
                    ev.HasMorePages = true;
                    barcodeSheet.currentPage++;
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
            else if (barcodeSheet != null)
            {
                barcodeSheet.currentPage++;
                if (barcodeSheet.currentPage == barcodeSheet.totalPages)
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
            else if (barcodeSheet != null)
            {
                barcodeSheet.currentPage--;
                if (barcodeSheet.currentPage == 1)
                    prevPageBtn.Enabled = false;
                nextPageBtn.Enabled = true;
                pagePreview1.Refresh();
            }
        }
    }
}
