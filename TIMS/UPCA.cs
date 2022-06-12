using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfSharp.Drawing;

namespace TIMS
{
    public class UPCA
    {
        // This is the nomimal size recommended by the UCC.
        private float _fFontSize = 8.0f;
        private float Width = 108.0f;
        private float Height = 72.0f;
        private float Scale = 1.0f;
        public XPoint pt = new XPoint(0, 0);

        // Left Hand Digits.
        private string[] _aLeft = { "0001101", "0011001", "0010011", "0111101",
                             "0100011", "0110001", "0101111", "0111011",
                             "0110111", "0001011" };

        // Right Hand Digits.
        private string[] _aRight = { "1110010", "1100110", "1101100", "1000010",
                              "1011100", "1001110", "1010000", "1000100",
                              "1001000", "1110100" };

        private string _sQuiteZone = "0000000000";
        private string _sLeadTail = "101";
        private string _sSeparator = "01010";

        private string ProductType;
        private string ManufacturerCode;
        private string ProductCode;
        private string ChecksumDigit;

        public UPCA(string UPC)
        {
            if (UPC.Length != 12 && UPC.Length != 11)
                throw new Exception("UPCA must contain 12 digits with the checksum or 11 digits without it!");

            if (UPC.Length == 12)
            {
                ProductType = UPC.Substring(0, 1);
                ManufacturerCode = UPC.Substring(1, 5);
                ProductCode = UPC.Substring(6, 5);
                ChecksumDigit = UPC.Substring(11, 1);
                //CalculateChecksumDigit();

                //if (ChecksumDigit != tempChecksumDigit)
                //    throw new Exception("Invalid UPC");
            }
            else
            {
                ProductType = UPC.Substring(0, 1);
                ManufacturerCode = UPC.Substring(1, 5);
                ProductCode = UPC.Substring(6, 5);
                CalculateChecksumDigit();
            }
        }

        public void CalculateChecksumDigit()
        {
            string sTemp = this.ProductType + this.ManufacturerCode + this.ProductCode;
            int iSum = 0;
            int iDigit = 0;

            // Calculate the checksum digit here.
            for (int i = 1; i <= sTemp.Length; i++)
            {
                iDigit = Convert.ToInt32(sTemp.Substring(i - 1, 1));
                if (i % 2 == 0)
                {    // even
                    iSum += iDigit * 1;
                }
                else
                {    // odd
                    iSum += iDigit * 3;
                }
            }

            int iCheckSum = (10 - (iSum % 10)) % 10;
            this.ChecksumDigit = iCheckSum.ToString();

        }

        public static string CalculateChecksumDigit(string UPC11)
        {
            string sTemp = UPC11;
            int iSum = 0;
            int iDigit = 0;

            // Calculate the checksum digit here.
            for (int i = 1; i <= sTemp.Length; i++)
            {
                iDigit = Convert.ToInt32(sTemp.Substring(i - 1, 1));
                if (i % 2 == 0)
                {    // even
                    iSum += iDigit * 1;
                }
                else
                {    // odd
                    iSum += iDigit * 3;
                }
            }

            int iCheckSum = (10 - (iSum % 10)) % 10;
            string Checksum = iCheckSum.ToString();
            return UPC11 + Checksum;
        }

        private string ConvertToDigitPatterns(string inputNumber, string[] patterns)
        {
            System.Text.StringBuilder sbTemp = new StringBuilder();
            int iIndex = 0;
            for (int i = 0; i < inputNumber.Length; i++)
            {
                iIndex = Convert.ToInt32(inputNumber.Substring(i, 1));
                sbTemp.Append(patterns[iIndex]);
            }
            return sbTemp.ToString();
        }

        public void RenderBarcode(XGraphics g, double width, double height)
        {
            //g.DrawRectangle(XPens.Black, 0, 0, 108.0d, 72.0d);
            width *= this.Scale;
            height *= this.Scale;

            // A upc-a excluding 2 or 5 digit supplement information 
            // should be a total of 113 modules wide. 
            // Supplement information is typically 
            // used for periodicals and books.
            double lineWidth = width / 113f;

            // Save the GraphicsState.
            XGraphicsState gs = g.Save();

            // Set the PageUnit to Inch because all of 
            // our measurements are in inches.
            //g.PageUnit = System.Drawing.GraphicsUnit.Inch;

            // Set the PageScale to 1, so an inch will represent a true inch.
            //g.PageScale = 1;

            XBrush brush = XBrushes.Black;

            double xPosition = pt.X;

            StringBuilder strbUPC = new StringBuilder();

            double xStart = pt.X;
            double yStart = pt.Y;
            double xEnd = 0;

            XFont font =
              new XFont("Courier", this._fFontSize * this.Scale);

            // Calculate the Check Digit.
            //this.CalculateChecksumDigit();

            // Build the UPC Code.
            strbUPC.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{1}{0}",
                        this._sQuiteZone, this._sLeadTail,
                        ConvertToDigitPatterns(this.ProductType, this._aLeft),
                        ConvertToDigitPatterns(this.ManufacturerCode, this._aLeft),
                        this._sSeparator,
                        ConvertToDigitPatterns(this.ProductCode, this._aRight),
                        ConvertToDigitPatterns(this.ChecksumDigit, this._aRight));

