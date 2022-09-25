using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    class AuthContainer<DataType>
    {
        public DataType Data;
        public AuthKey Key;

        public AuthContainer()
        {
            
        }
    }
}
