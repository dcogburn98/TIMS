using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class CheckinItem : InvoiceItem
    {
        public decimal shipped = 0;
        public decimal received = 0;
        public decimal damaged = 0;
    }
}
