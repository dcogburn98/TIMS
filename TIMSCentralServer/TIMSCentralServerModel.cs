using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

using TIMSServerModel;

namespace TIMSCentralServer
{
    public class TIMSCentralServerModel : ITIMSCentralServerModel
    {
        private void OpenConnection() => Program.OpenConnection();
        private void CloseConnection() => Program.CloseConnection();
        private SqliteConnection connection => Program.sqlite_conn;

        public List<Item> CheckItemNumber(string itemNumber, string key)
        {
            if (connection.State == ConnectionState.Closed)
                OpenConnection();

            if (connection.State == ConnectionState.Open)
                CloseConnection();

            throw new NotImplementedException();
        }

        public bool CheckProductLine(string productLine, string key)
        {
            if (connection.State == ConnectionState.Closed)
                OpenConnection();



            if (connection.State == ConnectionState.Open)
                CloseConnection();

            throw new NotImplementedException();
        }

        public bool RequestProductLineRegistration(string productLine, string key)
        {
            throw new NotImplementedException();
        }

        public byte[] RetrieveItemImages(Item item, string key)
        {
            throw new NotImplementedException();
        }

        public byte[] RetrieveItemModel(Item item, string key)
        {
            throw new NotImplementedException();
        }

        public bool AddBrandToProductLine(string productLine, string brand, string key)
        {
            throw new NotImplementedException();
        }

        public bool CheckKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool RequestProductLineRegistration(string productLine, string description, string[] brands, string key)
        {
            throw new NotImplementedException();
        }
    }
}
