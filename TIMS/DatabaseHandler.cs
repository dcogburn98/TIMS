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

        #region Purchase orders and check-ins
        public static int SqlRetrieveNextPONumber()
        {
            int PONumber = 0;
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT MAX(PONUMBER) FROM PURCHASEORDERS";
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return PONumber;
            }

            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    CloseConnection();
                    return PONumber;
                }
                PONumber = reader.GetInt32(0) + 1;
            }

            CloseConnection();
            return PONumber;
        }
        
        public static List<PurchaseOrder> SqlRetrievePurchaseOrders()
        {
            List<int> poNumbers = new List<int>();
            List<PurchaseOrder> order = new List<PurchaseOrder>();
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT PONUMBER FROM PURCHASEORDERS";
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }
            while (reader.Read())
                poNumbers.Add(reader.GetInt32(0));
            CloseConnection();

            foreach (int po in poNumbers)
                order.Add(SqlRetrievePurchaseOrder(po));
            return order;
        }

        public static PurchaseOrder SqlRetrievePurchaseOrder(int PONumber, bool connectionOpened = false)
        {
            PurchaseOrder po = new PurchaseOrder("NONE");
            if (!connectionOpened)
                OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM PURCHASEORDERS WHERE PONUMBER = $PONUMBER";
            SQLiteParameter p1 = new SQLiteParameter("$PONUMBER", PONumber);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                if (!connectionOpened)
                    CloseConnection();
                return null;
            }
            while (reader.Read())
            {
                po.PONumber = reader.GetInt32(0);
                po.totalCost = reader.GetDecimal(1);
                po.totalItems = reader.GetDecimal(2);
                po.assignedCheckin = reader.GetInt32(3);
                po.supplier = reader.GetString(4);
                po.finalized = reader.GetBoolean(5);
                po.shippingCost = reader.GetDecimal(6);
            }
            reader.Close();

            command.CommandText =
                "SELECT * FROM PURCHASEORDERITEMS WHERE PONUMBER = $PONUMBER";
            reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                if (!connectionOpened)
                    CloseConnection();
                return null;
            }
            while (reader.Read())
            {
                InvoiceItem i = new InvoiceItem();
                i.itemNumber = reader.GetString(2);
                i.productLine = reader.GetString(3);
                i.quantity = reader.GetDecimal(4);
                i.cost = reader.GetDecimal(5);
                i.price = reader.GetDecimal(6);
                po.items.Add(i);
            }

            if (!connectionOpened)
                CloseConnection();
            return po;
        }
        
        public static void SqlSavePurchaseOrder(PurchaseOrder PO)
        {
            bool exists = false;
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM PURCHASEORDERS WHERE PONUMBER = $PO";
            SQLiteParameter p8 = new SQLiteParameter("$PO", PO.PONumber);
            command.Parameters.Add(p8);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                exists = true;
            reader.Close();
            command.Parameters.Clear();

            if (exists)
                SqlDeletePurchaseOrder(PO.PONumber, true);

            command.CommandText =
                "INSERT INTO PURCHASEORDERS (" +
                "PONUMBER, TOTALCOST, TOTALITEMS, ASSIGNEDCHECKIN, SUPPLIER, FINALIZED, SHIPPINGCOST) " +
                "VALUES ($PONUMBER, $COST, $TOTALITEMS, $CHECKIN, $SUPPLIER, $FINALIZED, $SHIPPING)";
            SQLiteParameter p1 = new SQLiteParameter("$PONUMBER", PO.PONumber);
            SQLiteParameter p2 = new SQLiteParameter("$COST", PO.totalCost);
            SQLiteParameter p3 = new SQLiteParameter("$TOTALITEMS", PO.totalItems);
            SQLiteParameter p4 = new SQLiteParameter("$CHECKIN", PO.assignedCheckin);
            SQLiteParameter p5 = new SQLiteParameter("$SUPPLIER", PO.supplier);
            SQLiteParameter p6 = new SQLiteParameter("$FINALIZED", PO.finalized);
            SQLiteParameter p7 = new SQLiteParameter("$SHIPPING", PO.shippingCost);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            command.Parameters.Add(p5);
            command.Parameters.Add(p6);
            command.Parameters.Add(p7);

            command.ExecuteNonQuery();

            command.CommandText =
                "INSERT INTO PURCHASEORDERITEMS (" +
                "PONUMBER, ID, ITEMNUMBER, PRODUCTLINE, QUANTITY, COST, PRICE) " +
                "VALUES ($PONUMBER, $ID, $ITEMNUMBER, $PRODUCTLINE, $QTY, $COST, $PRICE)";

            foreach (InvoiceItem item in PO.items)
            {
                p1 = new SQLiteParameter("$PONUMBER", PO.PONumber);
                p2 = new SQLiteParameter("$ID", Guid.NewGuid());
                p3 = new SQLiteParameter("$ITEMNUMBER", item.itemNumber);
                p4 = new SQLiteParameter("$PRODUCTLINE", item.productLine);
                p5 = new SQLiteParameter("$QTY", item.quantity);
                p6 = new SQLiteParameter("$COST", item.cost);
                p7 = new SQLiteParameter("$PRICE", item.price);
                command.Parameters.Clear();
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);
                command.Parameters.Add(p4);
                command.Parameters.Add(p5);
                command.Parameters.Add(p6);
                command.Parameters.Add(p7);
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }
        
        public static void SqlFinalizePurchaseOrder(PurchaseOrder PO)
        {
            SqlDeletePurchaseOrder(PO.PONumber);

            SqlSavePurchaseOrder(PO);
        }
        
        public static void SqlDeletePurchaseOrder(int PONumber, bool connectionOpened = false)
        {
            if (!connectionOpened)
                OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "DELETE FROM PURCHASEORDERS WHERE PONUMBER = $PO";
            SQLiteParameter p1 = new SQLiteParameter("$PO", PONumber);
            command.Parameters.Add(p1);

            command.ExecuteNonQuery();

            command.CommandText =
                "DELETE FROM PURCHASEORDERITEMS WHERE PONUMBER = $PO";
            command.ExecuteNonQuery();

            if (!connectionOpened)
                CloseConnection();
        }

        public static int SqlRetrieveNextCheckinNumber()
        {
            int checkinNumber = 10000;
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT MAX(PONUMBER) FROM CHECKINS";
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return checkinNumber;
            }

            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    CloseConnection();
                    return checkinNumber;
                }
                checkinNumber = reader.GetInt32(0) + 1;
            }

            CloseConnection();
            return checkinNumber;
        }
        
        public static void SqlSaveCheckin(Checkin checkin)
        {
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO CHECKINS (CHECKINNUMBER, PONUMBERS)";

            CloseConnection();
            return;
        }

        public static List<Checkin> SqlRetrieveCheckins()
        {
            List<int> checkinNumbers = new List<int>();
            List<Checkin> checkins = new List<Checkin>();
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT PONUMBER FROM PURCHASEORDERS";
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }
            while (reader.Read())
                checkinNumbers.Add(reader.GetInt32(0));
            CloseConnection();

            foreach (int checkinNumber in checkinNumbers)
                checkins.Add(SqlRetrieveCheckin(checkinNumber));
            return checkins;
        }

        public static Checkin SqlRetrieveCheckin(int checkinNumber)
        {
            Checkin checkin = new Checkin();
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM CHECKINS WHERE CHECKINNUMBER = $CHECKINNUMBER";
            SQLiteParameter p1 = new SQLiteParameter("$CHECKINNUMBER", checkinNumber);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }
            while (reader.Read())
            {
                checkin.checkinNumber = reader.GetInt32(0);
                foreach (string orderNo in reader.GetString(1).Split(','))
                {
                    PurchaseOrder number = SqlRetrievePurchaseOrder(int.Parse(orderNo), true);
                    checkin.orders.Add(number);
                }
            }
            reader.Close();

            command.CommandText =
                "SELECT * FROM PURCHASEORDERITEMS WHERE PONUMBER = $PONUMBER";
            reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }
            while (reader.Read())
            {
                InvoiceItem i = new InvoiceItem();
                i.itemNumber = reader.GetString(2);
                i.productLine = reader.GetString(3);
                i.quantity = reader.GetDecimal(4);
                i.cost = reader.GetDecimal(5);
                i.price = reader.GetDecimal(6);
                //items.Add(i);
            }

            CloseConnection();
            return checkin;
        }
        #endregion

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
