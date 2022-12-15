using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

namespace TIMSCentralServer
{
    internal class Program
    {
        public static SqliteConnection sqlite_conn;

        static void Main(string[] args)
        {
            //This project is for the central TIMS server to regulate the usage of product lines
            //The central TIMS server will also provide media such as images and models for 
            //items that are in instances of TIMS running in remote locations

            //This project also handles licensing requirements and publishing of new versions of TIMS

            sqlite_conn = new SqliteConnection(
              @"Data Source=database.db; 
                Pooling = true;");
            //Password = 3nCryqtEdT!MSPa$$w0rdFoRrev!tAc0m;

            OpenConnection();
            CloseConnection();
            CreateDatabase();

            ServiceHost host = new ServiceHost(typeof(TIMSCentralServerModel));
            host.Open();
        }

        public static void OpenConnection()
        {
            sqlite_conn.Open();
        }

        public static void CloseConnection()
        {
            sqlite_conn.Close();
        }

        public static void CreateDatabase()
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            if (!TableExists("ProductLines"))
            {
                command.CommandText =
                    @"CREATE TABLE ""ProductLines"" (
	                ""ProductLine""	    TEXT NOT NULL UNIQUE,
	                ""LineDescription""	TEXT NOT NULL,
	                ""DateRegistered""	TEXT NOT NULL,
	                ""BrandsIncluded""	TEXT NOT NULL,
	                PRIMARY KEY(""ProductLine"")
                    )";
                command.ExecuteNonQuery();
            }

            if (!TableExists("LicenseKeys"))
            {
                command.CommandText =
                    @"CREATE TABLE ""LicenseKeys"" (
	                ""Key""	            TEXT NOT NULL UNIQUE,
	                ""IssueDate""	    TEXT NOT NULL,
	                ""ExpirationDate""	TEXT NOT NULL,
	                ""LicenseHolder""	TEXT NOT NULL,
	                PRIMARY KEY(""Key"")
                    )";
                command.ExecuteNonQuery();
            }
        }

        public static bool TableExists(string tableName)
        {
            var sql =
            "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "';";
            if (sqlite_conn.State == System.Data.ConnectionState.Open)
            {
                SqliteCommand command = new SqliteCommand(sql, sqlite_conn);
                SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                return false;
            }
            else
            {
                throw new System.ArgumentException("Data.ConnectionState must be open");
            }
        }
    }
}
