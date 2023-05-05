using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

using TIMSServerModel;

using Quartz;

namespace TIMSServer
{
    internal class EOD : IJob
    {
        public static TIMSServiceModel instance;

        public async Task Execute(IJobExecutionContext context)
        {
"EOD Begin".LogEOD(LogLevel.Info);
            Program.OpenConnection();

"Invoice Costing Begin".LogEOD(LogLevel.Info);
            List<Invoice> TodaysInvoices = instance.RetrieveInvoicesByDateRange(DateTime.Today.AddDays(-1), DateTime.Today);
            if (TodaysInvoices != null && TodaysInvoices.Count > 0)
                foreach (Invoice inv in TodaysInvoices)
                {
                    Console.WriteLine(inv.invoiceNumber.ToString());
                }
"Invoice Costing End".LogEOD(LogLevel.Info);

            #region Integrated Payments Batch Operations
"Integrated Payments Batch Operations Begin".LogEOD(LogLevel.Info);
            int batchID = 1;
            
            SqliteCommand command = Program.sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = ""Current Batch Number"" ";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                batchID = 1;
            else
                while (reader.Read())
                {
                    batchID = reader.GetInt32(0);
                }
            reader.Close();
            command.CommandText =
                "UPDATE GLOBALPROPERTIES SET VALUE = $BID WHERE KEY = \"Current Batch Number\"";
            command.Parameters.Add(new SqliteParameter("$BID", batchID + 1));
            command.ExecuteNonQuery();
            #endregion

            Program.CloseConnection();

            await Task.Run(() => {
                
                Console.WriteLine("Started End of Day...");
            });
        }
    }
}