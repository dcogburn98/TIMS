using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.Planogram.Shelving
{
    public class GondolaRow
    {
        public bool doubleSided;
        public List<Gondola> shelvesSide1;
        public List<Gondola> shelvesSide2;
    }
}
