using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.WooCommerce.Orders
{
    public class CouponLine
    {
        public int id;
        public string code;
        public decimal discount;
        public decimal discountTax;
        public List<Metadata> metadata;
    }
}
