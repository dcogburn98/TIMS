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
        public static IngenicoResponse InitiatePayment(IngenicoRequest request)
        {
            XDocument Sendingxml = new XDocument(
                new XElement("DETAIL", 
                    new XElement("TRAN_TYPE", "CCR1"),
                    new XElement("AMOUNT", "10.00"),
                    new XElement("TRAN_FEE", "2.50")
                    ));

            IngenicoResponse resp = new IngenicoResponse(XDocument.Parse(HTTP.postXMLData("http://192.168.254.84:6200", Sendingxml)));
            return resp;
        }

        public static string InitiateSignatureCapture()
        {
            XDocument Sendingxml = new XDocument(
                new XElement("DETAIL",
                    new XElement("TRAN_TYPE", "SS01")
                    ));

            XDocument doc = XDocument.Parse(HTTP.postXMLData("http://192.168.254.84:6200", Sendingxml));
            return doc.ToString();
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
    }
}
