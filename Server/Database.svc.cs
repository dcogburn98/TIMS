using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Database" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Database.svc or Database.svc.cs at the Solution Explorer and start debugging.
    public class Database : IDatabase
    {
        public static SQLiteConnection sqlite_conn;

        public void InitializeDatabase()
        {
            sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Pooling = true; Max Pool Size = 100; Compress = True; ");
            OpenConnection();
            CloseConnection();
        }

        public static void OpenConnection()
        {
            sqlite_conn.Open();
        }

        public static void CloseConnection()
        {
            sqlite_conn.Close();
        }

    }
}
