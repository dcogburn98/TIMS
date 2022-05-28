using System;
using System.Collections.Generic;

namespace TIMS
{
    class Report
    {
        public List<string> Fields;
        public string DataSource;
        public List<string> Conditions;
        public List<string> Totals;

        public string Query;
        public int ColumnCount;
        public List<object> Results;

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
            Results = DatabaseHandler.SqlGeneralQuery(Query, ColumnCount);
        }
    }
}