            string sTempUPC = strbUPC.ToString();

            double fTextHeight = g.MeasureString(sTempUPC, font).Height;

            // Draw the barcode lines.
            for (int i = 0; i < strbUPC.Length; i++)
            {
                if (sTempUPC.Substring(i, 1) == "1")
                {
                    if (xStart == pt.X)
                        xStart = xPosition;

                    // Save room for the UPC number below the bar code.
                    if ((i > 19 && i < 56) || (i > 59 && i < 95))
                        // Draw space for the number
                        g.DrawRectangle(brush, xPosition, yStart, lineWidth, height - fTextHeight);
                    else
                        // Draw a full line.
                        g.DrawRectangle(brush, xPosition, yStart, lineWidth, height);
                }

                xPosition += lineWidth;
                xEnd = xPosition;
            }

            // Draw the upc numbers below the line.
            xPosition = xStart - g.MeasureString(this.ProductType, font).Width;
            double yPosition = yStart + (height - fTextHeight);

            // Draw Product Type.
            //g.DrawString(this.ProductType, font, brush, new System.Drawing.PointF((float)xPosition, (float)yPosition));

            // Each digit is 7 modules wide, therefore the MFG_Number 
            // is 5 digits wide so
            // 5 * 7 = 35, then add 3 for the LeadTrailer 
            // Info and another 7 for good measure,
            // that is where the 45 comes from.
            //xPosition += g.MeasureString(this.ProductType, font).Width + 45 * lineWidth - g.MeasureString(this.ManufacturerCode, font).Width;

            // Draw MFG Number.
            //g.DrawString(this.ManufacturerCode, font, brush, new System.Drawing.PointF((float)xPosition, (float)yPosition));

            // Add the width of the MFG Number and 5 modules for the separator.
           // xPosition += g.MeasureString(this.ManufacturerCode, font).Width + 5 * lineWidth;

            // Draw Product ID.
            //g.DrawString(this.ProductCode, font, brush, new System.Drawing.PointF((float)xPosition, (float)yPosition));

            // Each digit is 7 modules wide, therefore 
            // the Product Id is 5 digits wide so
            // 5 * 7 = 35, then add 3 for the LeadTrailer 
            // Info, + 8 more just for spacing
            // that is where the 46 comes from.
            //xPosition += 46 * lineWidth;

            // Draw Check Digit.
            //g.DrawString(this.ChecksumDigit, font, brush, new System.Drawing.PointF((float)xPosition, (float)yPosition));

            // Restore the GraphicsState.
            g.Restore(gs);

        }

