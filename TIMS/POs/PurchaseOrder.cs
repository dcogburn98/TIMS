using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TIMSServerModel;

namespace TIMS.POs
{
    public class PurchaseOrder
    {
        public List<InvoiceItem> Items = new List<InvoiceItem>();

        public string PurchaseOrderNumber;
        public string Supplier;
    }
}
