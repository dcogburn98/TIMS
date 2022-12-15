using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS
{
    class ArchitecturalMath
    {
        public static decimal PointsPerFoot = 3;

        public static decimal FeetFromPoints(decimal points)
        {
            return points / PointsPerFoot;
        }
    }
}