        public System.Drawing.Bitmap CreateBitmap()
        {
            float tempWidth = (this.Width * this.Scale) * 100;
            float tempHeight = (this.Height * this.Scale) * 100;

            System.Drawing.Bitmap bmp =
               new System.Drawing.Bitmap((int)tempWidth, (int)tempHeight);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            this.RenderBarcode(XGraphics.FromGraphics(g, new XSize(tempWidth, tempHeight), XGraphicsUnit.Inch), 72, 108);
            g.Dispose();
            return bmp;
        }
    }

    public class BarcodeSheet
    {
        public double topMargin = 0.0d;
        public double bottomMargin = 0.15625d;
        public double leftMargin = 0.3125d;
        //public double rightMargin = 0.5d;
        public int labelColumns = 3;
        public int labelRows = 9;
        public double labelVerticalSpacing = 0.0d;
        public double labelHorizontalSpacing = 0.0d;
        public double labelWidth = 2.5d;
        public double labelHeight = 1.1875d;
        public double ppi = 72.0d;
        public int totalLabels;

        public bool drawPrice = true;
        public bool drawItemNumber = true;
        public bool drawBarcode = true;
        public bool drawBarcodeString = false;

        public int totalPages = 1;
        public int currentPage = 1;

        public List<Item> LabelItems;

        public BarcodeSheet(List<Item> items)
        {
            totalLabels = labelColumns * labelRows;
            topMargin *= ppi;
            bottomMargin *= ppi;
            leftMargin *= ppi;
            labelVerticalSpacing *= ppi;
            labelHorizontalSpacing *= ppi;
            labelWidth *= ppi;
            labelHeight *= ppi;

            LabelItems = items;

            totalPages = LabelItems.Count % totalLabels == 0 ? 
                LabelItems.Count / totalLabels : 
                LabelItems.Count / totalLabels + 1;
        }

        public BarcodeSheet(List<string> barcodes)
        {
            totalLabels = labelColumns * labelRows;
            topMargin *= ppi;
            bottomMargin *= ppi;
            leftMargin *= ppi;
            labelVerticalSpacing *= ppi;
            labelHorizontalSpacing *= ppi;

            foreach (string barcode in barcodes)
            {
                LabelItems.Add(DatabaseHandler.SqlRetrieveItem(barcode));
            }
        }

        public void RenderBarcodePage(XGraphics gfx)
        {
            //labelWidth = ((gfx.PageSize.Width - (2 * sideMargin)) / labelColumns);
            //labelHeight = ((gfx.PageSize.Height - (bottomMargin + topMargin)) / labelRows);
            XFont smallFont = new XFont("Arial", 8, XFontStyle.Bold);
            XFont priceFont = new XFont("Arial", 34, XFontStyle.BoldItalic);
            gfx.DrawString((labelWidth / ppi).ToString(), smallFont, XBrushes.Black, 0, 10);
            gfx.DrawString((labelHeight / ppi).ToString(), smallFont, XBrushes.Black, 0, 25);

            #region
            for (int i = 0; i != labelColumns; i++)
            {
                bool broken = false;
                for (int j = 0; j != labelRows; j++)
                {
                    //gfx.DrawRectangle(XPens.Black, new System.Drawing.RectangleF(
                    //    (float)((i * labelWidth) + leftMargin),
                    //    (float)((j * labelHeight) + topMargin),
                    //    (float)labelWidth,
                    //    (float)labelHeight));

                    Item item = LabelItems[(i * labelRows) + j + ((currentPage - 1) * totalLabels)];

                    string itemName = item.itemName;
                    double x = 0;
                    if (gfx.MeasureString(itemName, smallFont).Width < labelWidth * 0.95d)
                        x = ((i * labelWidth) + leftMargin + ((labelWidth / 2) - (gfx.MeasureString(itemName, smallFont).Width / 2)));
                    else
                        x = i * labelWidth + 10 + leftMargin;
                    while (gfx.MeasureString(itemName, smallFont).Width > labelWidth * 0.90)
                        itemName = itemName.Substring(0, itemName.Length - 1);
                    gfx.DrawString(
                        itemName,
                        smallFont,
                        XBrushes.Black,
                        new XPoint(
                            (float)x,
                            (float)((j * labelHeight) + topMargin) + (labelHeight * 0.2d) - smallFont.GetHeight()));

                    string price = item.greenPrice.ToString("C");
                    gfx.DrawString(
                        price,
                        priceFont,
                        XBrushes.Black,
                        new XPoint(
                            (float)((i * labelWidth) + leftMargin + ((labelWidth / 2) - (gfx.MeasureString(price, priceFont).Width / 2))),
                            (float)((j * labelHeight) + topMargin) + (labelHeight * 0.55d) - smallFont.GetHeight()));

                    string itemNumber = item.productLine + " " + item.itemNumber;
                    gfx.DrawString(
                        itemNumber,
                        smallFont,
                        XBrushes.Black,
                        new XPoint(
                            (float)((i * labelWidth) + leftMargin + ((labelWidth / 2) - (gfx.MeasureString(itemNumber, smallFont).Width / 2))),
                            (float)((j * labelHeight) + topMargin) + (labelHeight * 0.7d) - smallFont.GetHeight()));

                    //string barcode = 
                    //    DatabaseHandler.SqlRetrieveBarcode(item) != null ? 
                    //    DatabaseHandler.SqlRetrieveBarcode(item)[0] : null;
                    string barcode = "047708762106";
                    if (barcode != null)
                    {
                        UPCA upc = new UPCA(barcode);
                        upc.pt = new XPoint(
                            (float)((i * labelWidth) + leftMargin),
                            (float)((j * labelHeight) + topMargin) + (labelHeight * 0.65d));
                        upc.RenderBarcode(gfx, labelWidth - 10, labelHeight / 4);

                        gfx.DrawString(
                            barcode,
                            smallFont,
                            XBrushes.Black,
                            new XPoint(
                                (float)((i * labelWidth) + leftMargin + ((labelWidth / 2) - (gfx.MeasureString(barcode, smallFont).Width / 2))),
                                (float)((j * labelHeight) + topMargin) + (labelHeight * 1.0d) - smallFont.GetHeight()));
                    }

                    if (((i * labelRows) + j + ((currentPage - 1) * totalLabels)+ 1) == LabelItems.Count)
                    {
                        broken = true;
                        break;
                    }
                }
                if (broken)
                    break;
            }
            #endregion

            XGraphicsState state = gfx.Save();
            gfx.Restore(state);
        }
    }
}
