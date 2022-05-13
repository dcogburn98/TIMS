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
            this.inv = inv;
            pagePreview1.Zoom = PdfSharp.Forms.Zoom.BestFit;
            pagePreview1.SetRenderFunction(inv.CreateDocument);
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
            inv.CreateDocument(gfx);

            ev.HasMorePages = false;
        }
    }
}
