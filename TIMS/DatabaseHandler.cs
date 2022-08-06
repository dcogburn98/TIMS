using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using TIMS.Server;
using TIMSServerModel;

namespace TIMS
{
    public partial class DatabaseHandler
    {
        public static SQLiteConnection sqlite_conn;

        public static void InitializeDatabases()
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
