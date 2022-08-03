using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SQLite;

using TIMSServerModel;

namespace TIMSServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeModel" in both code and config file together.
    public class TIMSServiceModel : ITIMSServiceModel
    {
        public string CheckEmployee(string input)
        {
            if (!int.TryParse(input, out int v))
                input = "'" + input + "'";
            string value = null;
            Program.OpenConnection();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT FULLNAME " +
                "FROM EMPLOYEES " +
                "WHERE USERNAME = " + input + " " +
                "OR EMPLOYEENUMBER = " + input;

            SQLiteDataReader rdr = sqlite_cmd.ExecuteReader();
            while (rdr.Read())
            {
                value = $"{rdr.GetString(0)}";
            }

            Program.CloseConnection();

            if (value == null)
                return null;
            else
                return value;
        }
    }
}
