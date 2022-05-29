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
                else if (DatabaseHandler.SqlRetrieveTableHeaders(dataSource).Contains(conArray[2]))
                {
                    conditionsString += conArray[2] + " AND ";
                }
                else
                {
                    conditionsString += "\"" + conArray[2]  + "\" " + " AND ";
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

            gfx.DrawString(ReportName, font, XBrushes.Black, gfx.PageSize.Width / 2, 5);

            XGraphicsState state = gfx.Save();
            gfx.Restore(state);
        }
    }
}
