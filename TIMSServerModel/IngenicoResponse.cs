using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    [DataContract]
    public class IngenicoResponse
    {
        [DataMember]
        public string RawXMLResponse;

        [DataMember]
        public string AuthCode;
        [DataMember]
        public string MaskedCardNumber;
        [DataMember]
        public string FirstName;
        [DataMember]
        public string LastName;
        [DataMember]
        public int CustomerNumber;
        [DataMember]
        public int BatchID;
        [DataMember]
        public decimal RequestedAmount;
        [DataMember]
        public decimal CapturedAmount;
        [DataMember]
        public string ID;
        [DataMember]
        public string Signature;
        [DataMember]
        public string AppLabel;
        [DataMember]
        public string TVR;
        [DataMember]
        public string AID;
        [DataMember]
        public string TSI;

        public IngenicoResponse(XDocument responseData)
        {
            RawXMLResponse = responseData.ToString();

            foreach (XElement el in responseData.Element("RESPONSE").Elements())
            {
                if (el.Name == "AUTH_CODE")
                    AuthCode = el.Value;
                if (el.Name == "AUTH_MASKED_ACCOUNT_NBR")
                    MaskedCardNumber = el.Value;
                if (el.Name == "FIRST_NAME")
                    FirstName = el.Value;
                if (el.Name == "LAST_NAME")
                    LastName = el.Value;
                if (el.Name == "CUST_NBR")
                    CustomerNumber = int.Parse(el.Value);
                if (el.Name == "BATCH_ID")
                    BatchID = int.Parse(el.Value);
                if (el.Name == "AUTH_AMOUNT_REQUESTED")
                    RequestedAmount = decimal.Parse(el.Value);
                if (el.Name == "AUTH_AMOUNT")
                    CapturedAmount = decimal.Parse(el.Value);
                if (el.Name == "AUTH_GUID")
                    ID = el.Value;
                if (el.Name == "SIGNATURE")
                    Signature = el.Value;
                if (el.Name == "SI_EMV_APP_LABEL")
                    AppLabel = el.Value;
                if (el.Name == "SI_EMV_TVR")
                    TVR = el.Value;
                if (el.Name == "SI_EMV_AID")
                    AID = el.Value;
                if (el.Name == "SI_EMV_TSI")
                    TSI = el.Value;
            }
        }
    }
}
