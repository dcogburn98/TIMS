using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.WooCommerce.Products
{
    public class Attribute
    {
        public int id;
        public string name;
        public int position;
        public bool visible;
        public bool variation;
        public List<string> options;
    }
}
