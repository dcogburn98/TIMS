using System;
using System.Collections.Generic;
using System.Drawing;

using PdfSharp.Drawing;

namespace TIMS
{
    public class Report
    {
        public string ReportName;
        public string ReportShortcode;

        public List<string> Fields;
        public string DataSource;
        public List<string> Conditions;
        public List<string> Totals;

        public string Query;
        public int ColumnCount;
        public List<object> Results;

        public int totalPages;
        public int currentPage;

        public Report(List<string> fields, string dataSource, List<string> conditions, List<string> totals)
        {
            Fields = fields;
            DataSource = dataSource;
            Conditions = conditions;
            Totals = totals;

            GenerateQuery();
        }
        
        public void GenerateQuery()
        {
            ColumnCount = 0;
            string fieldsQuery = string.Empty;
            foreach (string field in Fields)
            {
                fieldsQuery += field.ToUpper() + ", ";
                ColumnCount++;
            }
            fieldsQuery = fieldsQuery.Trim(' ');
            fieldsQuery = fieldsQuery.Trim(',');

            string conditionsString = string.Empty;
            foreach (string con in Conditions)
            {
                string[] conArray = con.Split(' ');
                conditionsString += conArray[0] + " " + conArray[1] + " ";
                if (float.TryParse(conArray[2], out float i))
                {
                    conditionsString += conArray[2] + " AND ";
                }
                else if (DateTime.TryParse(conArray[2], out DateTime date))
                {
                    if (conArray[1] == "<=")
                        date = date.AddDays(1);
                    conditionsString += @"""" + date.ToString("M/d/yyyy") + @""" AND ";
                }
                else if (DatabaseHandler.SqlRetrieveTableHeaders(DataSource).Contains(conArray[2]))
                {
                    conditionsString += conArray[2] + " AND ";
                }
                else if (conArray[2] == "DateTime.Today")
                {
                    date = DateTime.Now;
                    if (conArray[1] == "<=")
                        date = date.AddDays(1);
                    conditionsString += @"""" + date.ToString("M/d/yyyy") + @""" AND ";
                }
                else
                {
                    conditionsString += "\"" + conArray[2] + "\" AND ";
                }
            }
            if (conditionsString.EndsWith("AND "))
                conditionsString = conditionsString.Substring(0, conditionsString.Length - 5);

            Query =
                "SELECT " + fieldsQuery + " " +
                "FROM " + DataSource.ToUpper() + " " +
                "WHERE " + conditionsString;
        }

        public void ExecuteReport()
        {
            GenerateQuery();
            Results = DatabaseHandler.SqlReportQuery(Query, ColumnCount);
        }
    
        public void SaveReport(string reportName, string shortcode)
        {
            ReportName = reportName;
            ReportShortcode = shortcode;

            DatabaseHandler.SqlSaveReport(this);
        }

        public void RenderPage(XGraphics gfx)
        {
            totalPages = ((Results.Count / ColumnCount) / 40 + ((Results.Count / ColumnCount) % 40 > 0 ? 1 : 0));
            XFont font = new XFont("Times", 8);

            double currentLine = font.GetHeight() + 5;
            double columnWidth = (gfx.PageSize.Width - 5) / ColumnCount;
            int totalRows = 0;

            gfx.DrawString(DateTime.Now.ToString(), font, XBrushes.Black, 10, currentLine);
            gfx.DrawString(ReportName, font, XBrushes.Black, (gfx.PageSize.Width / 2) - gfx.MeasureString(ReportName, font).Width / 2, currentLine);
            gfx.DrawString("Page " + currentPage.ToString() + "/" + totalPages.ToString(), font, XBrushes.Black, 
                gfx.PageSize.Width - gfx.MeasureString("Page " + currentPage.ToString() + "/" + totalPages.ToString(), font).Width - 15, currentLine);
            currentLine += font.GetHeight();

            gfx.DrawString(ReportShortcode, font, XBrushes.Black, gfx.PageSize.Width - gfx.MeasureString(ReportShortcode, font).Width - 15, currentLine);
            gfx.DrawString("Source: " + DataSource, font, XBrushes.Black, 10, currentLine);
            currentLine += font.GetHeight();

            gfx.DrawLine(XPens.Black, 0, currentLine, gfx.PageSize.Width, currentLine);
            currentLine += font.GetHeight();

            for (int i = 0; i != Fields.Count; i++)
            {
                gfx.DrawString(Fields[i], font, XBrushes.Black, (columnWidth * i) + ((columnWidth - gfx.MeasureString(Fields[i], font).Width) / 2), currentLine);
            }

            for (int i = 0; i != Results.Count; i++)
            {
                if (i % ColumnCount == 0)
                {
                    currentLine += font.GetHeight();
                    totalRows++;
                }
                string result = double.TryParse(Results[i].ToString(), out double f) ? Math.Round(f, 2).ToString() : Results[i].ToString(); //Try parsing as a number to get rid of all those extra digits that don't need to be there
                gfx.DrawString(result, font, XBrushes.Black, (columnWidth * (i % ColumnCount)) + ((columnWidth - gfx.MeasureString(result, font).Width) / 2), currentLine);
            }
            currentLine += font.GetHeight();

            gfx.DrawString("Total Rows: " + totalRows.ToString(), font, XBrushes.Black, 10, currentLine + font.GetHeight());
            for (int i = 0; i != Totals.Count; i++)
            {
                currentLine += font.GetHeight();
                int totalColumnNumber = Fields.IndexOf(Totals[i]);
                float totalAmt = 0;

                for (int j = 0; j != totalRows; j++)
                {
                    int index = totalColumnNumber + (j * ColumnCount);
                    totalAmt += float.TryParse(Results[index].ToString(), out float f) ? (float)Math.Round(f, 2) : 0;
                }
                
                gfx.DrawString(Totals[i] + ": " + totalAmt.ToString("0.00"), font, XBrushes.Black, gfx.PageSize.Width - gfx.MeasureString(Totals[i] + ": " + totalAmt.ToString("0.00"), font).Width - 15, currentLine);
            }

            gfx.DrawString("TIMS - Total Inventory Management System | Reporting System", font, XBrushes.Black, 10, gfx.PageSize.Height - font.GetHeight() - 10);

            XGraphicsState state = gfx.Save();
            gfx.Restore(state);
        }
    }
}
