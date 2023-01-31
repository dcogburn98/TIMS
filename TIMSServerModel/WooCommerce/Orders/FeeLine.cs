using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.WooCommerce.Orders
{
    public class FeeLine
    {
        public int id;
        public string name;
        public string taxClass;
        public string taxStatus;
        public decimal total;
        public decimal totalTax;
        public List<Tax> taxes;
        public List<Metadata> metadata;
    }
}
