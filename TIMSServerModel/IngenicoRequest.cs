using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TIMSServerModel
{
    [DataContract]
    public class IngenicoRequest
    {
        [DataMember]
        public string RawXMLRequest;

        public enum TRAN_TYPE_TYPES
        {
            /// <summary>
            /// Retail Account Verification transaction
            /// </summary>
            CCR0,

            /// <summary>
            /// Retail Purchase Authorization & Capture transaction
            /// </summary>
            CCR1,

            /// <summary>
            /// Retail Purchase Authorization Only transaction
            /// </summary>
            CCR2,

            /// <summary>
            /// Retail Purchase Capture Only transaction
            /// </summary>
            CCR4,

            /// <summary>
            /// Retail Purchase Authorization Reversal
            /// </summary>
            CCR7,

            /// <summary>
            /// Retail BRIC Storage or Updates transaction
            /// </summary>
            CCR8,

            /// <summary>
            /// Retail Return Capture transaction
            /// </summary>
            CCR9,

            /// <summary>
            /// Retail Return Authorization & Capture transaction
            /// </summary>
            CCRA,

            /// <summary>
            /// Retail Return Authorization & Capture Hold Release
            /// </summary>
            CCRG,

            /// <summary>
            /// Retail Edit Authorization and Capture transaction
            /// </summary>
            CCRN,

            /// <summary>
            /// Retail Void transaction
            /// </summary>
            CCRX,

            /// <summary>
            /// Open to Buy/Balance Inquiry transaction. Citi Private Label only
            /// </summary>
            CCRY,

            /// <summary>
            /// Retail Close Batch transaction
            /// </summary>
            CCRZ,

            /// <summary>
            /// Sending a signature capture
            /// </summary>
            SS01
        }

        public IngenicoRequest(TRAN_TYPE_TYPES TransactionType, decimal? TransactionAmount = null, 
            int? AccountNumber = null, long? BatchID = null, decimal? CashBackAmount = null, decimal? ConvenienceFee = null, bool SignatureRequired = false)
        {
            string amount = null;
            string accountnbr = null;
            string batchid = null;
            string cashbkamt = null;
            string conveniencefee = null;

            string trantype = Enum.GetName(typeof(TRAN_TYPE_TYPES), TransactionType);

            XDocument xml = new XDocument(
                new XElement("DETAIL",
                    new XElement("TRAN_TYPE", trantype)));

            if (TransactionAmount != null)
            {
                amount = Math.Round((double)TransactionAmount, 2).ToString("C");
                amount = amount.Substring(1, amount.Length - 1);
            }

            if (AccountNumber != null)
                accountnbr = AccountNumber.ToString();

            if (BatchID != null)
                batchid = BatchID.ToString();

            if (CashBackAmount != null)
            {
                cashbkamt = Math.Round((double)CashBackAmount, 2).ToString("C");
                cashbkamt = cashbkamt.Substring(1, cashbkamt.Length - 1);
            }

            if (ConvenienceFee != null)
            {
                conveniencefee = Math.Round((double)ConvenienceFee, 2).ToString("C");
                conveniencefee = conveniencefee.Substring(1, conveniencefee.Length - 1);
            }

            if (amount != null)
                xml.Element("DETAIL").Add(new XElement("AMOUNT", amount));
            if (accountnbr != null)
                xml.Element("DETAIL").Add(new XElement("ACCOUNT_NBR", accountnbr));
            if (batchid != null)
                xml.Element("DETAIL").Add(new XElement("BATCH_ID", batchid));
            if (cashbkamt != null)
                xml.Element("DETAIL").Add(new XElement("CASH_BK_AMT", cashbkamt));
            if (conveniencefee != null)
                xml.Element("DETAIL").Add(new XElement("CONVENIENCE_FEE", conveniencefee));
            if (SignatureRequired == true || TransactionType == TRAN_TYPE_TYPES.CCRA)
                xml.Element("DETAIL").Add(new XElement("SI_SIGNATURE_REQUIRED", "Y"));

            RawXMLRequest = xml.ToString();
        }
    
        public IngenicoRequest(string xml)
        {
            RawXMLRequest = xml;
        }
    }
}
