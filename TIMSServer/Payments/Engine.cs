using System;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

using TIMSServerModel;

namespace TIMSServer.Payments
{
    public class Engine
    {
        private static IngenicoResponse SendRequest(IngenicoRequest request, Device device)
        {
            IngenicoResponse resp = new IngenicoResponse(
                XDocument.Parse(HTTP.postXMLData("http://" + device.address.Address.ToString() + ":" + device.address.Port, XDocument.Parse(request.RawXMLRequest))));
            return resp;
        }

        public static Payment InitiatePayment(Device device, decimal amount, decimal cashBackAmount, decimal convenienceFee, int customerNumber)
        {
            bool amountProvided = false;
            bool cashBackAmountProvided = false;
            bool convenienceFeeProvided = false;
            bool customerNumberProvided = false;

            if (amount != 0)
                amountProvided = true;
            if (cashBackAmount != 0)
                cashBackAmountProvided = true;
            if (convenienceFee != 0)
                convenienceFeeProvided = true;
            if (customerNumber != 0)
                customerNumberProvided = true;
            int transactionID = 1;
            int batchID = 1;

            Program.OpenConnection();

            SqliteCommand command = Program.sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = ""Current Transaction Number"" ";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                transactionID = 1;
            else
                while (reader.Read())
                {
                    transactionID = reader.GetInt32(0);
                }
            reader.Close();
            command.CommandText =
                @"UPDATE GLOBALPROPERTIES SET VALUE = $TID WHERE KEY = ""Current Transaction Number""";
            command.Parameters.Add(new SqliteParameter("$TID", transactionID + 1));
            command.ExecuteNonQuery();
            command.Parameters.Clear();

            command.CommandText =
                @"SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = ""Current Batch Number"" ";
            reader = command.ExecuteReader();
            if (!reader.HasRows)
                batchID = 1;
            else
                while (reader.Read())
                {
                    batchID = reader.GetInt32(0);
                }
            reader.Close();
            Program.CloseConnection();

            IngenicoRequest req = new IngenicoRequest(
                    TransactionType:    IngenicoRequest.TRAN_TYPE_TYPES.CCR1,
                    TransactionAmount:  amountProvided ? (amount as decimal?) : null,
                    TranNBR:            transactionID,
                    BatchID:            batchID,
                    AccountNumber:      customerNumberProvided ? (customerNumber as int?) : null,
                    CashBackAmount:     cashBackAmountProvided ? (cashBackAmount as decimal?) : null,
                    ConvenienceFee:     convenienceFeeProvided ? (convenienceFee as decimal?) : null
                    );

            IngenicoResponse resp = SendRequest(req, device);
            Payment payment = new Payment()
            {
                cardRequest = req,
                cardResponse = resp,
                paymentAmount = resp.CapturedAmount,
                paymentType = Payment.PaymentTypes.PaymentCard,
            };
            return payment;
        }

        public static Payment InitiateRefund(Device device, decimal amount)
        {
            int transactionID = 1;
            int batchID = 1;
            Program.OpenConnection();

            SqliteCommand command = Program.sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = ""Current Transaction Number"" ";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                transactionID = 1;
            else
                while (reader.Read())
                {
                    transactionID = reader.GetInt32(0);
                }
            reader.Close();
            command.CommandText =
                @"UPDATE GLOBALPROPERTIES SET VALUE = $TID WHERE KEY = ""Current Transaction Number""";
            command.Parameters.Add(new SqliteParameter("$TID", transactionID + 1));
            command.ExecuteNonQuery();

            command.Parameters.Clear();
            command.CommandText =
                @"SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = ""Current Batch Number"" ";
            reader = command.ExecuteReader();
            if (!reader.HasRows)
                batchID = 1;
            else
                while (reader.Read())
                {
                    batchID = reader.GetInt32(0);
                }
            reader.Close();

            Program.CloseConnection();
            IngenicoRequest req = new IngenicoRequest(
                TransactionType:    IngenicoRequest.TRAN_TYPE_TYPES.CCR9, 
                TransactionAmount:  amount,
                TranNBR:            transactionID,
                BatchID:            batchID);

            IngenicoResponse resp = SendRequest(req, device);
            Payment payment = new Payment()
            {
                cardRequest = req,
                cardResponse = resp,
                paymentAmount = 0 - resp.CapturedAmount,
                paymentType = Payment.PaymentTypes.PaymentCard
            };
            return payment;
        }

        public static string InitiateSignatureCapture(Device device)
        {
            IngenicoResponse resp = SendRequest(
                new IngenicoRequest(
                    IngenicoRequest.TRAN_TYPE_TYPES.SS01),
                device);

            return resp.Signature;
        }

        internal static string SendRawRequest(Device device, IngenicoRequest request)
        {
            return SendRequest(request, device).RawXMLResponse;
        }
    }
}
