using System;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static Payment InitiatePayment(Device device, decimal amount, decimal cashBackAmount, decimal convenienceFee, int batchID, int customerNumber, bool signatureRequired)
        {
            bool amountProvided = false;
            bool cashBackAmountProvided = false;
            bool convenienceFeeProvided = false;
            bool batchIDProvided = false;
            bool customerNumberProvided = false;

            if (amount != 0)
                amountProvided = true;
            if (cashBackAmount != 0)
                cashBackAmountProvided = true;
            if (convenienceFee != 0)
                convenienceFeeProvided = true;
            if (batchID != 0)
                batchIDProvided = true;
            if (customerNumber != 0)
                customerNumberProvided = true;

            IngenicoResponse resp = SendRequest(
                new IngenicoRequest(
                    IngenicoRequest.TRAN_TYPE_TYPES.CCR1,
                    amountProvided ? (amount as decimal?) : null,
                    customerNumberProvided ? (customerNumber as int?) : null,
                    batchIDProvided ? (batchID as int?) : null,
                    cashBackAmountProvided ? (cashBackAmount as decimal?) : null,
                    convenienceFeeProvided ? (convenienceFee as decimal?) : null,
                    signatureRequired
                    ),
                device);

            Payment payment = new Payment()
            {
                cardResponse = resp,
                paymentAmount = resp.CapturedAmount,
                paymentType = Payment.PaymentTypes.PaymentCard
            };
            return payment;
        }

        public static Payment InitiateRefund(Device device, decimal amount)
        {
            IngenicoResponse resp = SendRequest(
                new IngenicoRequest(
                    IngenicoRequest.TRAN_TYPE_TYPES.CCRA,
                    amount),
                device);

            Payment payment = new Payment()
            {
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

        public static string VoidPayment(decimal payment)
        {
            XDocument Sendingxml = new XDocument(
                new XElement("DETAIL",
                    new XElement("TRAN_TYPE", "CCRX"),
                    new XElement("AMOUNT", payment)
                    ));

            XDocument doc = XDocument.Parse(HTTP.postXMLData("http://192.168.254.84:6200", Sendingxml));
            return doc.ToString();
        }

        public static string AbortPayment()
        {
            XDocument Sendingxml = new XDocument(
                new XElement("DETAIL",
                    new XElement("ABORT", "1")
                    ));

            XDocument doc = XDocument.Parse(HTTP.postXMLData("http://192.168.254.84:6200", Sendingxml));
            return doc.ToString();
        }

        internal static string SendRawRequest(Device device, IngenicoRequest request)
        {
            return SendRequest(request, device).RawXMLResponse;
        }
    }
}
