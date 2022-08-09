using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.Planogram.Shelving
{
    class GondolaRow
    {
        public bool doubleSided;
        public List<Gondola> shelvesSide1;
        public List<Gondola> shelvesSide2;
    }
}
