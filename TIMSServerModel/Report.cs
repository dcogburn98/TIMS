﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfSharp.Drawing;

namespace TIMSServerModel
{
    [DataContract]
    public class Report
    {
        [DataMember]
        public string ReportName;
        [DataMember]
        public string ReportShortcode;

        [DataMember]
        public List<string> Fields;
        [DataMember]
        public string DataSource;
        [DataMember]
        public List<string> Conditions;
        [DataMember]
        public List<string> Totals;

        [DataMember]
        public string Query;
        [DataMember]
        public int ColumnCount;
        [DataMember]
        public List<object> Results;

        [DataMember]
        public int totalPages;
        [DataMember]
        public int currentPage;
        [DataMember]
        public int pageRows = 60;

        [DataMember]
        public List<string> tableheaders = new List<string>();

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
                else if (tableheaders.Contains(conArray[2]))
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

        public void SaveReport(string reportName, string shortcode)
        {
            ReportName = reportName;
            ReportShortcode = shortcode;
        }

        public void RenderPage(XGraphics gfx)
        {
            int rows = Results.Count / ColumnCount;
            totalPages = (rows / pageRows + (rows % pageRows > 0 ? 1 : 0));
            XFont font = new XFont("Times", 8.5d);

            double currentLine = font.GetHeight() + 5;
            double columnWidth = (gfx.PageSize.Width - 15 - 5) / ColumnCount;

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

            for (int i = 0; i != pageRows * Fields.Count; i++)
            {
                if (((currentPage - 1) * (Fields.Count * pageRows)) + i > Results.Count - 1)
                    break;

                if (i % ColumnCount == 0)
                {
                    currentLine += font.GetHeight();
                }
                string result = double.TryParse(Results[((currentPage - 1) * (Fields.Count * pageRows)) + i].ToString(), out double f) ? Math.Round(f, 2).ToString() : Results[((currentPage - 1) * (Fields.Count * pageRows)) + i].ToString(); //Try parsing as a number to get rid of all those extra digits that don't need to be there
                gfx.DrawString(result, font, XBrushes.Black, (columnWidth * (i % ColumnCount)) + ((columnWidth - gfx.MeasureString(result, font).Width) / 2), currentLine);
            }
            currentLine += font.GetHeight();

            if (currentPage == totalPages)
            {
                gfx.DrawString("Total Rows: " + rows, font, XBrushes.Black, 10, currentLine + font.GetHeight());
                for (int i = 0; i != Totals.Count; i++)
                {
                    currentLine += font.GetHeight();
                    int totalColumnNumber = Fields.IndexOf(Totals[i]);
                    float totalAmt = 0;

                    for (int j = 0; j != rows; j++)
                    {
                        int index = totalColumnNumber + (j * ColumnCount);
                        totalAmt += float.TryParse(Results[index].ToString(), out float f) ? (float)Math.Round(f, 2) : 0;
                    }

                    gfx.DrawString(Totals[i] + ": " + totalAmt.ToString("0.00"), font, XBrushes.Black, gfx.PageSize.Width - gfx.MeasureString(Totals[i] + ": " + totalAmt.ToString("0.00"), font).Width - 15, currentLine);
                }
            }

            gfx.DrawString("TIMS - Total Inventory Management System | Reporting System", font, XBrushes.Black, 10, gfx.PageSize.Height - font.GetHeight() - 10);

            XGraphicsState state = gfx.Save();
            gfx.Restore(state);
        }
    }
}
