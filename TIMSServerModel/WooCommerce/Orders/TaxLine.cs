using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.WooCommerce.Orders
{
    public class TaxLine
    {
        public int id;
        public string rateCode;
        public string rateID;
        public string label;
        public bool compound;
        public decimal taxTotal;
        public decimal shippingTaxTotal;
        public List<Metadata> metadata;

        public TaxLine()
        {
            metadata = new List<Metadata>();
        }
    }
}
