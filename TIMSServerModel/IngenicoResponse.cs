using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class IngenicoResponse
    {
        public XDocument RawXMLResponse;

        public int InvoiceNumber;
        public char CardEntryMethod;
        public bool SignatureRequired;


        public IngenicoResponse(XDocument responseData)
        {
            RawXMLResponse = responseData;

        }
    }
}
