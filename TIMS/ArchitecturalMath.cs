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
        public static int PointsPerFoot = 3;

        public static int FeetFromPoints(int points)
        {
            return points / PointsPerFoot;
        }
    }
}
