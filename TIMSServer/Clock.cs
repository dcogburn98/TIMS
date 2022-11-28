using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServer
{
    public class Clock
    {
        private int SkippedDays = 0;

        public DateTime Now()
        {
#if DEBUG
            return DateTime.Now.AddDays(SkippedDays);
#else
            return DateTime.Now;
#endif
        }

        public Clock()
        {

        }
    }
}
