using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.WooCommerce.Orders
{
    public class LineItem
    {
        public int id;
        public string name;
        public int productID;
        public int variationID;
        public int quantity;
        public string taxClass;
        public decimal subtotal;
        public decimal subtotalTax;
        public decimal total;
        public decimal totalTax;
        public List<Tax> taxes;
        public List<Metadata> metadata;
        public string sku;
        public decimal price;

        public LineItem()
        {
            taxes = new List<Tax>();
            metadata = new List<Metadata>();
        }
    }
}
