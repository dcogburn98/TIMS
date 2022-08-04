using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TIMSServerModel;

namespace TIMS
{
    public class CheckinItem : InvoiceItem
    {
        public decimal shipped = 0;
        public decimal received = 0;
        public decimal damaged = 0;
    }
}
